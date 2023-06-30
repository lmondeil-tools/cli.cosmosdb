namespace datahub.batch.CosmosDBUpdate.Models
{
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    using Newtonsoft.Json;

    public class IntegrationContainerElement
    {
        [JsonProperty("id", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("id")]
        [DataMember(Name = "id")]
        public string Id { get; set; }

        [JsonProperty("_partitionKey", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("_partitionKey")]
        [DataMember(Name = "_partitionKey")]
        public string PartitionKey { get; set; }

        [JsonProperty("operationType", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("operationType")]
        [DataMember(Name = "operationType")]
        public string OperationType { get; set; } = "Add";

        [JsonProperty("operationPath", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("operationPath")]
        [DataMember(Name = "operationPath")]
        public string OperationPath { get; set; }

        [JsonProperty("value", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("value")]
        [DataMember(Name = "value")]
        public string Value { get; set; }

        [JsonProperty("valueType", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("valueType")]
        [DataMember(Name = "valueType")]
        public string ValueType { get; set; } = "String";

        [JsonProperty("database", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("database")]
        [DataMember(Name = "database")]
        public string Database { get; set; }

        [JsonProperty("container", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("container")]
        [DataMember(Name = "container")]
        public string Container { get; set; }
    }
}
