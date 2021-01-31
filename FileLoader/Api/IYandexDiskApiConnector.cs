using System.Threading.Tasks;

namespace FileLoader.Api
{
    /// <summary>
    /// Connector for Yandex.Disk.API
    /// </summary>
    public interface IYandexDiskApiConnector
    {
        /// <summary>
        /// Upload file
        /// </summary>
        /// <param name="path">The path where the resource will be placed</param>
        /// <param name="url">The URL of the external resource to download</param>
        Task UploadFile(string path, string url);
    }
}
