namespace lmondeil.cli.cosmosdb.Commands.CosmosDb;

using lmondeil.cli.cosmosdb.Models.Settings;
using lmondeil.cli.cosmosdb.services.Models;
using lmondeil.cli.cosmosdb.services.Repositories;

using McMaster.Extensions.CommandLineUtils;

using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using System.Net;

[Command("patch-many", ExtendedHelpText = "Example: \n" +
    "patch persons set firstName \"Pierre\" System.String \"WHERE c.firstName = ''\"\n" +
    "patch persons set age 18 System.Int32 \"WHERE c.firstName = 'Pierre' AND c.lastName = 'DUPONT'\"\n" +
    "patch persons increment age 3 \"WHERE c.wasForgotten = true\"\n" +
    "patch persons delete teenager \"WHERE c.age = >= 18\"")]
internal class CosmosDbPatchMany
{
    private readonly CosmosDbSettings _cosmosDbSettings;
    private readonly ILogger<CosmosDbPatchMany> _logger;


    [Argument(0)]
    public string ContainerName { get; set; } = "";

    [Argument(1)]
    public PatchType PatchType { get; set; }

    [Argument(2)]
    public string PropertyPath { get; set; } = "";

    [Argument(3)]
    public string Value { get; set; } = "";

    [Argument(4, Description = "Exemples: String, Int32, ...")]
    public string ValueType { get; set; } = "";

    [Argument(5, Description = "Example: \"WHERE c.property = true\"")]
    public string Where { get; set; }

    public CosmosDbPatchMany(IOptions<CosmosDbSettings> cosmosDbSettings, ILogger<CosmosDbPatchMany> logger)
    {
        _cosmosDbSettings = cosmosDbSettings.Value;
        _logger = logger;
    }

    private async Task OnExecuteAsync()
    {
        var repo = new CosmosDbRepository(_cosmosDbSettings.ConnectionString, _cosmosDbSettings.Database, ContainerName);

        // Get PartitionKey
        var pkPath = await repo.GetPartitionKeyPathAsync();

        // Patch
        var documents = repo.SelectAsync($"SELECT c.id, c.{pkPath.TrimStart('/')} FROM c {this.Where}");
        await foreach (var doc in documents)
        {
            string docId = doc.id;
            var pkValue = doc[pkPath.TrimStart('/')].ToString();
            PartitionKey pk = new PartitionKey(pkValue);
            var statusCode = await repo.PatchAsync(docId, pk, new[] { new PatchEntity(PatchType, PropertyPath, Value, ValueType) });
            if (statusCode == HttpStatusCode.OK)
                this._logger.LogInformation("Successfully patched #{id}", docId);
            else
                this._logger.LogWarning("Failed to patch #{id}", docId);
        }
    }
}
