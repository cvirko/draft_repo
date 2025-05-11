using Auth.Domain.Core.Logic.Models.File;
using Auth.Domain.Interface.Logic.External.Files;
using Auth.Domain.Interface.Logic.Read.ModelBuilder.ServiceBuilder;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;

namespace Auth.Infrastructure.Logic.Read.ModelBuilders.ServicesBuilder
{
    internal class FileBuilder(IFileService file, IHostingEnvironment env) : IFileBuilder
    {
        private readonly IFileService _file = file;
        private readonly string webRootPath = env.WebRootPath;
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
            var fullDirectoryPath = GetFullDirectoryPath(directory);
            if (!Directory.Exists(fullDirectoryPath))
                Directory.CreateDirectory(fullDirectoryPath);
            string filePath = Path.Combine(fullDirectoryPath, fileName);

            using (var fs = File.Open(filePath, FileMode.OpenOrCreate))
                fs.Write(buffer, 0, buffer.Length);
        }
        public async Task ReWriteFileAsync(string directory, string fileName, Stream buffer)
        {
            var fullDirectoryPath = GetFullDirectoryPath(directory);
            if (!Directory.Exists(fullDirectoryPath))
                Directory.CreateDirectory(fullDirectoryPath);
            string filePath = Path.Combine(fullDirectoryPath, fileName);

            using (var fs = File.Open(filePath, FileMode.OpenOrCreate))
            {
                await buffer.CopyToAsync(fs);
            }
        }
        public void DeleteFile(string directory, string fileName)
        {
            if (TryGetFilePathWithExtension(directory, fileName, out string filePath))
                File.Delete(filePath);
        }
        public byte[] ReadFile(string directory, string fileName)
        {
            if (!TryGetFilePathWithExtension(directory, fileName, out string filePath))
                return default;
            return File.ReadAllBytes(filePath);
        }
        public async Task<byte[]> ReadFileAsync(string directory, string fileName)
        {
            if (!TryGetFilePathWithExtension(directory, fileName, out string filePath))
                return default;
            return await File.ReadAllBytesAsync(filePath);
        }
        public async Task<FileStreamModel> ReadFileByUriAsync(string uri)
        {
            return await _file.ReadFileByUriAsync(uri);
        }
        public FileInfoModel GetInfo(string directory, string fileName)
        {
            if (!TryGetFilePathWithExtension(directory, fileName, out string filePath))
                return null;
            var fileInfo = new FileInfo(filePath);
            
            return new() 
            {
                FilePath = Path.Combine(directory, fileInfo.Name),
                ContentType = FileExtension.GetContentType(fileInfo.Extension),
                FileSize = fileInfo.Length,
                FileDownloadName = fileName
            };
        }
        private string GetFullDirectoryPath(string directory, string file = null)
        {
            if (string.IsNullOrEmpty(file))
                return Path.Combine(webRootPath, directory);
            return Path.Combine(webRootPath, directory, file);
        }
        private bool TryGetFilePathWithExtension(string directory, string fileName, out string filePath)
        {
            filePath = null;
            const short takeLastSymbols = 5;
            if (string.IsNullOrEmpty(fileName)) return false;
            if (takeLastSymbols > fileName.Length) return false;

            string extension = fileName.Substring(fileName.Length - takeLastSymbols);
            if (extension.Contains('.'))
            {
                filePath =  GetFullDirectoryPath(directory, fileName);
                return File.Exists(filePath);
            }
                
            var fullDirectoryPath = GetFullDirectoryPath(directory);
            var nameFiles = Directory.GetFiles(fullDirectoryPath,
                string.Format("{0}.*", fileName), SearchOption.TopDirectoryOnly);
            if (nameFiles.Length == 0) return false;
            filePath =  nameFiles[0];
            return true;
        }
    }
}
