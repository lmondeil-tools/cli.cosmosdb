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

    public static async Task SetConnectionStringAsync(CosmosDbSettings? cosmosDbSettings, string connectionString)
    {
        var result = (cosmosDbSettings ?? new CosmosDbSettings());
        result.ConnectionString = connectionString;
        await SaveSettingsAsync(result);
    }

    private static async Task SaveSettingsAsync(CosmosDbSettings cosmosDbSettings)
    {
        var serializerOptions = new JsonSerializerOptions { WriteIndented = true };
        var allSettings = new AllSettings(cosmosDbSettings);
        await File.WriteAllTextAsync(
            Path.Combine(AppContext.BaseDirectory,"appSettings.json"), 
            JsonSerializer.Serialize(allSettings, options: serializerOptions));
    }
}
