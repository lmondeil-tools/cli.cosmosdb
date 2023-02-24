namespace lmondeil.cli.cosmosdb.Commands.CosmosDb;

using lmondeil.cli.cosmosdb.Models.Settings;
using lmondeil.cli.cosmosdb.services.Models;
using lmondeil.cli.cosmosdb.services.Repositories;

using McMaster.Extensions.CommandLineUtils;

using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

[Command("patch", ExtendedHelpText = "Example: \n" +
    "patch persons d1e23f7d-ce7a-42ad-bc26-da296efb37be set firstName \"Pierre\" string\n" +
    "patch persons d1e23f7d-ce7a-42ad-bc26-da296efb37be set age 18 Int32 \n" +
    "patch persons d1e23f7d-ce7a-42ad-bc26-da296efb37be increment age 3 \n" +
    "patch persons d1e23f7d-ce7a-42ad-bc26-da296efb37be delete teenager")]
internal class CosmosDbPatch
{
    private readonly CosmosDbSettings _cosmosDbSettings;
    private readonly ILogger<CosmosDbPatch> _logger;

    [Argument(0)]
    public string ContainerName { get; set; } = "";

    [Argument(1)]
    public string Id { get; set; } = "";

    [Argument(2)]
    public PatchType PatchType { get; set; }

    [Argument(3)]
    public string PropertyPath { get; set; } = "";

    [Argument(4)]
    public string Value { get; set; } = "";

    [Argument(5, Description = "Exemples: String, Int32, ...")]
    public string ValueType { get; set; } = "";

    [Option(CommandOptionType.NoValue)]
    public bool Silently { get; set; }

    public CosmosDbPatch(IOptions<CosmosDbSettings> cosmosDbSettings, ILogger<CosmosDbPatch> logger)
    {
        _cosmosDbSettings = cosmosDbSettings.Value;
        _logger = logger;
    }

    private async Task OnExecuteAsync()
    {
        var repo = new CosmosDbRepository(_cosmosDbSettings.ConnectionString, _cosmosDbSettings.Database, ContainerName, this.Silently ? null : this._logger);

        // Get PartitionKey
        var pkPath = await repo.GetPartitionKeyPathAsync();
        var doc = await repo.GetItemByIdAsync(this.Id);
        var pkValue = doc[pkPath.TrimStart('/')].ToString();
        PartitionKey pk = new PartitionKey(pkValue);

        // Patch
        await repo.PatchAsync(Id, pk, new[] { new PatchEntity(PatchType, PropertyPath, Value, ValueType) });

    }
}
