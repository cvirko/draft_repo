namespace Auth.Domain.Interface.Logic.External.Files
{
    public interface IImageService
    {
        byte[] ReSizePng(Stream file, int width = 512, int height = 512);
    }
}
