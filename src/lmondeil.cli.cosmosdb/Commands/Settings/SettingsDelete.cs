namespace lmondeil.cli.cosmosdb.Commands.Settings;

using lmondeil.cli.cosmosdb.services.Services;

using McMaster.Extensions.CommandLineUtils;

[Command("delete")]
internal class SettingsDelete
{
    [Argument(0)]
    public string Environment { get; set; }

    private async Task OnExecute(CommandLineApplication app, IConsole console)
    {
        await SettingsService.DeleteAsync(this.Environment);
    }
}
