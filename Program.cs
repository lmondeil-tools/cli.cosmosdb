using lmondeil.cli.template;
using lmondeil.cli.template.Extensions;
using lmondeil.cli.template.Models.Settings;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

try
{
    await Host.CreateDefaultBuilder(args)
        .ConfigureServices((ctx, services) =>
        {
            // Loads settings from app.settings.json
            // 2 ways of nested structure definition are used here
            //  * nested in json format
            //  * nested using colons
            services
            .AddOptionMatchingSection<NestedConfigurationSettings>()
            .AddOptionMatchingSection<AzureFunctionStyleConfigurationSettings>();

            // Add logging feature
            //  logs to console and files
            //services.AddConsoleAndFileLogging("logs", "traces.log", "errors.log");
            services.AddLogMultiConfig(conf =>
            {
                conf
                    .ConfigureFiles("logs", "traces.log", "errors.log");
            });

            // Http
            services.AddHttpClient("test-invoke", client => { client.BaseAddress = new Uri("https://data.education.gouv.fr"); });
        })
        .RunCommandLineApplicationAsync<MainCommand>(args);
}
catch(Exception mainExc)
{
    Console.WriteLine(mainExc.Message);
}