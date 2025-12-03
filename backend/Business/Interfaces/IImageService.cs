using Jannara_Ecommerce.Utilities;

namespace Jannara_Ecommerce.Business.Interfaces
{
    public interface IImageService
    {
        public  Task<Result<string>> SaveImageAsync(IFormFile imageFile, string folderName);
        public Result<bool> DeleteImage(string relativePath);
        public Task<Result<string>> ReplaceImageAsync(string oldPath, IFormFile newFile, string folderName);
    }
}
