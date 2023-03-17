namespace lmondeil.cli.cosmosdb.Commands.Settings;

using lmondeil.cli.cosmosdb.Models.Settings;

using McMaster.Extensions.CommandLineUtils;

using Microsoft.Extensions.Options;

using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;

[Command("show")]
internal class SettingsShow
{
    private readonly CosmosDbSettings _cosmosdbSettings;

    public SettingsShow(IOptions<CosmosDbSettings> cosmosdbSettings)
    {
        _cosmosdbSettings = cosmosdbSettings.Value;
    }

    private void OnExecute(CommandLineApplication app, IConsole console)
    {
        var serializerOptions = new JsonSerializerOptions { WriteIndented = true };
        console.WriteLine(JsonSerializer.Serialize(_cosmosdbSettings, options: serializerOptions));

        console.WriteLine("Other environments : ");
        var appSettingsFiles = Directory.GetFiles(Environment.CurrentDirectory, "appSettings*.json");

        foreach (var file in appSettingsFiles.ToList().Except(new[] { "appSettings.json" }))
        {
            var env = Regex.Match(file, @"appSettings.(?<env>\w+).json").Groups["env"].Value;
            console.WriteLine($"\t * {env}");
        }
    }
}
