namespace lmondeil.cli.template.Commands.Test
{
    using McMaster.Extensions.CommandLineUtils;

    using Microsoft.Extensions.Logging;

    [Command("app-insight")]
    internal class TestAppInsight
    {
        private readonly ILogger _logger;

        public TestAppInsight(ILogger<TestAppInsight> _logger)
        {
            this._logger = _logger;
        }

        public void OnExecute(CommandLineApplication app)
        {
            _logger.LogInformation("Logged from Cli.Template at {date}", DateTimeOffset.Now);
        }

    }
}
