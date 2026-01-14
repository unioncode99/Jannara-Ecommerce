using Jannara_Ecommerce.DTOs.Customer;
using Jannara_Ecommerce.DTOs.General;
using Jannara_Ecommerce.DTOs.Product;
using Jannara_Ecommerce.DTOs.Seller;
using Jannara_Ecommerce.Utilities;
using Microsoft.Data.SqlClient;

namespace Jannara_Ecommerce.Business.Interfaces
{
    public interface IProductService
    {
        public Task<Result<PagedResponseDTO<ProductResponseDTO>>> GetAllAsync(FilterProductDTO filter);
        public Task<Result<ProductDetailDTO>> FindAsync(Guid publicId, int? customerId);
        Task<Result<bool>> CreateAsync(ProductCreateDTO productCreateDTO);

        Task<Result<ProductDTO>> AddNewAsync(ProductCreateDBDTO product, SqlConnection connection, SqlTransaction transaction);
        Task<Result<PagedResponseDTO<ProductDTO>>> GetAllGeneralAsync(GeneralProductFilterDTO filter);
    }
}
