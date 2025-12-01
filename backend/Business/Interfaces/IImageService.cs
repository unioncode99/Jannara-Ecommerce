using Jannara_Ecommerce.Utilities;

namespace Jannara_Ecommerce.Business.Interfaces
{
    public interface IImageService
    {
        Task<Result<string>> SaveProductImageAsync(IFormFile imageFile);
        Task<Result<string>> SaveProfileImageAsync(IFormFile imageFile);

    }
}
