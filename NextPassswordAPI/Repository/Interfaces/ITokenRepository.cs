using NextPassswordAPI.Models;

namespace NextPassswordAPI.Repository.Interfaces
{
    public interface ITokenRepository
    {
        Task AddTokenAsync(Token token);

        Task<Token?> GetTokenById(Guid? id);

    }
}
