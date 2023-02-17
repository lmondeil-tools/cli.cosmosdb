namespace lmondeil.cli.cosmosdb.Commands;

using lmondeil.cli.cosmosdb.Extensions;

using McMaster.Extensions.CommandLineUtils;

[Command]
internal class ShowFullHelp
{
    [Option(CommandOptionType.NoValue)]
    public bool Markdown { get; set; }

    private async Task OnExecuteAsync(CommandLineApplication app, IConsole console)
    {
        if (Markdown)
        {
            app.ShowFullMarkdownHelp(console);
        }
        else
        {
            app.ShowFullHelp(console);
        }
    }
}
