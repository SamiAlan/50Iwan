using nQuant;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Processing;
using System.Drawing.Imaging;
using System.IO;

namespace Iwan.ImageManager
{
    public class ImageManipulator
    {
        public static byte[] CompressJpegImage(string imagePath, int quality = 90, int resolutionDecrease = 2)
        {
            using var image = System.Drawing.Image.FromFile(imagePath);
            using var ms = new MemoryStream();
         
            var encoder = GetEncoder(ImageFormat.Jpeg);
            var encoderParameters = new EncoderParameters(1);
            encoderParameters.Param[0] = new EncoderParameter(Encoder.Quality, quality);
            image.Save(ms, encoder, encoderParameters);

            var bytes = ms.ToArray();
            ms.Dispose();
            return ResizeImage(bytes, resolutionDecrease, Path.GetExtension(imagePath));
        }

        public static byte[] CompressPngImage(string imagePath, int resolutionDecrease = 2)
        {
            var quantizer = new WuQuantizer();
            using var bitmap = new System.Drawing.Bitmap(imagePath);
            using var image = quantizer.QuantizeImage(bitmap);
            using var ms = new MemoryStream();

            image.Save(ms, ImageFormat.Png);
            var bytes = ms.ToArray();
            ms.Dispose();
            image.Dispose();
            return ResizeImage(bytes, resolutionDecrease, Path.GetExtension(imagePath));
        }

        public static byte[] ResizeImage(byte[] bytes, int resolutionDecrease, string extension)
        {
            using var image = Image.Load(bytes);
            using var ms = new MemoryStream();

            var width = image.Width;
            var height = image.Height;

            var newWidth = 0;
            var newHeight = 0;

            if (width > height)
            {
                var ratio = (float)height / width;
                newWidth = width / resolutionDecrease;
                newHeight = (int)(ratio * newWidth);
            }
            else
            {
                var ratio = (float)width / height;
                newHeight = height / resolutionDecrease;
                newWidth = (int)(ratio * newHeight);
            }

            image.Mutate(p => p.Resize(newWidth, newHeight));

            image.Save(ms, GetImageFormatFromExtension(extension));
            return ms.ToArray();

        }

        private static ImageCodecInfo GetEncoder(ImageFormat format)
        {
            var codecs = ImageCodecInfo.GetImageDecoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }

            return null;
        }

        private static IImageFormat GetImageFormatFromExtension(string extension)
        {
            return extension switch
            {
                ".png" => PngFormat.Instance,
                _ => JpegFormat.Instance
            };
        }
    }
}
