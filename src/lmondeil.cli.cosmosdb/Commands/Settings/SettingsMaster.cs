namespace lmondeil.cli.cosmosdb.Commands.Settings;

using McMaster.Extensions.CommandLineUtils;

[Command("settings")]
[Subcommand(
    typeof(SettingsShow),
    typeof(SettingsSetMaster)
)]
internal class SettingsMaster
{
    private void OnExecute(CommandLineApplication app, IConsole console)
    {
        app.ShowHelp();
    }
}
