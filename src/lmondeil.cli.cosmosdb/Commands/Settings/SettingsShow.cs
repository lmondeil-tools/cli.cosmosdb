namespace lmondeil.cli.cosmosdb.Commands.Settings;

using lmondeil.cli.cosmosdb.Models.Settings;

using McMaster.Extensions.CommandLineUtils;

using Microsoft.Extensions.Options;

using System.Text.Json;

[Command("show")]
internal class SettingsShow
{
    private readonly CosmosDbSettings _cosmosdbSettings;

    public SettingsShow(IOptions<CosmosDbSettings> cosmosdbSettings)
    {
        _cosmosdbSettings = cosmosdbSettings.Value;
    }
    private void OnExecute(CommandLineApplication app, IConsole console)
    {
        var serializerOptions = new JsonSerializerOptions { WriteIndented = true };
        console.WriteLine(JsonSerializer.Serialize(_cosmosdbSettings, options: serializerOptions));
    }
}
