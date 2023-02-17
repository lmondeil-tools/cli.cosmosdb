namespace lmondeil.cli.cosmosdb.Models.Logging
{
    using Serilog;
    using Serilog.Core;

    internal class LogMultiConfig
    {
        private readonly LoggerConfiguration _loggerConfiguration;

        public LogMultiConfig()
        {
            _loggerConfiguration = new LoggerConfiguration();
            _loggerConfiguration.MinimumLevel.Override(typeof(HttpClient).FullName, Serilog.Events.LogEventLevel.Warning);

        }

        public LogMultiConfig ConfigureConsole()
        {
            _loggerConfiguration.WriteTo.ColoredConsole();

            return this;
        }

        public LogMultiConfig ConfigureFiles(string logFolder, string tracesFileName, string errorsFileName)
        {
            _loggerConfiguration.WriteTo.File(Path.Combine(logFolder, tracesFileName), rollOnFileSizeLimit: true, fileSizeLimitBytes: 10485760) // 10 MB
            .WriteTo.File(Path.Combine(logFolder, errorsFileName), restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Warning, rollOnFileSizeLimit: true, fileSizeLimitBytes: 10485760);

            return this;
        }

        public Logger Build() => _loggerConfiguration.CreateLogger();
    }
}
