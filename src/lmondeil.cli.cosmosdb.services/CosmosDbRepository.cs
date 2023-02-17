namespace lmondeil.cli.cosmosdb.services
{
    using lmondeil.cli.cosmosdb.services.Helpers;
    using lmondeil.cli.cosmosdb.services.Models;

    using Microsoft.Azure.Cosmos;
    using Microsoft.Extensions.Logging;

    using Newtonsoft.Json.Linq;

    using System.Net;
    using System.Text.Json;

    public class CosmosDbRepository
    {
        private Container _container;
        private readonly ILogger? _logger;

        public CosmosDbRepository(string connectionString, string database, string containerName, ILogger? logger = null)
        {
            CosmosClient client = new CosmosClient(connectionString, new CosmosClientOptions
            {
                ConnectionMode = ConnectionMode.Gateway
            });

            this._container = client.GetContainer(database, containerName);
            _logger = logger;
        }

        public async IAsyncEnumerable<dynamic> SelectAsync(string query)
        {
            FeedIterator<dynamic> iterator = this._container.GetItemQueryIterator<dynamic>(query);
            while (iterator.HasMoreResults)
            {
                foreach (dynamic item in await iterator.ReadNextAsync())
                {
                    yield return item;
                }
            }
        }

        public async Task<dynamic> GetItemByIdAsync(string id)
        {
            FeedIterator<dynamic> iterator = this._container.GetItemQueryIterator<dynamic>($"SELECT * FROM c WHERE c.id = '{id}'");
            dynamic result = null;
            while (iterator.HasMoreResults && result is null)
            {
                var response = await iterator.ReadNextAsync();
                result = response.FirstOrDefault();
            }
            return result;
        }

        public async IAsyncEnumerable<ItemResponse<dynamic>> UpsertCollectionAsync(IEnumerable<dynamic> collection)
        {
            foreach (var item in collection)
            {
                var response = await this._container.UpsertItemAsync<dynamic>(item);
                yield return response;
            }
        }

        public async Task<ItemResponse<dynamic>> UpsertAsync(dynamic item)
        {
            return await this._container.UpsertItemAsync<dynamic>(item);
        }

        public async Task<HttpStatusCode> PatchAsync(string id, PartitionKey partitionKey, IEnumerable<PatchEntity> patchEntities)
        {
            var response = await this._container.PatchItemAsync<dynamic>(id, partitionKey, patchEntities.Select(x => PatchOperationHelper.BuildFrom(x)).ToArray());
            if (response.StatusCode == HttpStatusCode.OK)
            {
                string resourceString = response.Resource.ToString();
                this._logger?.LogInformation("Patch was successfull : {response}", resourceString);
            }
            else
            {
                this._logger?.LogInformation("Patch failed : {response}", response.StatusCode);
            }

            return response.StatusCode;
        }

        public async Task DeleteAsync(string filter, string partitionKeyName)
        {
            QueryDefinition query = new QueryDefinition($"SELECT c.id, c.{partitionKeyName} FROM c {filter}");
            QueryRequestOptions opt = new QueryRequestOptions() { MaxItemCount = 500, MaxBufferedItemCount = 500 };

            var iterator = this._container.GetItemQueryIterator<dynamic>(query, requestOptions: opt);
            int itemCount = 0;
            List<CosmosDbItem> entities = new List<CosmosDbItem>();
            while (iterator.HasMoreResults)
            {
                // Get items
                dynamic[] pageItems = (await iterator.ReadNextAsync())
                    .Select(x => x).ToArray();

                foreach (var item in pageItems)
                {
                    var jo = item as JObject;
                    var id = jo.Value<string>("id");
                    var partKValue = jo.Value<string>(partitionKeyName);
                    var partK = partKValue is null
                        ? PartitionKey.Null
                        : new PartitionKey(partKValue);

                    // Add to delete list
                    entities.Add(new CosmosDbItem(id, partK));
                }
            }


            // Delete items
            var dicDelete = entities.GroupBy(x => x.PartitionKey).ToDictionary(x => x.Key, x => x.ToList());
            foreach (var deleteItem in dicDelete)
            {
                TransactionalBatch batch = this._container.CreateTransactionalBatch(deleteItem.Key);
                foreach (var item in deleteItem.Value)
                {
                    var deletionResult = await this._container.DeleteItemAsync<CosmosDbItem>(item.Id, item.PartitionKey);
                    this._logger?.LogInformation("Deleting item {id}", item.Id);

                    itemCount++;
                }

                var result = await batch.ExecuteAsync();
                foreach(var failure in result.Where(x => !x.IsSuccessStatusCode))
                {
                    this._logger?.LogError("Failed to delete item {etag}", failure.ETag);
                }
            }
        }

        public async Task<string> GetPartitionKeyPathAsync() => (await this._container.ReadContainerAsync()).Resource.PartitionKeyPath;
    }
}
