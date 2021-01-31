using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;


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
            var host = new HostBuilder();
            host.ConfigureServices((hostContext, services) =>
            {
                var configuration = hostContext.Configuration;
                services
                   .AddHostedService<FileLoaderWorker>()
                   .AddLogging();
            });
            return host;
        }
    }
}
