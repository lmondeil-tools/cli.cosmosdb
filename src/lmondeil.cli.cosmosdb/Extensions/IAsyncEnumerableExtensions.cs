namespace lmondeil.cli.cosmosdb.Extensions;

using McMaster.Extensions.CommandLineUtils;

using Newtonsoft.Json;

using System.Collections.Generic;
using System.Threading.Tasks;

internal static class IAsyncEnumerableExtensions
{
    public static async Task DisplayToAsync<T>(this IAsyncEnumerable<T> @this, IConsole console)
    {
        console.WriteLine("[");
        bool isFirstMessage = true;
        await foreach (var item in @this)
        {
            if (isFirstMessage)
                isFirstMessage = false;
            else
                console.WriteLine(",");

            string message = JsonConvert.SerializeObject(item, Formatting.Indented);
            console.WriteLine(message);
        }
        console.WriteLine("]");

    }
}
