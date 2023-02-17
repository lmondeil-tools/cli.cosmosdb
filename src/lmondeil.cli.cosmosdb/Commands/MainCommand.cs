namespace lmondeil.cli.cosmosdb.Commands;
using lmondeil.cli.cosmosdb.Commands.CosmosDb;
using lmondeil.cli.cosmosdb.Commands.Environments;
using lmondeil.cli.cosmosdb.Commands.Settings;
using lmondeil.cli.cosmosdb.Extensions;

using McMaster.Extensions.CommandLineUtils;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

// Here are registered available commands.
// CHANGE "Command" attribute : replace MyCli by your cli name
// REMOVE all except ShowFullHelp - REMOVE corresponding folders in [Commands] folder
// ADD and register yours
[Command("lmcosmos")]
[Subcommand(
    typeof(CosmosDbSelect),
    typeof(CosmosDbPatch),
    typeof(CosmosDbPatchMany),
    typeof(CosmosDbDelete),
    typeof(SwitchTo),
    typeof(SettingsMaster)
)]
internal class MainCommand
{
    private IHostEnvironment _env;
    private readonly ILogger _logger;

    public MainCommand(IHostEnvironment env, ILogger<MainCommand> logger)
    {
        _env = env;
        _logger = logger;
    }

    [Option(CommandOptionType.NoValue)]
    public bool FullHelp { get; set; }

    [Option(CommandOptionType.NoValue)]
    public bool Markdown { get; set; }

    private void OnExecute(CommandLineApplication app, IConsole console)
    {
        if (!this.FullHelp)
        {
            console.WriteLine("You must specify a subcommand.");
            console.WriteLine("See documentation below.");
            console.WriteLine("****************************************************.");
            console.WriteLine();
        }

        if (this.Markdown)
        {
            app.ShowFullMarkdownHelp(console);
        }
        else
        {
            app.ShowFullHelp(console);
        }
    }
}
