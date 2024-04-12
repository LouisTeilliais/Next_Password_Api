using NextPassswordAPI.Models;

namespace NextPassswordAPI.Services.Interfaces
{
    public interface ITokenService
    {
        public Task<Token?> FindByIdAsync(Guid? id);
    }
}
