using Jannara_Ecommerce.Business.Interfaces;
using Jannara_Ecommerce.DataAccess.Interfaces;
using Jannara_Ecommerce.DataAccess.Repositories;
using Jannara_Ecommerce.DTOs.General;
using Jannara_Ecommerce.DTOs.SellerProduct;
using Jannara_Ecommerce.Utilities;
using Microsoft.Extensions.Options;

namespace Jannara_Ecommerce.Business.Services
{
    public class SellerProductService : ISellerProductService
    {
        private readonly ISellerProductRepository _sellerProductRepository;
        private readonly IImageService _imageService;
        private readonly IOptions<ImageSettings> _imageSettings;
        private readonly string _baseUrl;

        public SellerProductService(ISellerProductRepository sellerProductRepository, IImageService imageService,
            IOptions<ImageSettings> imageSettings, IOptions<AppSettings> appSettings
            )
        {
            _sellerProductRepository = sellerProductRepository;
            _imageService = imageService;
            _imageSettings = imageSettings;
            _baseUrl = appSettings.Value.BaseUrl;
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
    }
}
