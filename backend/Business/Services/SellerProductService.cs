using Jannara_Ecommerce.Business.Interfaces;
using Jannara_Ecommerce.DataAccess.Interfaces;
using Jannara_Ecommerce.DataAccess.Repositories;
using Jannara_Ecommerce.DTOs.General;
using Jannara_Ecommerce.DTOs.Product;
using Jannara_Ecommerce.DTOs.ProductItemImage;
using Jannara_Ecommerce.DTOs.SellerProduct;
using Jannara_Ecommerce.DTOs.SellerProductImage;
using Jannara_Ecommerce.DTOs.Variation;
using Jannara_Ecommerce.DTOs.VariationOption;
using Jannara_Ecommerce.Utilities;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace Jannara_Ecommerce.Business.Services
{
    public class SellerProductService : ISellerProductService
    {
        private readonly ISellerProductRepository _sellerProductRepository;
        private readonly ISellerProductImageService _sellerProductImageService;
        private readonly IImageService _imageService;
        private readonly IOptions<ImageSettings> _imageSettings;
        private readonly string _baseUrl;
        private readonly string _connectionString;
        private readonly ILogger<ISellerProductService> _logger;
        

        public SellerProductService(ISellerProductRepository sellerProductRepository, IImageService imageService,
            IOptions<ImageSettings> imageSettings, IOptions<AppSettings> appSettings,
            IOptions<DatabaseSettings> options, ILogger<ISellerProductService> logger,
            ISellerProductImageService sellerProductImageService

            )
        {
            _sellerProductRepository = sellerProductRepository;
            _imageService = imageService;
            _imageSettings = imageSettings;
            _baseUrl = appSettings.Value.BaseUrl;
            _connectionString = options.Value.DefaultConnection;
            _logger = logger;
            _sellerProductImageService = sellerProductImageService;
        }

        public async Task<Result<bool>> CreateAsync(SellerProductCreateDTO productCreateDTO)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            using var transaction = connection.BeginTransaction();
            var savedImages = new List<string>(); // for rollback
            try
            {

                List<SellerProductImageCreateDBDTO> ProductImages = new List<SellerProductImageCreateDBDTO>();
                // Save Seller Product images
                if (productCreateDTO.SellerProductImages != null)
                {
                    foreach (var image in productCreateDTO?.SellerProductImages)
                    {
                        var imageSaveResult = await _imageService.SaveImageAsync(image, _imageSettings.Value.ProductFolder);
                        if (!imageSaveResult.IsSuccess)
                        {
                            return await RollbackAndFail(transaction, savedImages);
                        }
                        savedImages.Add(imageSaveResult.Data);
                    }
                }

                // Product
                var productDTO = new SellerProductCreateDBDTO
                {
                        UserId = productCreateDTO.UserId,
                        ProductItemId = productCreateDTO.ProductItemId,
                        Price = productCreateDTO.Price,
                        StockQuantity = productCreateDTO.StockQuantity,
                };

                var addProductResult = await AddNewAsync(productDTO, connection, transaction);
                if (!addProductResult.IsSuccess)
                {
                    return await RollbackAndFail(transaction, savedImages);
                }

                int SellerProductId = addProductResult.Data.Id;

                if (savedImages != null)
                {
                    foreach (var image in savedImages)
                    {

                        var addSellerImageResult = await _sellerProductImageService.AddNewAsync(
                            new SellerProductImageCreateDBDTO { ImageUrl = image, SellerProductId = SellerProductId }
                            , connection, transaction);

                        if (!addSellerImageResult.IsSuccess)
                        {
                            return await RollbackAndFail(transaction, savedImages);
                        }
                    }
                }

                transaction.Commit();
                return new Result<bool>(true, addProductResult.Message, true, addProductResult.ErrorCode);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CreateProductWithImagesAsync failed");
                return await RollbackAndFail(transaction, savedImages);
            }
        }

        public async Task<Result<SellerProductDTO>> AddNewAsync(SellerProductCreateDBDTO product, SqlConnection connection, SqlTransaction transaction)
        {
            return await _sellerProductRepository.AddNewAsync(product, connection, transaction);
        }

        public async Task<Result<bool>> DeleteAsync(int id)
        {
            return await _sellerProductRepository.DeleteAsync(id);
        }

        public async Task<Result<PagedResponseDTO<SellerProductResponseDTO>>> GetAllAsync(SellerProductFilterDTO filter)
        {
            var ProductsResult = await _sellerProductRepository.GetAllAsync(filter);
            if (ProductsResult.IsSuccess)
            {
                foreach (var product in ProductsResult.Data.Items)
                {
                    product.ProductImage = ImageUrlHelper.ToAbsoluteUrl(product.ProductImage, _baseUrl);
                }
            }

            return ProductsResult;
        }

        public async Task<Result<SellerProductDTO>> UpdateAsync(int id, SellerProductUpdateDTO productUpdateDBDTO)
        {
            return await _sellerProductRepository.UpdateAsync(id, productUpdateDBDTO);
        }

        private async Task<Result<bool>> RollbackAndFail(
     SqlTransaction transaction,
     IEnumerable<string> images)
        {
            transaction.Rollback();
            await RollbackImages(images);
            return new Result<bool>(false, "Error creating product", false, 500);
        }

        private async Task RollbackImages(IEnumerable<string> images)
        {
            foreach (var img in images)
                await _imageService.DeleteImage(img);
        }

        public async Task<Result<SellerProductResponseForEdit>> GetSellerProductForEditAsync(int id)
        {
            var productResult = await _sellerProductRepository.GetSellerProductForEditAsync(id);
            foreach (var image in productResult.Data.SellerProductImages)
            {
                image.ImageUrl = ImageUrlHelper.ToAbsoluteUrl(image.ImageUrl, _baseUrl);
            }
            return productResult;
        }
    }
}
