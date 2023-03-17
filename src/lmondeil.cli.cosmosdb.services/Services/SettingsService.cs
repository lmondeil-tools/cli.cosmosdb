namespace lmondeil.cli.cosmosdb.services.Services;

using lmondeil.cli.cosmosdb.Models.Settings;

using System.Text.Json;

public class SettingsService
{
    public static async Task SetDatabaseAsync(CosmosDbSettings? cosmosDbSettings, string database)
    {
        var result = (cosmosDbSettings ?? new CosmosDbSettings());
        result.Database = database;
        await SaveSettingsAsync(result);
    }
    public static async Task SetDatabaseAsync(string database, string? environment = null)
    {
        var allSettings = await LoadSettingsAsync(environment);
        var cosmosDbSettings = allSettings.CosmosDbSettings;
        cosmosDbSettings.Database = database;
        await SaveSettingsAsync(cosmosDbSettings, environment);
    }

    public static async Task SetConnectionStringAsync(CosmosDbSettings? cosmosDbSettings, string connectionString)
    {
        var result = (cosmosDbSettings ?? new CosmosDbSettings());
        result.ConnectionString = connectionString;
        await SaveSettingsAsync(result);
    }
    public static async Task SetConnectionStringAsync(string connectionString, string environment = null)
    {
        var allSettings = await LoadSettingsAsync(environment);
        var cosmosDbSettings = allSettings.CosmosDbSettings;
        cosmosDbSettings.ConnectionString = connectionString;
        await SaveSettingsAsync(cosmosDbSettings, environment);
    }

    public static async Task SwitchSettings(string environment)
    {
        string sourceFilePath = Path.Combine(Environment.CurrentDirectory, $"appSettings.{environment}.json");
        string targetFilePath = Path.Combine(Environment.CurrentDirectory, "appSettings.json");
        if (!File.Exists(sourceFilePath))
            throw new FileNotFoundException();
        File.Copy(sourceFilePath, targetFilePath);
    }

    private static async Task SaveSettingsAsync(CosmosDbSettings cosmosDbSettings, string? environment = null)
    {
        var serializerOptions = new JsonSerializerOptions { WriteIndented = true };
        var allSettings = new AllSettings(cosmosDbSettings);
        await File.WriteAllTextAsync(
            Path.Combine(Environment.CurrentDirectory, string.IsNullOrWhiteSpace(environment) ? "appSettings.json" : $"appSettings.{environment}.json"),
            JsonSerializer.Serialize(allSettings, options: serializerOptions));
    }

    private static async Task<AllSettings> LoadSettingsAsync(string environment)
    {
        string filePath = Path.Combine(Environment.CurrentDirectory, string.IsNullOrWhiteSpace(environment) ? "appSettings.json" : $"appSettings.{environment}.json");

        if(!File.Exists(filePath))
            return new AllSettings(new CosmosDbSettings());

        string fileContent = await File.ReadAllTextAsync(filePath);
        var result = JsonSerializer.Deserialize<AllSettings>(fileContent);

        return result;
    }
}
