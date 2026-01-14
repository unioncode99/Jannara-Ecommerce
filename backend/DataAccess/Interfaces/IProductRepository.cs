using Jannara_Ecommerce.DTOs;
using Jannara_Ecommerce.DTOs.Customer;
using Jannara_Ecommerce.DTOs.General;
using Jannara_Ecommerce.DTOs.Product;
using Jannara_Ecommerce.Utilities;
using Microsoft.Data.SqlClient;

namespace Jannara_Ecommerce.DataAccess.Interfaces
{
    public interface IProductRepository
    {
        public Task<Result<PagedResponseDTO<ProductResponseDTO>>> GetAllAsync(FilterProductDTO filter);
        public Task<Result<ProductDetailDTO>> GetByPublicIdAsync(Guid publicId, int? customerId);
        Task<Result<ProductDTO>> AddNewAsync(ProductCreateDBDTO product, SqlConnection connection, SqlTransaction transaction);
        Task<Result<PagedResponseDTO<ProductGeneralResponseDTO>>> GetAllGeneralAsync(GeneralProductFilterDTO filter);
        Task<Result<ProductDetailsForAdminDTO>> GetProductForEditAsync(Guid publicId);
        Task<Result<ProductDTO>> GetGeneralByIdAsync(Guid publicId);
        Task<Result<ProductDTO>> UpdateAsync(Guid publicId, ProductUpdateDBDTO productUpdateDBDTO);
    }
}
