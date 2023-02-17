namespace lmondeil.cli.cosmosdb.services.Helpers;

using lmondeil.cli.cosmosdb.services.Models;

using Microsoft.Azure.Cosmos;

using System;

public class PatchOperationHelper
{
    public static PatchOperation BuildFrom(PatchEntity patchEntity)
    => patchEntity.PatchType switch
        {
            PatchType.Delete => PatchOperation.Remove("/" + patchEntity.PropertyPath.TrimStart('/')),
            PatchType.Increment => PatchOperation.Increment
                (
                    "/" + patchEntity.PropertyPath.TrimStart('/'),
                    long.Parse(patchEntity.Value)
                ),
            _ => PatchOperation.Set
                (
                    "/" + patchEntity.PropertyPath.TrimStart('/'),
                    Convert.ChangeType(patchEntity.Value, Enum.Parse<TypeCode>(patchEntity.ValueTypeCode))
                )
        };
}
