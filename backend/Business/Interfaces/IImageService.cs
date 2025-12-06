using Jannara_Ecommerce.Utilities;

namespace Jannara_Ecommerce.Business.Interfaces
{
    public interface IImageService
    {
        public  Task SaveImageAsync(IFormFile newImage, string folderName);
        public Result<bool> DeleteImage(string relativePath);
        public  Task ReplaceImageAsync(string oldPath, string newPath, IFormFile newImage);
        public Result<string> GetImageUrl(IFormFile newImage, string folderName);
    }
}
