namespace lmondeil.cli.cosmosdb.Commands.CosmosDb;

using lmondeil.cli.cosmosdb.Extensions;
using lmondeil.cli.cosmosdb.Models.Settings;
using lmondeil.cli.cosmosdb.services.Repositories;

using McMaster.Extensions.CommandLineUtils;

using Microsoft.Extensions.Options;

[Command("select", ExtendedHelpText = "Example: select persons \"SELECT c.firstName, c.lastName FROM c WHERE c.lastName = 'DUPONT'\"")]
internal class CosmosDbSelect
{
    private readonly CosmosDbSettings _cosmosDbSettings;

    [Argument(0)]
    public string ContainerName { get; set; }

    [Argument(1)]
    public string Query { get; set; }

    [Option(Description = "Select some fields based on a JsonPath string")]
    public string JsonPath { get; set; }

    public CosmosDbSelect(IOptions<CosmosDbSettings> cosmosDbSettings)
    {
        _cosmosDbSettings = cosmosDbSettings.Value;
    }

    private async Task OnExecuteAsync(IConsole console)
    {
        await new CosmosDbRepository(_cosmosDbSettings.ConnectionString, _cosmosDbSettings.Database, ContainerName)
            .SelectAsync(Query)
            .DisplayToAsync(console);
    }
}
