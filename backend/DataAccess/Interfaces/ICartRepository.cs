using Jannara_Ecommerce.DTOs.Cart;
using Jannara_Ecommerce.DTOs.Person;
using Jannara_Ecommerce.Utilities;
using Microsoft.Data.SqlClient;

namespace Jannara_Ecommerce.DataAccess.Interfaces
{
    public interface ICartRepository
    {
        Task<Result<CartResponseDTO>> GetActiveCartAsync(int customerId);
        Task<Result<bool>> ClearCartAsync(int cartId);
    }
}
