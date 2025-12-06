using Jannara_Ecommerce.Business.Interfaces;
using Jannara_Ecommerce.Utilities;

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
            return new Result<bool>(true, "Passed validation", true);
        }

       

        public async Task<Result<string>> SaveImageAsync(IFormFile imageFile, string folderName)
        {
            Result<bool> validationResult = _validateFile(imageFile);
            if (!validationResult.IsSuccess)
                return new Result<string>(false, validationResult.Message, string.Empty, validationResult.ErrorCode);

            // Save in project-root/uploads/profiles
            var uploadPath = Path.Combine(_env.WebRootPath, "uploads", folderName);
            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            var fileName = Guid.NewGuid() + Path.GetExtension(imageFile.FileName);
            var filePath = Path.Combine(uploadPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }


            var relativePath = Path.Combine("uploads", "profiles", fileName).Replace("\\", "/");
            return new Result<string>(true, "Image saved sucessfuly", relativePath);
        }

        public Result<bool> DeleteImage(string relativePath)
        {
            if (string.IsNullOrEmpty(relativePath))
                return new Result<bool>(false, "invalide image url", false, 500);

            try
            {
                var absolutePath = Path.Combine(_env.WebRootPath, relativePath.Replace("/", "\\"));

                if (File.Exists(absolutePath))
                {
                    File.Delete(absolutePath);
                    return new Result<bool>(true, "Image deleted successfult", true);
                }
                return new Result<bool>(false, "invalide image url", false, 500);
            }
            catch
            {
                return new Result<bool>(false, "Uexpected error in the server", false, 500);
            }
        }

        public async Task<Result<string>> ReplaceImageAsync(string oldPath, IFormFile newFile, string folderName)
        {
            DeleteImage(oldPath);
            return await SaveImageAsync(newFile, folderName);
        }
    }
}
