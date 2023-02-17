namespace lmondeil.cli.template.Commands.Show;

using lmondeil.cli.template.Models.Settings;

using McMaster.Extensions.CommandLineUtils;

using Microsoft.Extensions.Options;

using System.Text.Json;

[Command(Description = "Show settings that have been parsed from appSettings.json and injected")]
internal class ShowSettings
{
    private readonly NestedConfigurationSettings _settings;
    private readonly AzureFunctionStyleConfigurationSettings _azureFuncStyleSettings;

    public ShowSettings(IOptions<NestedConfigurationSettings> nestedSettings, IOptions<AzureFunctionStyleConfigurationSettings> azureFuncStyleSettings)
    {
        _settings = nestedSettings.Value;
        _azureFuncStyleSettings = azureFuncStyleSettings.Value;
    }
    private void OnExecute(CommandLineApplication app, IConsole console)
    {
        var serializerOptions = new JsonSerializerOptions { WriteIndented = true };
        console.WriteLine(JsonSerializer.Serialize(_settings, options: serializerOptions));
        console.WriteLine(JsonSerializer.Serialize(_azureFuncStyleSettings, options: serializerOptions));
    }
}
