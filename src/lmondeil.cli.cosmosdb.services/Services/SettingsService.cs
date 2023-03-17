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
        var cosmosDbSettings = await LoadSettingsAsync(environment);
        cosmosDbSettings.Database = database;
        await SaveSettingsAsync(cosmosDbSettings);
    }

    public static async Task SetConnectionStringAsync(CosmosDbSettings? cosmosDbSettings, string connectionString)
    {
        var result = (cosmosDbSettings ?? new CosmosDbSettings());
        result.ConnectionString = connectionString;
        await SaveSettingsAsync(result);
    }
    public static async Task SetConnectionStringAsync(string connectionString, string environment = null)
    {
        var cosmosDbSettings = await LoadSettingsAsync(environment);
        cosmosDbSettings.ConnectionString = connectionString;
        await SaveSettingsAsync(cosmosDbSettings);
    }

    private static async Task SaveSettingsAsync(CosmosDbSettings cosmosDbSettings, string? environment = null)
    {
        var serializerOptions = new JsonSerializerOptions { WriteIndented = true };
        var allSettings = new AllSettings(cosmosDbSettings);
        await File.WriteAllTextAsync(
            Path.Combine(Environment.CurrentDirectory, string.IsNullOrWhiteSpace(environment) ? "appSettings.json" : $"appSettings.{environment}.json"),
            JsonSerializer.Serialize(allSettings, options: serializerOptions));
    }

    private static async Task<CosmosDbSettings> LoadSettingsAsync(string environment)
    {
        var serializerOptions = new JsonSerializerOptions { WriteIndented = true };

        string filePath = Path.Combine(Environment.CurrentDirectory, string.IsNullOrWhiteSpace(environment) ? "appSettings.json" : $"appSettings.{environment}.json");

        if(!File.Exists(filePath))
            return new CosmosDbSettings();

        string fileContent = await File.ReadAllTextAsync(filePath);
        var result = JsonSerializer.Deserialize<CosmosDbSettings>(fileContent, options: serializerOptions);

        return result;
    }
}
