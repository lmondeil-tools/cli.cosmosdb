namespace lmondeil.cli.template.Commands.Test
{
    using McMaster.Extensions.CommandLineUtils;

    using Microsoft.Extensions.Logging;

    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;

    [Command("invoke")]
    internal class TestInvoke
    {
        private readonly ILogger _logger;
        private readonly HttpClient _client;

        public TestInvoke(ILogger<TestInvoke> logger, IHttpClientFactory clientFactory)
        {
            _logger = logger;
            _client = clientFactory.CreateClient("test-invoke");
        }

        public async Task OnExecute()
        {
            try
            {
                var response = await _client.GetAsync("api/v2/catalog/datasets/fr-en-annuaire-education");
                var message = response.EnsureSuccessStatusCode();
                var content = await message.Content.ReadAsStringAsync();
                _logger.LogInformation(content);
            }
            catch (Exception exc)
            {
                _logger.LogInformation(exc, "Failed to call rest api");
            }
        }
    }

    internal class InvokeModel
    {
        public IEnumerable<InnerModel> Links { get; set; }
    }

    internal class InnerModel
    {
        public string Rel { get; set; }
        public string Href { get; set; }
    }
}
