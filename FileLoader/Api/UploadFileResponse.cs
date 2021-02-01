using Newtonsoft.Json;

namespace FileLoader.Api
{
    /// <summary>
    /// Object to deserialize file upload response
    /// </summary>
    internal class UploadFileResponse
    {
        /// <summary>
        /// File download link
        /// </summary>
        [JsonProperty("href")]
        public string Href { get; set; }
    }
}
