using System;
using System.Collections.Generic;
using FileLoader.Api;
using FileLoader.Configurations;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace FileLoader
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            host.Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            var switchMappings = new Dictionary<string, string>()
            {
                { "--pathFrom", "PathFrom" },
                { "--pathTo", "PathTo" },
                { "-pathFrom", "PathFrom" },
                { "-pathTo", "PathTo" },
            };
            var host = new HostBuilder()
                .ConfigureAppConfiguration((hostContext, config) =>
                {
                    config.AddCommandLine(args, switchMappings);
                    config.AddJsonFile("appsettings.json");
                });
            host.ConfigureServices((hostContext, services) =>
            {
                var configuration = hostContext.Configuration;
                var yandexDiskApiConfig = configuration.GetSection("YandexDiskApi").Get<YandexDiskApiConfiguration>();
                services
                    .AddLogging(c =>
                    {
                        c.AddDebug();
                        c.AddConsole();
                        c.SetMinimumLevel(LogLevel.Information);
                    })
                    .AddHostedService<FileLoaderWorker>()
                    .AddHttpClient<IYandexDiskApiConnector, YandexDiskApiConnector>(cfg =>
                    {
                        cfg.BaseAddress = new Uri(yandexDiskApiConfig.BaseAddress);
                        cfg.DefaultRequestHeaders.Add("Authorization", yandexDiskApiConfig.Token);
                    });
            });

            return host;
        }
    }
}