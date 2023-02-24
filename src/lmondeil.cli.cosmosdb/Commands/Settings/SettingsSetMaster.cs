namespace lmondeil.cli.cosmosdb.Commands.Settings
{
    using McMaster.Extensions.CommandLineUtils;

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    [Command("set")]
    [Subcommand(typeof(SettingsSetConnectionString), typeof(SettingsSetDatabase))]
    internal class SettingsSetMaster
    {
        private void OnExecute(CommandLineApplication app, IConsole console)
        {
            app.ShowHelp();
        }
    }
}
