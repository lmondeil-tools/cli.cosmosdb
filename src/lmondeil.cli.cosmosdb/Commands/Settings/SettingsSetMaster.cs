namespace lmondeil.cli.cosmosdb.Commands.Settings;

using McMaster.Extensions.CommandLineUtils;

[Command("set")]
[Subcommand(typeof(SettingsSetConnectionString), typeof(SettingsSetDatabase))]
internal class SettingsSetMaster
{
    private void OnExecute(CommandLineApplication app, IConsole console)
    {
        app.ShowHelp();
    }
}
