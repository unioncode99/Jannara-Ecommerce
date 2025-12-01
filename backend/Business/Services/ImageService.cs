using Jannara_Ecommerce.Business.Interfaces;
using Jannara_Ecommerce.Utilities;
using Microsoft.AspNetCore.Http;

namespace Jannara_Ecommerce.Business.Services
{
    public class ImageService : IImageService
    {
        private readonly IWebHostEnvironment _env;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ImageService(IWebHostEnvironment env, IHttpContextAccessor httpContextAccessor)
        {
            _env = env;
            _httpContextAccessor = httpContextAccessor;
        }
        Result<bool> _validateFile(IFormFile imageFile)
        {

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
            var extension = Path.GetExtension(imageFile.FileName).ToLower();
            if (!allowedExtensions.Contains(extension))
                return new Result<bool>(false, "Invalid file type.", false, 400);

            if (imageFile.Length > 5 * 1024 * 1024)
                return new Result<bool>(false, "File too large (max 5MB).", false, 400);
            return new Result<bool>(false, "Passed validation", true);
        }

        public async Task<Result<string>> SaveProductImageAsync(IFormFile imageFile)
        {
            Result<bool> validationResult =   _validateFile(imageFile);
            if (!validationResult.IsSuccess)
                return new Result<string>(false, validationResult.Message, string.Empty, validationResult.ErrorCode);

            var uploadPath = Path.Combine(_env.WebRootPath, "products"); 
            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            var fileName = Guid.NewGuid() + Path.GetExtension(imageFile.FileName);
            var filePath = Path.Combine(uploadPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            string relativePath = Path.Combine("products", fileName);
            return new Result<string>(true, "Image saved sucessfuly", relativePath);
        }

        public async Task<Result<string>> SaveProfileImageAsync(IFormFile imageFile)
        {
            Result<bool> validationResult = _validateFile(imageFile);
            if (!validationResult.IsSuccess)
                return new Result<string>(false, validationResult.Message, string.Empty, validationResult.ErrorCode);

            // Save in project-root/uploads/profiles
            var uploadPath = Path.Combine(_env.ContentRootPath, "uploads", "profiles");
            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            var fileName = Guid.NewGuid() + Path.GetExtension(imageFile.FileName);
            var filePath = Path.Combine(uploadPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }


            var relativePath = Path.Combine("uploads", "profiles", fileName);
            return new Result<string>(true, "Image saved sucessfuly", relativePath);
        }
    }
}
