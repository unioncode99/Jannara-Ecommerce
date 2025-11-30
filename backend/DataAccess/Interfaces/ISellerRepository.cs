using Jannara_Ecommerce.DTOs;
using Jannara_Ecommerce.Utilities;

namespace Jannara_Ecommerce.DataAccess.Interfaces
{
    public interface ISellerRepository
    {
        public Task<Result<SellerDTO>> GetByIdAsync(int id);
        public Task<Result<IEnumerable<SellerDTO>>> GetAllAsync(int pageNumber = 1, int pageSize = 20);
        public Task<Result<SellerDTO>> AddNewAsync(SellerDTO newSeller);
        public Task<Result<bool>> UpdateAsync(int id, SellerDTO updatedSeller);
        public Task<Result<bool>> DeleteAsync(int id);
    }
}
