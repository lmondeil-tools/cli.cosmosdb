namespace lmondeil.cli.cosmosdb.Commands.Settings;

using McMaster.Extensions.CommandLineUtils;

[Command("settings")]
[Subcommand(typeof(SettingsShow))]
internal class SettingsMaster
{
    private void OnExecute(CommandLineApplication app, IConsole console)
    {
        app.ShowHelp();
    }
}
