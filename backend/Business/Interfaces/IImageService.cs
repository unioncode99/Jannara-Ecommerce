using Jannara_Ecommerce.Utilities;
using Jannara_Ecommerce.Utilities.WrapperClasses;

namespace Jannara_Ecommerce.Business.Interfaces
{
    public interface IImageService
    {
        public  Task SaveImageAsync(IFormFile newImage, string folderName);
        public Result<bool> DeleteImage(string relativePath);
        public  Task ReplaceImageAsync(string oldPath, string newPath, IFormFile newImage);
        public Result<GetImageUrlsResult> GetImageUrls(IFormFile newImage, string folderName);
    }
}
