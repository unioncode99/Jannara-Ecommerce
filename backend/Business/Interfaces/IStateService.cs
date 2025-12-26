using Jannara_Ecommerce.DTOs.State;
using Jannara_Ecommerce.Utilities;

namespace Jannara_Ecommerce.Business.Interfaces
{
    public interface IStateService
    {
        public Task<Result<IEnumerable<StateDTO>>> GetAllAsync();
    }
}
