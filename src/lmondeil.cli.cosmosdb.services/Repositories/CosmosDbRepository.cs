namespace lmondeil.cli.cosmosdb.services.Repositories
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

            _container = client.GetContainer(database, containerName);
            _logger = logger;
        }

        public async IAsyncEnumerable<dynamic> SelectAsync(string query)
        {
            FeedIterator<dynamic> iterator = _container.GetItemQueryIterator<dynamic>(query);
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
            FeedIterator<dynamic> iterator = _container.GetItemQueryIterator<dynamic>($"SELECT * FROM c WHERE c.id = '{id}'");
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
                var response = await _container.UpsertItemAsync<dynamic>(item);
                yield return response;
            }
        }

        public async Task<ItemResponse<dynamic>> UpsertAsync(dynamic item)
        {
            return await _container.UpsertItemAsync<dynamic>(item);
        }

        public async Task<HttpStatusCode> PatchAsync(string id, PartitionKey partitionKey, IEnumerable<PatchEntity> patchEntities)
        {
            var response = await _container.PatchItemAsync<dynamic>(id, partitionKey, patchEntities.Select(x => PatchOperationHelper.BuildFrom(x)).ToArray());
            if (response.StatusCode == HttpStatusCode.OK)
            {
                string resourceString = response.Resource.ToString();
                _logger?.LogInformation("Patch was successfull : {response}", resourceString);
            }
            else
            {
                _logger?.LogInformation("Patch failed : {response}", response.StatusCode);
            }

            return response.StatusCode;
        }

        public async Task DeleteAsync(string filter, string partitionKeyName, int maxDegreeOfParallelism)
        {
            QueryDefinition query = new QueryDefinition($"SELECT c.id, c.{partitionKeyName} FROM c {filter}");
            QueryRequestOptions opt = new QueryRequestOptions() { MaxItemCount = 500, MaxBufferedItemCount = 500 };

            var iterator = _container.GetItemQueryIterator<dynamic>(query, requestOptions: opt);
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
            foreach (var deleteItemChunks in dicDelete.Chunk(maxDegreeOfParallelism))
            {
                List<Task> tasks = new ();
                foreach(var chunkItem in deleteItemChunks)
                {
                    Task.WaitAll(chunkItem.Value.Select(document 
                        => Task.Run(async () =>
                        {
                            _logger?.LogInformation("Deleting item #id:{id} - #partitionKey:{partitionKey}", document.Id, document.PartitionKey);
                            try
                            {
                                await _container.DeleteItemAsync<CosmosDbItem>(document.Id, document.PartitionKey);
                                _logger?.LogInformation("Succesfully deleted item #id:{id} - #partitionKey:{partitionKey}", document.Id, document.PartitionKey);
                            }
                            catch (Exception ex)
                            {
                                _logger?.LogError(ex, "Failed to delete item #id:{id} - #partitionKey:{partitionKey}", document.Id, document.PartitionKey);
                            }
                        })
                    ).ToArray());
                }
            }
        }

        public async Task<string> GetPartitionKeyPathAsync() => (await _container.ReadContainerAsync()).Resource.PartitionKeyPath;
    }
}
