namespace lmondeil.cli.cosmosdb.services.Models;

public record PatchEntity
    (
        PatchType PatchType,
        string PropertyPath, 
        string Value, 
        string ValueTypeCode
    );