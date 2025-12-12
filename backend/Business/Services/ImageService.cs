using Jannara_Ecommerce.Business.Interfaces;
using Jannara_Ecommerce.Utilities;

namespace Jannara_Ecommerce.Business.Services
{
    public class ImageService : IImageService
    {
        private readonly IWebHostEnvironment _env;
        public ImageService(IWebHostEnvironment env)
        {
            _env = env;
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
        public async Task ReplaceImageAsync(string oldPath, string newPath, IFormFile newImage)
        {
            DeleteImage(oldPath);
             await SaveImageAsync(newImage, newPath);
        }

        public async Task SaveImageAsync(IFormFile newImage,  string imagePath)
        {
            using (var stream = new FileStream(imagePath, FileMode.Create))
            {
                await newImage.CopyToAsync(stream);
            }
        }

        public Result<string> GetImageUrl(IFormFile newImage, string folderName)
        {
            Result<bool> validationResult = _validateFile(newImage);
            if (!validationResult.IsSuccess)
                return new Result<string>(false, validationResult.Message, string.Empty, validationResult.ErrorCode);

            var uploadPath = Path.Combine(_env.WebRootPath, "Uploads", folderName);
            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);
            var fileName = Guid.NewGuid() + Path.GetExtension(newImage.FileName);
            string imageUrl =  Path.Combine(uploadPath, fileName);
            var relativePath = Path.Combine("Uploads",  imageUrl).Replace("\\", "/");
            return new Result<string>(true, "Image url generated successfully", relativePath);
        }

       
    }
}
