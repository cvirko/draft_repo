using Auth.Domain.Core.Logic.Models.File;

namespace Auth.Domain.Interface.Logic.External.Files
{
    public interface IFileService
    {
        Task<FileStreamModel> ReadFileByUriAsync(string uri);
    }
}
