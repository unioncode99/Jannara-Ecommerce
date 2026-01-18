using Jannara_Ecommerce.Business.Interfaces;
using Jannara_Ecommerce.DataAccess.Interfaces;
using Jannara_Ecommerce.DTOs.ProductItemImage;
using Jannara_Ecommerce.DTOs.SellerProductImage;
using Jannara_Ecommerce.Utilities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace Jannara_Ecommerce.Business.Services
{
    public class SellerProductImageService : ISellerProductImageService
    {

        private readonly ISellerProductImageRepository _sellerProductImageRepository;
        private readonly string _connectionString;
        private readonly ILogger<IProductService> _logger;
        private readonly IImageService _imageService;
        private readonly IOptions<ImageSettings> _imageSettings;
        private readonly string _baseUrl;

        public SellerProductImageService(ISellerProductImageRepository sellerProductImageRepository, IImageService imageService,
            IOptions<ImageSettings> imageSettings, IOptions<DatabaseSettings> options, IOptions<AppSettings> appSettings)
        {
            _sellerProductImageRepository = sellerProductImageRepository;
            _imageSettings = imageSettings;
            _imageService = imageService;
            _connectionString = options.Value.DefaultConnection;
            _baseUrl = appSettings.Value.BaseUrl;
        }

        public async Task<Result<SellerProductImageDTO>> AddNewAsync(SellerProductImageCreateDBDTO productImage, SqlConnection connection, SqlTransaction transaction)
        {
            return await _sellerProductImageRepository.AddNewAsync(productImage, connection, transaction);
        }

        public async Task<Result<IEnumerable<SellerProductImageDTO>>> AddNewImagesAsync(SellerProductImageCreateOneDTO productImage)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            using var transaction = connection.BeginTransaction();
            var savedImages = new List<string>(); // for rollback

            try
            {
                var SellerProductImages = new List<SellerProductImageCreateDBDTO>();
                var ProductImages = new List<SellerProductImageDTO>();

                foreach (var img in productImage.Images)
                {
                    var imageSaveResult = await _imageService.SaveImageAsync(img, _imageSettings.Value.ProductFolder);
                    if (!imageSaveResult.IsSuccess)
                    {
                        return await RollbackAndFail(transaction, savedImages);
                    }
                    savedImages.Add(imageSaveResult.Data);

                    SellerProductImages.Add(new SellerProductImageCreateDBDTO
                    {
                        SellerProductId = productImage.SellerProductId,
                        ImageUrl = imageSaveResult.Data,
                    });
                }

                foreach (var image in SellerProductImages)
                {
                    var addSellerProductImageResult = await AddNewAsync(image, connection, transaction);

                    if (!addSellerProductImageResult.IsSuccess)
                    {
                        return await RollbackAndFail(transaction, savedImages);
                    }

                    addSellerProductImageResult.Data.ImageUrl = ImageUrlHelper.ToAbsoluteUrl(addSellerProductImageResult.Data.ImageUrl, _baseUrl);

                    ProductImages.Add(addSellerProductImageResult.Data);
                }
                transaction.CommitAsync();
                return new Result<IEnumerable<SellerProductImageDTO>>(true, "Image save successfully", ProductImages, 200);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CreateProductWithImagesAsync failed");
                return await RollbackAndFail(transaction, savedImages);
            }
        }

        public async Task<Result<bool>> DeleteAsync(int id)
        {
            return await _sellerProductImageRepository.DeleteAsync(id);
        }

        private async Task<Result<IEnumerable<SellerProductImageDTO>>> RollbackAndFail(
SqlTransaction transaction,
IEnumerable<string> images)
        {
            transaction.Rollback();
            await RollbackImages(images);
            return new Result<IEnumerable<SellerProductImageDTO>>(false, "Error creating product", null, 500);
        }

        private async Task RollbackImages(IEnumerable<string> images)
        {
            foreach (var img in images)
            {
                await _imageService.DeleteImage(img);
            }
        }
    }
}
