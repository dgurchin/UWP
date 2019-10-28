using System;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;

using Inventory.Models.Enums;

using Windows.Graphics.Imaging;
using Windows.Storage.Streams;

namespace Inventory.Services
{
    public class ImageService : IImageService
    {
        private IVariableService VariableService { get; }

        public ImageService(IVariableService variableService)
        {
            VariableService = variableService;
        }

        public async Task<int> GetDefaultThumbnailHeightAsync()
        {
            return await VariableService.GetVariableValueAsync(VariableStrings.ThumbnailHeight, 56);
        }

        public async Task<int> GetDefaultThumbnailWidthAsync()
        {
            return await VariableService.GetVariableValueAsync(VariableStrings.ThumbnailWidth, 68);
        }

        public async Task<byte[]> ResizeAsync(byte[] imageData, int width, int height)
        {
            if (imageData == null || imageData.Length == 0)
            {
                return null;
            }

            var memStream = new MemoryStream(imageData);

            IRandomAccessStream imageStream = memStream.AsRandomAccessStream();
            var decoder = await BitmapDecoder.CreateAsync(imageStream);
            if (decoder.PixelHeight > height || decoder.PixelWidth > width)
            {
                using (imageStream)
                {
                    var resizedStream = new InMemoryRandomAccessStream();

                    BitmapEncoder encoder = await BitmapEncoder.CreateForTranscodingAsync(resizedStream, decoder);
                    double widthRatio = (double)width / decoder.PixelWidth;
                    double heightRatio = (double)height / decoder.PixelHeight;

                    double scaleRatio = Math.Min(widthRatio, heightRatio);

                    if (width == 0)
                        scaleRatio = heightRatio;

                    if (height == 0)
                        scaleRatio = widthRatio;

                    uint aspectHeight = (uint)Math.Floor(decoder.PixelHeight * scaleRatio);
                    uint aspectWidth = (uint)Math.Floor(decoder.PixelWidth * scaleRatio);

                    encoder.BitmapTransform.InterpolationMode = BitmapInterpolationMode.Linear;

                    encoder.BitmapTransform.ScaledHeight = aspectHeight;
                    encoder.BitmapTransform.ScaledWidth = aspectWidth;

                    await encoder.FlushAsync();
                    resizedStream.Seek(0);
                    var outBuffer = new byte[resizedStream.Size];
                    await resizedStream.ReadAsync(outBuffer.AsBuffer(), (uint)resizedStream.Size, InputStreamOptions.None);
                    return outBuffer;
                }
            }
            return imageData;
        }
    }
}
