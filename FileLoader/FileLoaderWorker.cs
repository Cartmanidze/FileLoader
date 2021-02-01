using FileLoader.Api;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace FileLoader
{
    /// <summary>
    /// Worker for file unload
    /// </summary>
    internal class FileLoaderWorker : BackgroundService
    {
        private readonly IYandexDiskApiConnector _yandexDiskApi;

        private readonly ILogger<FileLoaderWorker> _logger;

        private string _pathFrom;

        private string _pathTo;

        public FileLoaderWorker(IYandexDiskApiConnector yandexDiskApi, ILogger<FileLoaderWorker> logger, IConfiguration configuration)
        {
            _yandexDiskApi = yandexDiskApi;
            _logger = logger;
            Initialize(configuration);
        }

        private void Initialize(IConfiguration configuration)
        {
            if (string.IsNullOrEmpty(configuration["PathFrom"])) throw new ArgumentException("Argument PathFrom was not set");
            if (string.IsNullOrEmpty(configuration["PathTo"])) throw new ArgumentException("Argument PathTo was not set");
            _pathFrom = configuration["PathFrom"];
            _pathTo = configuration["PathTo"];
            if (!Directory.Exists(_pathFrom)) throw new Exception("PathFrom is not exist");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                var files = Directory.GetFiles(_pathFrom).Select(f => new FileInfo(f));
                _logger.LogInformation("Start uploading files");
                var uploadingTasks = files.Select(file => Task.Run(async () => await UploadFile(file, stoppingToken).ConfigureAwait(false), stoppingToken)).ToArray();
                var tasks = Task.WhenAll(uploadingTasks);
                await tasks.ConfigureAwait(false);
                _logger.LogInformation("Files upload completed");
            }

            catch (Exception e)
            {
                _logger.LogError(e.Message);
                throw;
            }
        }

        private async Task UploadFile(FileInfo file, CancellationToken stoppingToken)
        {
            _logger.LogInformation($"File - {file.Name} uploading...");
            await _yandexDiskApi.UploadFileAsync(file, _pathTo, stoppingToken).ConfigureAwait(false);
            _logger.LogInformation($"File - {file.Name} uploaded");
        }
    }
}
