using NextPassswordAPI.Models;

namespace NextPassswordAPI.Services.Interfaces
{
    public interface ITokenService
    {
        public Task<Token?> FindByIdAsync(Guid? id);

        public Task AddToken(Token token);

        public Task<Token?> UpdateToken(Token token);
    }
}
