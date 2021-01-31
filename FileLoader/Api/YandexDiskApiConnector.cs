using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FileLoader.Api
{
    public sealed class YandexDiskApiConnector : IYandexDiskApiConnector
    {
        private readonly HttpClient _client;

        private readonly ILogger<YandexDiskApiConnector> _logger;

        public YandexDiskApiConnector(HttpClient client, ILogger<YandexDiskApiConnector> logger)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _logger = logger;
        }

        public async Task UploadFile(string path, string url)
        {
            try
            {
                var parameter = new { path, url };
                var jsonParameter = JsonConvert.SerializeObject(parameter);
                var httpContent = new StringContent(jsonParameter, Encoding.UTF8, "application/json");
                var response = await _client.PostAsync(_client.BaseAddress, httpContent).ConfigureAwait(false);
                response.EnsureSuccessStatusCode();
                _logger.LogInformation("The file was uploaded");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "The file was not uploaded");
                throw;
            }
        }
    }
}
