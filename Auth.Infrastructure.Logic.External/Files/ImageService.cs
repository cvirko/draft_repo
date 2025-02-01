using Auth.Domain.Interface.Logic.External.Files;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Processing;

namespace Auth.Infrastructure.Logic.External.Files
{
    internal class ImageService : IImageService
    {
        public byte[] ReSizePng(Stream file, int width = 512, int height = 512)
        {
            if (file == null) return null;
            file.Position = 0;
            using (var smallStream = new MemoryStream())
            using (var image = Image.Load(file))
            {
                var clone = image.Clone(context => context
                .Resize(new ResizeOptions
                {
                    Mode = ResizeMode.Max,
                    Size = new Size(width, height)
                }));
                clone.Save(smallStream, new PngEncoder());
                return smallStream.ToArray();
            }
        }
    }
}
