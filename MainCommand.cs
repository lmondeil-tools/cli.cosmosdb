namespace lmondeil.cli.template;

using lmondeil.cli.template.Commands;
using lmondeil.cli.template.Commands.Hello;
using lmondeil.cli.template.Commands.List;
using lmondeil.cli.template.Commands.Show;
using lmondeil.cli.template.Commands.Test;
using lmondeil.cli.template.Extensions;

using McMaster.Extensions.CommandLineUtils;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

// Here are registered available commands.
// CHANGE "Command" attribute : replace MyCli by your cli name
// REMOVE all except ShowFullHelp - REMOVE corresponding folders in [Commands] folder
// ADD and register yours
[Command("MyCli")]
[Subcommand(
    typeof(ShowFullHelp),
    typeof(HelloMaster),
    typeof(ListMaster),
    typeof(ShowSettings),
    typeof(TestMaster)
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

    private void OnExecute(CommandLineApplication app, IConsole console)
    {
        console.WriteLine("You must specify a subcommand.");
        console.WriteLine("See documentation below.");
        console.WriteLine("****************************************************.");
        console.WriteLine();
        app.ShowFullHelp(console);
    }
}
