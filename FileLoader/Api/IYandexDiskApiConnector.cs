using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace FileLoader.Api
{
    /// <summary>
    /// Connector for Yandex.Disk.API
    /// </summary>
    internal interface IYandexDiskApiConnector
    {
        /// <summary>
        /// Upload file (async method)
        /// </summary>
        /// <param name="file">File to send</param>
        /// <param name="path">The path where the resource will be placed</param>
        Task UploadFileAsync(FileInfo file, string path);

        /// <summary>
        /// Upload file (async method)
        /// </summary>
        /// <param name="file">File to send</param>
        /// <param name="path">The path where the resource will be placed</param>
        /// <param name="token">Cancellation token</param>
        Task UploadFileAsync(FileInfo file, string path, CancellationToken token);
    }
}
