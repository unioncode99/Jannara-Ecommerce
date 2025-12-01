using Jannara_Ecommerce.DTOs;
using Jannara_Ecommerce.Utilities;

namespace Jannara_Ecommerce.Business.Interfaces
{
    public interface ISellerService
    {
        public Task<Result<SellerDTO>> FindAsync(int id);
        public Task<Result<SellerDTO>> AddNewAsync(SellerDTO newSeller);
        public Task<Result<bool>> UpdateAsync(int id, SellerDTO updatedSeller);
        public Task<Result<bool>> DeleteAsync(int id);
    }
}
