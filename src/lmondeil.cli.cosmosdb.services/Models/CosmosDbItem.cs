namespace lmondeil.cli.cosmosdb.services.Models;


using Microsoft.Azure.Cosmos;

public record CosmosDbItem(string Id,PartitionKey PartitionKey);
