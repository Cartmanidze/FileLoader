using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace FileLoader.Api
{
    internal sealed class YandexDiskApiConnector : IYandexDiskApiConnector
    {
        private readonly HttpClient _client;

        private readonly ILogger<YandexDiskApiConnector> _logger;

        public YandexDiskApiConnector(HttpClient client, ILogger<YandexDiskApiConnector> logger)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _logger = logger;
        }

        public async Task UploadFileAsync(FileInfo file, string path, CancellationToken token)
        {
            try
            {
                var response = await _client.GetAsync($"{_client.BaseAddress}/disk/resources/upload/?path={path}{file.Name}", token).ConfigureAwait(false);
                response.EnsureSuccessStatusCode();
                var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var uploadFileResponse = JsonConvert.DeserializeObject<UploadFileResponse>(responseString);
                var httpContent = GetDataContent(file);
                await _client.PutAsync(uploadFileResponse.Href, httpContent, token).ConfigureAwait(false);
                _logger.LogInformation("The file was uploaded");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "The file was not uploaded");
                throw;
            }
        }

        public Task UploadFileAsync(FileInfo file, string path)
        {
            return UploadFileAsync(file, path, CancellationToken.None);
        }

        private MultipartFormDataContent GetDataContent(FileInfo file)
        {
            var content =
                new MultipartFormDataContent();
            FileStream fs = File.OpenRead(file.FullName);
            content.Add(new StreamContent(fs), "file", Path.GetFileName(file.FullName));
            return content;
        }
    }

}
