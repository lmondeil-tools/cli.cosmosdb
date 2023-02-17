namespace lmondeil.cli.template.Commands.List;

using McMaster.Extensions.CommandLineUtils;

using System.Diagnostics;

[Command("processes", Description = "List processes on this machine")]
internal class ListProcesses
{
    private async Task OnExecute(IConsole console)
    {
        var entries = Process.GetProcesses().Select(x => x.ProcessName);
        foreach (var entry in entries)
        {
            console.WriteLine(entry);
        }
    }
}
