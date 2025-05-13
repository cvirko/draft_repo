namespace Auth.Domain.Interface.Logic.External.Files
{
    public interface IImageService
    {
        byte[] ReSizePng(Stream file, int width = 512);
        Task<byte[]> ReSizePngAsync(Stream file, int width = 512);
    }
}
