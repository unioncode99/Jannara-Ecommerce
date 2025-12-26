using Jannara_Ecommerce.DTOs.Address;
using Jannara_Ecommerce.DTOs.State;
using Jannara_Ecommerce.Utilities;

namespace Jannara_Ecommerce.DataAccess.Interfaces
{
    public interface IStateRepository
    {
        public Task<Result<IEnumerable<StateDTO>>> GetAllAsync();
    }
}
