using Jannara_Ecommerce.DTOs.General;
using Jannara_Ecommerce.DTOs.SellerProduct;
using Jannara_Ecommerce.DTOs.SellerProductImage;
using Jannara_Ecommerce.Utilities;
using Microsoft.Data.SqlClient;

namespace Jannara_Ecommerce.DataAccess.Interfaces
{
    public interface ISellerProductRepository
    {
        Task<Result<PagedResponseDTO<SellerProductResponseDTO>>> GetAllAsync(SellerProductFilterDTO filter);
        Task<Result<SellerProductDTO>> AddNewAsync(SellerProductCreateDBDTO product, SqlConnection connection, SqlTransaction transaction);
        Task<Result<SellerProductDTO>> UpdateAsync(int id, SellerProductUpdateDTO productUpdateDBDTO);
        Task<Result<bool>> DeleteAsync(int id);
        Task<Result<SellerProductResponseForEdit>> GetSellerProductForEditAsync(int id);
    }
}
