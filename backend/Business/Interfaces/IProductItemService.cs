using Jannara_Ecommerce.DTOs.ProductItem;
using Jannara_Ecommerce.Utilities;
using Microsoft.Data.SqlClient;

namespace Jannara_Ecommerce.Business.Interfaces
{
    public interface IProductItemService
    {
        Task<Result<ProductItemDTO>> AddNewAsync(int productId,
            string sku,
            SqlConnection connection, SqlTransaction transaction);

        Task<Result<IEnumerable<ProductItemDropdown>>> GetProductDropdownAsync(ProductItemDropdownRequest request);
    }
}
