namespace lmondeil.cli.cosmosdb.Commands.Settings;

using lmondeil.cli.cosmosdb.Models.Settings;
using lmondeil.cli.cosmosdb.services.Services;

using McMaster.Extensions.CommandLineUtils;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

[Command("connection-string")]
internal class SettingsSetConnectionString
{
    private readonly CosmosDbSettings _cosmosdbSettings;
    private readonly ILogger _logger;

    [Argument(0)]
    public string ConnectionString { get; set; }

    [Argument(1)]
    public string? Environment { get; set; }

    public SettingsSetConnectionString(IOptions<CosmosDbSettings> cosmosdbSettings, ILogger<SettingsSetConnectionString> logger)
    {
        _cosmosdbSettings = cosmosdbSettings.Value;
        _logger = logger;
    }
    private async Task OnExecuteAsync(CommandLineApplication app)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(Environment))
            {
                await SettingsService.SetConnectionStringAsync(_cosmosdbSettings, this.ConnectionString);
            }
            else
            {
                await SettingsService.SetConnectionStringAsync(this.ConnectionString, this.Environment);
            }
            _logger.LogInformation("Successfully set Database value : {database}", this.ConnectionString);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to set Database value : {database}\n{error}", this.ConnectionString, ex.Message);
        }
    }
}
