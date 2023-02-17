namespace lmondeil.cli.template.Models.Settings;
public class NestedConfigurationSettings
{
    public string DisplayName { get; set; }
    public CosmosDbSettings CosmosDb { get; set; }
    public ServiceBusSettings ServiceBus { get; set; }
    public AppInsightSettings appInsight { get; set; }
}
