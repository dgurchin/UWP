using System.Threading.Tasks;

namespace Inventory.Services
{
    public interface IImageService
    {
        /// <summary>
        /// Изменить размер изображения
        /// </summary>
        /// <param name="imageData"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        Task<byte[]> ResizeAsync(byte[] imageData, int width, int height);

        /// <summary>
        /// Ширина превю картинки по умолчанию
        /// </summary>
        /// <returns></returns>
        Task<int> GetDefaultThumbnailWidthAsync();

        /// <summary>
        /// Высота превю картинки по умолчанию
        /// </summary>
        /// <returns></returns>
        Task<int> GetDefaultThumbnailHeightAsync();
    }
}
