using Auth.Domain.Core.Logic.Models.File;

namespace Auth.Domain.Interface.Logic.Read.ModelBuilder.ServiceBuilder
{
    public interface IFileBuilder : IBuilder
    {
        T GetFromJson<T>(string path);
        string GetFromJson(string path);
        void ReWriteFile(string directory, string fileName, byte[] buffer);
        byte[] ReadFile(string directory, string fileName);
        Task<byte[]> ReadFileAsync(string directory, string fileName);
        Task<FileStreamModel> ReadFileByUriAsync(string uri);
        void DeleteFile(string directory, string fileName);
    }
}
