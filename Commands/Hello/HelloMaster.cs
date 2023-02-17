namespace lmondeil.cli.template.Commands.Hello;

using McMaster.Extensions.CommandLineUtils;

[Command("hello", Description = "Greets in french or english")]
internal class HelloMaster
{
    [Argument(0, Description = "InFrench / InEnglish")]
    public string Lang { get; set; }

    [Option]
    public string Name { get; } = "Anonymous";

    private void OnExecute(IConsole console)
    {
        string message = Lang switch
        {
            nameof(HelloLang.InEnglish) => $"Hello {Name}",
            _ => $"Bonjour {Name}"
        };
        console.WriteLine(message);
    }
}
