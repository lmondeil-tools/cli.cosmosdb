namespace lmondeil.cli.template.Commands.List;

using McMaster.Extensions.CommandLineUtils;

[Command("list")]
[Subcommand(
    typeof(ListCsFiles),
    typeof(ListProcesses)
)]
internal class ListMaster
{
    private void OnExecute(CommandLineApplication app, IConsole console)
    {
        console.WriteLine("You must specify a subcommand.");
        app.ShowHelp();
    }
}
