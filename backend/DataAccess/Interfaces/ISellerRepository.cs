using Jannara_Ecommerce.DTOs.Seller;
using Jannara_Ecommerce.Utilities;
using Microsoft.Data.SqlClient;

namespace Jannara_Ecommerce.DataAccess.Interfaces
{
    public interface ISellerRepository
    {
        public Task<Result<SellerDTO>> GetByIdAsync(int id);
        public Task<Result<IEnumerable<SellerDTO>>> GetAllAsync(int pageNumber = 1, int pageSize = 20);
        public Task<Result<SellerDTO>> AddNewAsync(int userId, SellerCreateDTO newSeller, SqlConnection connection, SqlTransaction transaction);
        public Task<Result<bool>> UpdateAsync(int id, SellerUpdateDTO updatedSeller);
        public Task<Result<bool>> DeleteAsync(int id);
    }
}
