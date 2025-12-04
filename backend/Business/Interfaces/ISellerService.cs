using Jannara_Ecommerce.DTOs;
using Jannara_Ecommerce.DTOs.General;
using Jannara_Ecommerce.DTOs.Seller;
using Jannara_Ecommerce.Utilities;
using Microsoft.Data.SqlClient;

namespace Jannara_Ecommerce.Business.Interfaces
{
    public interface ISellerService
    {
        public Task<Result<SellerDTO>> FindAsync(int id);
        public Task<Result<SellerDTO>> AddNewAsync(int userId, SellerCreateDTO newSeller, SqlConnection connection, SqlTransaction transaction);
        public Task<Result<bool>> UpdateAsync(int id, SellerUpdateDTO updatedSeller);
        public Task<Result<bool>> DeleteAsync(int id);
        public Task<Result<SellerDTO>> CreateAsync(SellerCreateRequestDTO sellerCreateRequestDTO);
        public Task<Result<PagedResponseDTO<SellerDTO>>> GetAllAsync(int pageNumber, int pageSize);
    }
}
