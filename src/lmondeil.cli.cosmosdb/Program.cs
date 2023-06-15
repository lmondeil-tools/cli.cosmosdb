using lmondeil.cli.cosmosdb.Commands;
using lmondeil.cli.cosmosdb.Extensions;
using lmondeil.cli.cosmosdb.Models.Settings;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

try
{
    await Host.CreateDefaultBuilder(args)
        .ConfigureAppConfiguration((context, config) =>
        {
            config.SetBasePath(AppContext.BaseDirectory);
        })
        .ConfigureServices((ctx, services) =>
        {
            
            // Loads settings from app.settings.json
            services
                .AddOptionMatchingSection<CosmosDbSettings>();

            // Add logging feature
            //  logs to console and files
            services.AddLogMultiConfig(conf =>
            {
                conf
                    .ConfigureFiles("logs", "traces.log", "errors.log");
            });
        })
        .RunCommandLineApplicationAsync<MainCommand>(args);
}
catch(Exception mainExc)
{
    Console.WriteLine(mainExc.Message);
}