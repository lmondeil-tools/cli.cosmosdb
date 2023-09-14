namespace lmondeil.cli.cosmosdb.services.Helpers;

using lmondeil.cli.cosmosdb.services.Models;

using Microsoft.Azure.Cosmos;

using Newtonsoft.Json;

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
                    GetValueFromStrings(patchEntity.Value, patchEntity.ValueTypeCode)
                )
        };

    private static object? GetValueFromStrings(string value, string valueTypeCode)
    {
        Type? returnType = Type.GetType(valueTypeCode);
        if( returnType == null )
        {
            throw new ArgumentException($"Unable to get type from string {valueTypeCode}");
        }

        if (returnType.IsArray)
            return JsonConvert.DeserializeObject(value, returnType);
        else
            return Convert.ChangeType(value, returnType);
    }
}
