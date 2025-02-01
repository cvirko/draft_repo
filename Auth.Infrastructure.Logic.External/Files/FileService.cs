using Auth.Domain.Core.Logic.Models.File;
using Auth.Domain.Interface.Logic.External.Files;
using Microsoft.Extensions.Logging;

namespace Auth.Infrastructure.Logic.External.Files
{
    internal class FileService(IHttpClientFactory httpClientFactory,
        ILogger<FileService> logger) : IFileService
    {
        private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
        ILogger<FileService> _logger = logger;
        public async Task<FileStreamModel> ReadFileByUriAsync(string uri)
        {
            if (string.IsNullOrEmpty(uri)) return default;

            try
            {
                var httpClient = _httpClientFactory.CreateClient();
                var response = await httpClient.GetAsync(uri, HttpCompletionOption.ResponseHeadersRead);

                if (!response.IsSuccessStatusCode)
                {
                    return default;
                }
                MemoryStream stream = new MemoryStream();
                using (var content =  await response.Content.ReadAsStreamAsync())
                {
                    content.CopyTo(stream);
                }
                stream.Position = 0;
                return new FileStreamModel
                {
                    ContentType = response.Content.Headers.ContentType?.ToString() ?? "application/octet-stream",
                    Name = response.Content.Headers.ContentDisposition?.FileName ?? "file",
                    Stream = stream
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return default;
            }
        }
    }
}
