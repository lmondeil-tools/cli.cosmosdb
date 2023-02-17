namespace lmondeil.cli.template.Commands;

using lmondeil.cli.template.Extensions;

using McMaster.Extensions.CommandLineUtils;

[Command]
internal class ShowFullHelp
{
    private void OnExecute(CommandLineApplication app, IConsole console)
    {
        app.ShowFullHelp(console);
    }
}
