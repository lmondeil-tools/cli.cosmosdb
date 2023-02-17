namespace lmondeil.cli.template.Commands.Test
{
    using McMaster.Extensions.CommandLineUtils;

    [Command("test")]
    [Subcommand(
        typeof(TestAppInsight),
        typeof(TestInvoke)
    )]
    internal class TestMaster
    {
        public void OnExecute(CommandLineApplication app)
        {
            app.ShowHelp();
        }
    }
}
