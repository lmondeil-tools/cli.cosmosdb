namespace lmondeil.cli.template.Commands.List;

using McMaster.Extensions.CommandLineUtils;

[Command("cs-files", Description = "List all cs files in the current folder (recusrively)")]
internal class ListCsFiles
{
    private async Task OnExecute(IConsole console)
    {
        string directory = Directory.GetCurrentDirectory();
        console.WriteLine("***** {directory} *****");

        var entries = Directory
            .GetFiles(directory, "*.cs", new EnumerationOptions { RecurseSubdirectories = true })
            .Select(x => x.Replace(directory, ""));
        foreach (var entry in entries)
        {
            console.WriteLine(entry);
        }
    }
}
