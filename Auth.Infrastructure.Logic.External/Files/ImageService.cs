using Auth.Domain.Interface.Logic.External.Files;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Processing;

namespace Auth.Infrastructure.Logic.External.Files
{
    internal class ImageService : IImageService
    {
        public byte[] ReSizePng(Stream file, int width = 512)
        {
            if (file == null) return null;
            file.Position = 0;
            using (var smallStream = new MemoryStream())
            using (var image = Image.Load(file))
            {
                ReSize(image, width);
                image.Save(smallStream, new PngEncoder());
                return smallStream.ToArray();
            }
        }
        public async Task<byte[]> ReSizePngAsync(Stream file, int width = 512)
        {
            if (file == null) return null;
            file.Position = 0;
            using (var smallStream = new MemoryStream())
            using (var image = await Image.LoadAsync(file))
            {
                ReSize(image, width);
                await image.SaveAsync(smallStream, new PngEncoder());
                return smallStream.ToArray();
            }
        }
        private void ReSize(Image image, int width = 512)
        {
            if (image == null) return;
            int height = image.Height;
            if (image.Width > width)
            {
                height = (int)((double)width / image.Width * image.Height);
            }
            image.Mutate(context => context
                .Resize(new ResizeOptions
                {
                    Mode = ResizeMode.Max,
                    Size = new Size(width, height)
                })
            );
            image.Metadata.ExifProfile = null;
        }
    }
}
