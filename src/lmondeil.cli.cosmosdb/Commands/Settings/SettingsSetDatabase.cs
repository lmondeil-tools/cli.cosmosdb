namespace lmondeil.cli.cosmosdb.Commands.Settings
{
    using lmondeil.cli.cosmosdb.Models.Settings;
    using lmondeil.cli.cosmosdb.services.Services;

    using McMaster.Extensions.CommandLineUtils;

    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    using System.Text.Json;

    [Command("database")]
    internal class SettingsSetDatabase
    {
        private readonly CosmosDbSettings _cosmosdbSettings;
        private readonly ILogger _logger;

        [Argument(0)]
        public string Database { get; set; }

        [Argument(1)]
        public string? Environment { get; set; }

        public SettingsSetDatabase(IOptions<CosmosDbSettings> cosmosdbSettings, ILogger<SettingsSetDatabase> logger)
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
                    await SettingsService.SetDatabaseAsync(_cosmosdbSettings, this.Database);
                }
                else
                {
                    await SettingsService.SetDatabaseAsync(this.Database, this.Environment);
                }
                _logger.LogInformation("Successfully set Database value : {database}", this.Database);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Failed to set Database value : {database}\n{error}", this.Database, ex.Message);
            }
        }
    }
}
