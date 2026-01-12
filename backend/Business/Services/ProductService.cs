using Jannara_Ecommerce.Business.Interfaces;
using Jannara_Ecommerce.DataAccess.Interfaces;
using Jannara_Ecommerce.DTOs.General;
using Jannara_Ecommerce.DTOs.Product;
using Jannara_Ecommerce.DTOs.ProductItem;
using Jannara_Ecommerce.DTOs.ProductItemImage;
using Jannara_Ecommerce.DTOs.Seller;
using Jannara_Ecommerce.DTOs.Variation;
using Jannara_Ecommerce.DTOs.VariationOption;
using Jannara_Ecommerce.Utilities;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
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

        public ProductService(IProductRepository productRepository, IImageService imageService,
            IOptions<ImageSettings> imageSettings, IOptions<DatabaseSettings> options,
            ILogger<IProductService> logger)
        {
            _productRepository = productRepository;
            _imageSettings = imageSettings;
            _imageService = imageService;
            _connectionString = options.Value.DefaultConnection;
            _logger = logger;
        }

        public async Task<Result<ProductDetailDTO>> FindAsync(Guid publicId, int? customerId)
        {
            return await _productRepository.GetByPublicIdAsync(publicId, customerId);
        }

        public async Task<Result<PagedResponseDTO<ProductResponseDTO>>> GetAllAsync(FilterProductDTO filter)
        {
            return await _productRepository.GetAllAsync(filter);
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

                // Map API DTO to DB DTO
                var dbProduct = new ProductCreateDBDTO
                {
                    BrandId = productCreateDTO.BrandId,
                    CategoryId = productCreateDTO.CategoryId,
                    DefaultImageUrl = defaultImageUrl,
                    NameEn = productCreateDTO.NameEn,
                    NameAr = productCreateDTO.NameAr,
                    DescriptionEn = productCreateDTO.DescriptionEn,
                    DescriptionAr = productCreateDTO.DescriptionAr,
                    WeightKg = productCreateDTO.WeightKg,
                    Variations = productCreateDTO.Variations.Select(v => new VariationCreateDTO
                    {
                        NameEn = v.NameEn,
                        NameAr = v.NameAr,
                        VariationOptions = v.VariationOptions.Select(o => new VariationOptionCreateDTO
                        {
                            ValueEn = o.ValueEn,
                            ValueAr = o.ValueAr
                        }).ToList()
                    }).ToList(),

                    ProductItems = productCreateDTO.ProductItems.Select(pi => new ProductItemCreateDBDTO
                    {
                        Sku = pi.Sku,
                        VariationOptions = pi.VariationOptions.Select(o => new VariationOptionCreateDTO
                        {
                            ValueEn = o.ValueEn,
                            ValueAr = o.ValueAr
                        }).ToList(),
                        ProductItemImages = imageMap[pi.Sku]
                    }).ToList()
                };
                // Serialize DB DTO to JSON
                string json = JsonSerializer.Serialize(productCreateDTO);
                var addResult = await _productRepository.AddNewAsync(json, connection, transaction);
                transaction.Commit();
                return new Result<bool>(true, addResult.Message, false, addResult.ErrorCode);
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                foreach (var path in savedImages)
                {
                    await _imageService.DeleteImage(path);
                }

                _logger.LogError(ex, "CreateProductWithImagesAsync failed");
                return new Result<bool>(false, "Error creating product", false, 500);

            }
        }
    }
}
