namespace lmondeil.cli.cosmosdb.Commands.Settings;

using lmondeil.cli.cosmosdb.Models.Settings;
using lmondeil.cli.cosmosdb.services.Services;

using McMaster.Extensions.CommandLineUtils;

using Microsoft.Extensions.Options;

using System.Text.Json;

[Command("switchto")]
internal class SettingsSwitchTo
{
    private readonly CosmosDbSettings _cosmosdbSettings;

    public string Environment{ get; set; }

    private void OnExecute(CommandLineApplication app, IConsole console)
    {
        SettingsService.SwitchSettings(this.Environment);
    }
}
