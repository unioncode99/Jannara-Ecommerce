using Jannara_Ecommerce.Business.Interfaces;
using Jannara_Ecommerce.Utilities;
using Jannara_Ecommerce.Utilities.WrapperClasses;
using System.IO;
using System.Security;

namespace Jannara_Ecommerce.Business.Services
{
    public class ImageService : IImageService
    {
        private readonly IWebHostEnvironment _env;
        private const long MaxFileSize = 5 * 1024 * 1024;

        private static readonly Dictionary<string, byte[]> ImageSignatures = new()
        {
            { ".jpg", new byte[] { 0xFF, 0xD8 } },
            { ".jpeg", new byte[] { 0xFF, 0xD8 } },
            { ".png", new byte[] { 0x89, 0x50 } },
            { ".gif", new byte[] { 0x47, 0x49 } },
            { ".webp", new byte[] { 0x52, 0x49 } }
        };

        public ImageService(IWebHostEnvironment env)
        {
            _env = env;
        }

        // ===================== VALIDATION =====================
        private Result<bool> ValidateImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return new Result<bool>(false, "empty_file", false, 400);

            if (file.Length > MaxFileSize)
                return new Result<bool>(false, "file_exceeds_max_size", false, 400);

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!ImageSignatures.ContainsKey(extension))
                return new Result<bool>(false, "invalid_file_type", false, 400);

            if (!IsValidImageContent(file, ImageSignatures[extension]))
                return new Result<bool>(false, "invalid_image_content", false, 400);

            return new Result<bool>(true, "image_validated", true);
        }

        private bool IsValidImageContent(IFormFile file, byte[] signature)
        {
            using var stream = file.OpenReadStream();
            var buffer = new byte[signature.Length];
            stream.Read(buffer, 0, buffer.Length);
            return buffer.SequenceEqual(signature);
        }

        // ===================== PATH SAFETY =====================
        private string UploadRoot =>
            Path.Combine(_env.WebRootPath, "Uploads");
        private string GetSafeFullPath(string relativePath)
        {
            var fullPath = Path.GetFullPath(
                Path.Combine(UploadRoot, relativePath)
            );

            if (!fullPath.StartsWith(UploadRoot))
                throw new SecurityException("Invalid path traversal");

            return fullPath;
        }
        // ===================== SAVE =====================
        public async Task<Result<string>> SaveImageAsync(IFormFile image, string folderName)
        {
            var validation = ValidateImage(image);
            if (!validation.IsSuccess)
                return new Result<string>(false, validation.Message, null, validation.ErrorCode);

            var extension = Path.GetExtension(image.FileName);
            var fileName = $"{Guid.NewGuid()}{extension}";
            var folderPath = Path.Combine(UploadRoot, folderName);

            Directory.CreateDirectory(folderPath);

            var fullPath = Path.Combine(folderPath, fileName);

            await using var stream = new FileStream(fullPath, FileMode.CreateNew);
            await image.CopyToAsync(stream);

            var relativePath = Path.Combine(folderName, fileName).Replace("\\", "/");

            return new Result<string>(true, "image_saved", relativePath);
        }
        // ===================== DELETE =====================
        public async Task<Result<bool>> DeleteImage(string relativePath)
        {
            if (string.IsNullOrWhiteSpace(relativePath))
            {
                return new Result<bool>(false, "invalid_image_url", false, 400);
            }

            try
            {
                var fullPath = GetSafeFullPath(relativePath);

                if (!File.Exists(fullPath))
                {
                    //return new Result<bool>(false, "file_not_found", false, 404);
                    return new Result<bool>(true, "file_already_deleted", true);
                }

                 await Task.Run(() => File.Delete(fullPath));
                return new Result<bool>(true, "image_deleted", true);
            }
            catch (SecurityException)
            {
                return new Result<bool>(false, "invalid_path", false, 400);
            }
            catch
            {
                return new Result<bool>(false, "internal_server_error", false, 500);
            }
        }
        // ===================== REPLACE =====================
        public async Task<Result<string>> ReplaceImageAsync(
    string? oldRelativePath,
    IFormFile newImage,
    string folderName)
        {
            // Delete old image first
            if (!string.IsNullOrWhiteSpace(oldRelativePath))
            {
                var deleteResult = await DeleteImage(oldRelativePath);
                if (!deleteResult.IsSuccess)
                    return new Result<string>(false, deleteResult.Message, null, deleteResult.ErrorCode);
            }

            // Save new image
            var saveResult = await SaveImageAsync(newImage, folderName);
            if (!saveResult.IsSuccess)
                return saveResult;

            return saveResult;
        }


    }
}
