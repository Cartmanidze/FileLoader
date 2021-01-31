using FileLoader.Api;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FileLoader
{
    public class FileLoaderWorker : BackgroundService
    {
        private readonly IYandexDiskApiConnector _yandexDiskApi;

        private readonly ILogger<FileLoaderWorker> _logger;

        public FileLoaderWorker(IYandexDiskApiConnector yandexDiskApi, ILogger<FileLoaderWorker> logger)
        {
            _yandexDiskApi = yandexDiskApi;
            _logger = logger;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            throw new NotImplementedException();
        }
    }
}
