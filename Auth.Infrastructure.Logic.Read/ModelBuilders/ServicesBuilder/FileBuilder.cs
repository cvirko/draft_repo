using Auth.Domain.Core.Logic.Models.File;
using Auth.Domain.Interface.Logic.External.Files;
using Auth.Domain.Interface.Logic.Read.ModelBuilder.ServiceBuilder;
using Newtonsoft.Json;

namespace Auth.Infrastructure.Logic.Read.ModelBuilders.ServicesBuilder
{
    internal class FileBuilder(IFileService file) : IFileBuilder
    {
        private readonly IFileService _file = file;
        public T GetFromJson<T>(string path)
        {
            if (!File.Exists(path)) return default;
            var str = File.ReadAllText(path);
            if (string.IsNullOrEmpty(str)) return default;
            return JsonConvert.DeserializeObject<T>(str);
        }
        public string GetFromJson(string path)
        {
            if (!File.Exists(path)) return default;
            return File.ReadAllText(path);
        }
        public void ReWriteFile(string directory, string fileName, byte[] buffer)
        {
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);
            string filePath = Path.Combine(directory, fileName);

            using (var fs = File.Open(filePath, FileMode.OpenOrCreate))
                fs.Write(buffer, 0, buffer.Length);
        }
        public void DeleteFile(string directory, string fileName)
        {
            if (!Directory.Exists(directory))
                return;
            string filePath = Path.Combine(directory, fileName);
            if (!File.Exists(filePath))
                return;
            File.Delete(filePath);
        }
        public byte[] ReadFile(string directory, string fileName)
        {
            string filePath = Path.Combine(directory, fileName);
            if (!File.Exists(filePath))
                return default;
            return File.ReadAllBytes(filePath);
        }
        public async Task<byte[]> ReadFileAsync(string directory, string fileName)
        {
            string filePath = Path.Combine(directory, fileName);
            if (!File.Exists(filePath))
                return default;
            return await File.ReadAllBytesAsync(filePath);
        }
        public async Task<FileStreamModel> ReadFileByUriAsync(string uri)
        {
            return await _file.ReadFileByUriAsync(uri);
        }
    }
}
