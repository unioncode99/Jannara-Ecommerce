using Jannara_Ecommerce.Business.Interfaces;
using Jannara_Ecommerce.DataAccess.Interfaces;
using Jannara_Ecommerce.DataAccess.Repositories;
using Jannara_Ecommerce.DTOs.ProductItemImage;
using Jannara_Ecommerce.Utilities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace Jannara_Ecommerce.Business.Services
{
    public class ProductItemImageService : IProductItemImageService
    {

        private readonly IProductItemImageRepository _productItemImageRepository;
        private readonly string _connectionString;
        private readonly ILogger<IProductService> _logger;
        private readonly IImageService _imageService;
        private readonly IOptions<ImageSettings> _imageSettings;
        private readonly string _baseUrl;

        public ProductItemImageService(IProductItemImageRepository productItemImageRepository, IImageService imageService,
            IOptions<ImageSettings> imageSettings, IOptions<DatabaseSettings> options, IOptions<AppSettings> appSettings)
        {
            _productItemImageRepository = productItemImageRepository;
            _imageSettings = imageSettings;
            _imageService = imageService;
            _connectionString = options.Value.DefaultConnection;
            _baseUrl = appSettings.Value.BaseUrl;
        }

        public async Task<Result<ProductItemImageDTO>> AddNewAsync(int productItemId, ProductItemImageCreateDBDTO productItemImage, SqlConnection connection, SqlTransaction transaction)
        {
            return await _productItemImageRepository.AddNewAsync(productItemId, productItemImage, connection, transaction);
        }

        public async Task<Result<IEnumerable<ProductItemImageDTO>>> AddNewImagesAsync(ProductItemImageCreateOneDTO productItem)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            using var transaction = connection.BeginTransaction();
            var savedImages = new List<string>(); // for rollback
            
            try
            {
                var images = new List<ProductItemImageCreateDBDTO>();
                var ProductItems = new List<ProductItemImageDTO>();

                foreach (var img in productItem.ProductItemImages)
                {
                    var imageSaveResult = await _imageService.SaveImageAsync(img.ImageFile, _imageSettings.Value.ProductFolder);
                    if (!imageSaveResult.IsSuccess)
                    {
                        return await RollbackAndFail(transaction, savedImages);
                    }
                    savedImages.Add(imageSaveResult.Data);

                    images.Add(new ProductItemImageCreateDBDTO
                    {
                        ImageUrl = imageSaveResult.Data,
                        IsPrimary = img.IsPrimary
                    });
                }

                foreach (var image in images)
                {
                    var addProductItemImageResult = await AddNewAsync(productItem.ProductItemId, image, connection, transaction);

                    if (!addProductItemImageResult.IsSuccess)
                    {
                        return await RollbackAndFail(transaction, savedImages);
                    }

                    addProductItemImageResult.Data.ImageUrl = ImageUrlHelper.ToAbsoluteUrl(addProductItemImageResult.Data.ImageUrl, _baseUrl);

                    ProductItems.Add(addProductItemImageResult.Data);
                }

                return new Result<IEnumerable<ProductItemImageDTO>>(true, "Image save successfully", ProductItems, 200);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CreateProductWithImagesAsync failed");
                return await RollbackAndFail(transaction, savedImages);
            }
        }

        public async Task<Result<bool>> DeleteAsync(int id)
        {
            return await _productItemImageRepository.DeleteAsync(id);
        }

        public async Task<Result<ProductItemImageDTO>> SetPrimaryAsync(int id)
        {
            var setPrimaryResult = await _productItemImageRepository.SetPrimaryAsync(id);
            setPrimaryResult.Data.ImageUrl = ImageUrlHelper.ToAbsoluteUrl(setPrimaryResult.Data.ImageUrl, _baseUrl);
            return setPrimaryResult;
        }

        private async Task<Result<IEnumerable<ProductItemImageDTO>>> RollbackAndFail(
   SqlTransaction transaction,
   IEnumerable<string> images)
        {
            transaction.Rollback();
            await RollbackImages(images);
            return new Result<IEnumerable<ProductItemImageDTO>>(false, "Error creating product", null, 500);
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
