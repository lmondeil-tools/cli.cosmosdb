namespace lmondeil.cli.cosmosdb.Commands.CosmosDb;

using datahub.batch.CosmosDBUpdate.Models;

using lmondeil.cli.cosmosdb.Models.Settings;
using lmondeil.cli.cosmosdb.services.Models;
using lmondeil.cli.cosmosdb.services.Repositories;

using McMaster.Extensions.CommandLineUtils;

using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

[Command("patch-file", ExtendedHelpText = "Example: \n" +
    "patch persons file.json")]
internal class CosmosDbPatchFile
{
    private readonly CosmosDbSettings _cosmosDbSettings;
    private readonly ILogger<CosmosDbPatch> _logger;

    [Argument(0)]
    public string ContainerName { get; set; } = "";

    [Argument(1)]
    public string Filename { get; set; } = "";

    [Option("-p|--max-degree-of-parallelism", Description = "Default is 10")]
    public int MawDegreeOfParallelism { get; set; } = 10;

    [Option(CommandOptionType.NoValue)]
    public bool Silently { get; set; }


    public CosmosDbPatchFile(IOptions<CosmosDbSettings> cosmosDbSettings, ILogger<CosmosDbPatch> logger)
    {
        _cosmosDbSettings = cosmosDbSettings.Value;
        _logger = logger;
    }

    private async Task OnExecuteAsync()
    {
        var repo = new CosmosDbRepository(_cosmosDbSettings.ConnectionString, _cosmosDbSettings.Database, ContainerName, this.Silently ? null : this._logger);

        var json = await File.ReadAllTextAsync(this.Filename);
        var data = JsonConvert.DeserializeObject<IEnumerable<IntegrationContainerElement>>(json);

        await Parallel.ForEachAsync(data, new ParallelOptions { MaxDegreeOfParallelism = this.MawDegreeOfParallelism }, async (item, cancellationToken) => 
        {
            try
            {
                await repo.PatchAsync(item.Id, new PartitionKey(item.PartitionKey), new[] { new PatchEntity((PatchType)Enum.Parse(typeof(PatchType), item.OperationType), item.OperationPath, item.Value, item.ValueType) });
            }
            catch(Exception ex)
            {
                this._logger.LogError("Error while patching item #{id} - PartitionKey #{pk}", item.Id, item.PartitionKey);
            }
        });
    }
}
