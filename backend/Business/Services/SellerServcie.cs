using Jannara_Ecommerce.Business.Interfaces;
using Jannara_Ecommerce.DataAccess.Interfaces;
using Jannara_Ecommerce.DTOs;
using Jannara_Ecommerce.Utilities;

namespace Jannara_Ecommerce.Business.Services
{
    public class SellerServcie : ISellerService
    {
        private readonly ISellerRepository _repo;
        public SellerServcie( ISellerRepository repo)
        {
            _repo  = repo;
        }
        public async Task<Result<SellerDTO>> AddNewAsync(SellerDTO newSeller)
        {
            return await _repo.AddNewAsync(newSeller);
        }

        public async Task<Result<bool>> DeleteAsync(int id)
        {
            return await _repo.DeleteAsync(id);
        }

        public async Task<Result<SellerDTO>> FindAsync(int id)
        {
            return await _repo.GetByIdAsync(id);
        }

        public async Task<Result<bool>> UpdateAsync(int id, SellerDTO updatedSeller)
        {
            return await _repo.UpdateAsync(id, updatedSeller);
        }
    }
}
