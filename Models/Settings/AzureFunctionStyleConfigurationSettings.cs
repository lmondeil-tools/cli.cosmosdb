namespace lmondeil.cli.template.Models.Settings;
public class AzureFunctionStyleConfigurationSettings
{
    public string DisplayName { get; set; }
    public CosmosDbSettings CosmosDb { get; set; }
    public ServiceBusSettings ServiceBus { get; set; }

}
