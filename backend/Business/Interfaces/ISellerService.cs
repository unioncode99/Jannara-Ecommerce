using Jannara_Ecommerce.DTOs;
using Jannara_Ecommerce.Utilities;
using Microsoft.Data.SqlClient;

namespace Jannara_Ecommerce.Business.Interfaces
{
    public interface ISellerService
    {
        public Task<Result<SellerDTO>> FindAsync(int id);
        public Task<Result<SellerDTO>> AddNewAsync(SellerDTO newSeller, SqlConnection connection, SqlTransaction transaction);
        public Task<Result<bool>> UpdateAsync(int id, SellerDTO updatedSeller);
        public Task<Result<bool>> DeleteAsync(int id);
        public Task<Result<SellerDTO>> RegisterAsync(RegisteredSellerDTO registeredSeller);
    }
}
