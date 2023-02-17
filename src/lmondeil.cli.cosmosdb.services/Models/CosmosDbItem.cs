namespace lmondeil.cli.cosmosdb.services.Models;


using Microsoft.Azure.Cosmos;

internal record CosmosDbItem(string Id,PartitionKey PartitionKey);
