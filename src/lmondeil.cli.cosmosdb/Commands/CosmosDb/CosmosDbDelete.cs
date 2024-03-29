﻿namespace lmondeil.cli.cosmosdb.Commands.CosmosDb;

using lmondeil.cli.cosmosdb.Models.Settings;
using lmondeil.cli.cosmosdb.services.Repositories;

using McMaster.Extensions.CommandLineUtils;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

[Command("delete", ExtendedHelpText = "Example: delete persons \"WHERE c.lastName = 'DUPONT'\"")]
internal class CosmosDbDelete
{
    private readonly CosmosDbSettings _cosmosDbSettings;
    private readonly ILogger<CosmosDbDelete> _logger;

    [Argument(0)]
    public string ContainerName { get; set; }

    [Argument(1, Description = "usage example : \"WHERE c.property == 'value'\"")]
    public string Where { get; set; }

    [Option(Description = "Determines how many deletions are processed in parallel - default = 1 (meaning non parallelism)")]
    public int MaxDegreeOfParalellism { get; set; } = 1;

    public CosmosDbDelete(IOptions<CosmosDbSettings> cosmosDbSettings, ILogger<CosmosDbDelete> logger)
    {
        _cosmosDbSettings = cosmosDbSettings.Value;
        _logger = logger;
    }

    private async Task OnExecuteAsync(IConsole console)
    {
        var repo = new CosmosDbRepository(_cosmosDbSettings.ConnectionString, _cosmosDbSettings.Database, ContainerName, _logger);
        var pkName = (await repo.GetPartitionKeyPathAsync()).TrimStart('/');
        await repo.DeleteAsync(Where, pkName, MaxDegreeOfParalellism);
    }
}
