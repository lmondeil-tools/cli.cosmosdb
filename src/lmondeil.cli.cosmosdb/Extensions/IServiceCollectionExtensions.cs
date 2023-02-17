namespace lmondeil.cli.cosmosdb.Extensions;

using lmondeil.cli.cosmosdb.Models.Logging;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Serilog;

internal static class IServiceCollectionExtensions
{
    public static IServiceCollection AddOptionMatchingSection<T>(this IServiceCollection services) where T : class
    {
        services.AddOptions<T>().Configure<IConfiguration>((settings, configuration) =>
        {
            configuration.GetSection(typeof(T).Name).Bind(settings);
        });

        return services;
    }

    public static IServiceCollection AddConsoleAndFileLogging(this IServiceCollection services, string logFolder, string tracesFileName, string errorsFileName)
    {
        var logger = new LoggerConfiguration()
            //.Enrich.WithDemystifiedStackTraces() <-- install nuget package "Serilog.Enrichers.Demystifier"
            .MinimumLevel.Override(typeof(HttpClient).FullName, Serilog.Events.LogEventLevel.Warning)
            //.Enrich.FromLogContext()
            .WriteTo.Console()
            //.WriteTo.Console(outputTemplate: @"[{Timestamp:HH:mm:ss} {Level:u3} {SourceContext}] {Message:lj}{NewLine}{Exception}")
            //.WriteTo.SQLite(Path.ChangeExtension(logFilePath, "sqlite"))
            .WriteTo.File(Path.Combine(logFolder, tracesFileName), rollOnFileSizeLimit: true, fileSizeLimitBytes: 10485760) // 10 MB
            .WriteTo.File(Path.Combine(logFolder, errorsFileName), restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Warning, rollOnFileSizeLimit: true, fileSizeLimitBytes: 10485760)
            .CreateLogger();

        services.AddLogging(loggingBuilder
            => loggingBuilder.AddSerilog(logger: logger, dispose: true));

        return services;
    }
    public static IServiceCollection AddLogMultiConfig(this IServiceCollection services, Action<LogMultiConfig> configureMultiConfig)
    {
        var config = new LogMultiConfig();
        configureMultiConfig(config);
        var logger = config.Build();

        services.AddLogging(loggingBuilder
            => loggingBuilder.AddSerilog(logger: logger, dispose: true));

        return services;
    }

}
