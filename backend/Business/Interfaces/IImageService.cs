using Jannara_Ecommerce.Utilities;
using Jannara_Ecommerce.Utilities.WrapperClasses;

namespace Jannara_Ecommerce.Business.Interfaces
{
    public interface IImageService
    {
        Task<Result<string>> SaveImageAsync(IFormFile image, string folderName);
        Result<bool> DeleteImage(string relativePath);
        Task<Result<string>> ReplaceImageAsync(
            string? oldRelativePath,
            IFormFile newImage,
            string folderName);
    }
}
