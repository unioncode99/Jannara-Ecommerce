using Jannara_Ecommerce.Business.Interfaces;
using Jannara_Ecommerce.DataAccess.Interfaces;
using Jannara_Ecommerce.DataAccess.Repositories;
using Jannara_Ecommerce.DTOs.General;
using Jannara_Ecommerce.DTOs.Person;
using Jannara_Ecommerce.DTOs.Product;
using Jannara_Ecommerce.DTOs.ProductItem;
using Jannara_Ecommerce.DTOs.ProductItemImage;
using Jannara_Ecommerce.DTOs.Seller;
using Jannara_Ecommerce.DTOs.User;
using Jannara_Ecommerce.DTOs.Variation;
using Jannara_Ecommerce.DTOs.VariationOption;
using Jannara_Ecommerce.Utilities;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using Stripe;
using Stripe.Climate;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Intrinsics.Arm;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;

namespace Jannara_Ecommerce.Business.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IImageService _imageService;
        private readonly IOptions<ImageSettings> _imageSettings;
        private readonly string _connectionString;
        private readonly ILogger<IProductService> _logger;
        private readonly string _baseUrl;
        private readonly IVariationService _variationService;
        private readonly IVariationOptionService _variationOptionService;
        private readonly IProductItemService _productItemService;
        private readonly IProductItemImageService _productItemImageService;
        private readonly IProductItemVariationOptionService _productItemVariationOptionService;

        public ProductService(IProductRepository productRepository, IImageService imageService,
            IOptions<ImageSettings> imageSettings, IOptions<DatabaseSettings> options,
            ILogger<IProductService> logger, IOptions<AppSettings> appSettings,
            IProductItemImageService productItemImageService, 
            IVariationService variationService,
            IVariationOptionService variationOptionService,
            IProductItemService productItemService, 
            IProductItemVariationOptionService productItemVariationOptionService)
        {
            _productRepository = productRepository;
            _imageSettings = imageSettings;
            _imageService = imageService;
            _connectionString = options.Value.DefaultConnection;
            _logger = logger;
            _baseUrl = appSettings.Value.BaseUrl;
            _productItemImageService = productItemImageService;
            _variationService = variationService;
            _variationOptionService = variationOptionService;
            _productItemService = productItemService;
            _productItemVariationOptionService = productItemVariationOptionService;
        }

        public async Task<Result<ProductDetailDTO>> FindAsync(Guid publicId, int? customerId)
        {
            var ProductResult = await _productRepository.GetByPublicIdAsync(publicId, customerId);
            
            if (ProductResult.IsSuccess)
            {
                ProductResult.Data.DefaultImageUrl = ImageUrlHelper.ToAbsoluteUrl(ProductResult.Data.DefaultImageUrl, _baseUrl);
            }

            return ProductResult;
        }

        public async Task<Result<PagedResponseDTO<ProductResponseDTO>>> GetAllAsync(FilterProductDTO filter)
        {
            var ProductsResult = await _productRepository.GetAllAsync(filter);
            if (ProductsResult.IsSuccess)
            {
                foreach (var product in ProductsResult.Data.Items)
                {
                    product.DefaultImageUrl = ImageUrlHelper.ToAbsoluteUrl(product.DefaultImageUrl, _baseUrl);
                }
            }


            return ProductsResult;
        }
       
        public async Task<Result<bool>> CreateAsync(ProductCreateDTO productCreateDTO)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            using var transaction = connection.BeginTransaction();
            var savedImages = new List<string>(); // for rollback
            try
            {
                // Save default product image
                string defaultImageUrl = string.Empty;
                if (productCreateDTO.DefaultImageFile != null)
                {
                    var imageResult = await _imageService.SaveImageAsync(productCreateDTO.DefaultImageFile, _imageSettings.Value.ProductFolder);
                    if (!imageResult.IsSuccess)
                    {
                         return new Result<bool>(false, imageResult.Message, false, imageResult.ErrorCode);
                    }

                    defaultImageUrl = imageResult.Data;
                    savedImages.Add(defaultImageUrl);
                }

                // Save ProductItem images
                var imageMap = new Dictionary<string, List<ProductItemImageCreateDBDTO>>();
                foreach (var item in productCreateDTO.ProductItems)
                {
                    var images = new List<ProductItemImageCreateDBDTO>();
                    foreach (var img in item.ProductItemImages)
                    {
                        var imageSaveResult = await _imageService.SaveImageAsync(img.ImageFile, _imageSettings.Value.ProductFolder);
                        if (!imageSaveResult.IsSuccess)
                        {
                            foreach (var path in savedImages)
                            {
                                await _imageService.DeleteImage(path);

                            }
                            return new Result<bool>(false, imageSaveResult.Message, false, imageSaveResult.ErrorCode);
                        }
                        savedImages.Add(imageSaveResult.Data);

                        images.Add(new ProductItemImageCreateDBDTO
                        {
                            ImageUrl = imageSaveResult.Data,
                            IsPrimary = img.IsPrimary
                        });
                    }
                    imageMap[item.Sku] = images;
                }

                // Product
                var productDTO = new ProductCreateDBDTO
                {
                    BrandId = productCreateDTO.BrandId,
                    CategoryId = productCreateDTO.CategoryId,
                    DefaultImageUrl = defaultImageUrl,
                    NameEn = productCreateDTO.NameEn,
                    NameAr = productCreateDTO.NameAr,
                    DescriptionEn = productCreateDTO.DescriptionEn,
                    DescriptionAr = productCreateDTO.DescriptionAr,
                    WeightKg = productCreateDTO.WeightKg,
                };

                var addProductResult = await AddNewAsync(productDTO, connection, transaction);
                if (!addProductResult.IsSuccess)
                {
                    return await RollbackAndFail(transaction, savedImages);
                }

                int productId = addProductResult.Data.Id;

                // Variations
                // for item options
                var variationOptionMap = new Dictionary<string, int>();
                foreach (var variation in productCreateDTO.Variations)
                {
                    var variationDTO = new VariationCreateDTO
                    {
                        NameEn = variation.NameEn,
                        NameAr = variation.NameAr,  
                    };

                    var addVariationResult = await _variationService.AddNewAsync(productId, variationDTO, connection, transaction);

                   
                    if (!addVariationResult.IsSuccess)
                    {
                        return await RollbackAndFail(transaction, savedImages);
                    }

                    int variationId = addVariationResult.Data.Id;

                    foreach (var option in variation.VariationOptions)
                    {
                        var optionDTO = new VariationOptionCreateDTO
                        {
                            ValueEn = option.ValueEn,
                            ValueAr = option.ValueAr,
                        };

                        var addVariationOptionResult = await _variationOptionService.AddNewAsync(variationId, optionDTO, connection, transaction);
                        // for item options
                        if (!addVariationOptionResult.IsSuccess)
                        {
                            return await RollbackAndFail(transaction, savedImages);
                        }
                        variationOptionMap[option.ValueEn] = variationId;
                    }

                }

                // Product Items
                foreach (var productItem in productCreateDTO.ProductItems)
                {

                    var addProductItemResult = await _productItemService.AddNewAsync(productId, productItem.Sku, connection, transaction);

                    if (!addProductItemResult.IsSuccess)
                    {
                        return await RollbackAndFail(transaction, savedImages);
                    }

                    int productItemId = addProductItemResult.Data.Id;

                    foreach (var image in imageMap[productItem.Sku])
                    {
                        var addProductItemImageResult = await _productItemImageService.AddNewAsync(productItemId, image, connection, transaction);

                        if (!addProductItemImageResult.IsSuccess)
                        {
                            return await RollbackAndFail(transaction, savedImages);
                        }
                    }

                    foreach (var selectionOption in productItem.VariationOptions)
                    {
                        var key = selectionOption.ValueEn;

                        if (!variationOptionMap.TryGetValue(key, out var variationOptionId))
                        {
                            throw new Exception($"Variation option not found: {key}");
                        }

                        var addProductItemVariationOptionResult = await _productItemVariationOptionService.AddNewAsync(productItemId, variationOptionId, connection, transaction);
                        if (!addProductItemVariationOptionResult.IsSuccess)
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

        public async Task<Result<ProductDTO>> AddNewAsync(ProductCreateDBDTO product, SqlConnection connection, SqlTransaction transaction)
        {
            return await _productRepository.AddNewAsync(product, connection, transaction);
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

        public async Task<Result<PagedResponseDTO<ProductGeneralResponseDTO>>> GetAllGeneralAsync(GeneralProductFilterDTO filter)
        {
            var ProductsResult = await _productRepository.GetAllGeneralAsync(filter);
            if (ProductsResult.IsSuccess)
            {
                foreach (var product in ProductsResult.Data.Items)
                {
                    product.DefaultImageUrl = ImageUrlHelper.ToAbsoluteUrl(product.DefaultImageUrl, _baseUrl);
                }
            }

            return ProductsResult;
        }

        public async Task<Result<ProductDetailsForAdminDTO>> GetProductForEditAsync(Guid publicId)
        {
            var productResult = await _productRepository.GetProductForEditAsync(publicId);
            productResult.Data.DefaultImageUrl = ImageUrlHelper.ToAbsoluteUrl(productResult.Data.DefaultImageUrl, _baseUrl);
            foreach (var item in productResult.Data.ProductItems)
            {
                foreach (var image in item.ProductItemImages)
                {
                    image.ImageUrl = ImageUrlHelper.ToAbsoluteUrl(image.ImageUrl, _baseUrl);
                }
            }
            return productResult;
        }

        public async Task<Result<ProductDTO>> GetGeneralByIdAsync(Guid publicId)
        {
            return await _productRepository.GetGeneralByIdAsync(publicId);
        }

        public async Task<Result<ProductDTO>> UpdateAsync(Guid publicId, ProductUpdateDTO productUpdateDTO)
        {
            var findResult = await GetGeneralByIdAsync(publicId);
            if (!findResult.IsSuccess)
            {
                return new Result<ProductDTO>(false, findResult.Message, null, findResult.ErrorCode);
            }
            string? finalImageUrl = findResult.Data.DefaultImageUrl;
            // Handle delete-only request
            if (productUpdateDTO.DeleteProductImage && !string.IsNullOrWhiteSpace(finalImageUrl))
            {
                var deleteResult = await _imageService.DeleteImage(finalImageUrl);
                if (!deleteResult.IsSuccess)
                {
                    return new Result<ProductDTO>(false, deleteResult.Message, null, deleteResult.ErrorCode);
                }

                finalImageUrl = null;
            }

            // Handle new uploaded image
            if (productUpdateDTO.DefaultImageFile != null)
            {
                // Delete old image if it exists (after deleteProfileImage handled)
                if (!string.IsNullOrWhiteSpace(finalImageUrl))
                {
                    var deleteOld = await _imageService.DeleteImage(finalImageUrl);
                    if (!deleteOld.IsSuccess)
                    {
                        return new Result<ProductDTO>(false, deleteOld.Message, null, deleteOld.ErrorCode);
                    }
                }
                // Save new image
                var saveResult = await _imageService.SaveImageAsync(
           productUpdateDTO.DefaultImageFile,
           _imageSettings.Value.ProductFolder);

                if (!saveResult.IsSuccess)
                {
                    return new Result<ProductDTO>(false, saveResult.Message, null, saveResult.ErrorCode);
                }
                // Update final image path for DB
                finalImageUrl = saveResult.Data;
            }

            var productDTO = new ProductUpdateDBDTO
            {
                BrandId = productUpdateDTO.BrandId,
                CategoryId = productUpdateDTO.CategoryId,
                NameEn = productUpdateDTO.NameEn,
                NameAr = productUpdateDTO.NameAr,
                DescriptionEn = productUpdateDTO.DescriptionEn,
                DescriptionAr = productUpdateDTO.DescriptionAr,
                WeightKg = productUpdateDTO.WeightKg,
                DefaultImageUrl = finalImageUrl,

            };
            // Update DB with new image path
            var updateResult = await _productRepository.UpdateAsync(publicId, productDTO);
            updateResult.Data.DefaultImageUrl = ImageUrlHelper.ToAbsoluteUrl(updateResult.Data.DefaultImageUrl, _baseUrl);
            return updateResult;
        }

        public async Task<Result<bool>> DeleteAsync(int id)
        {
            return await _productRepository.DeleteAsync(id);
        }

        public async Task<Result<IEnumerable<ProductDropdownDTO>>> GetProductDropdownAsync(ProductDropdownRequestDTO request)
        {
            return await _productRepository.GetProductDropdownAsync(request);   
        }
    }
}
