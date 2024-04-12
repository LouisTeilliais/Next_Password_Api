using NextPassswordAPI.Models;
using NextPassswordAPI.Repository;
using NextPassswordAPI.Repository.Interfaces;
using NextPassswordAPI.Services.Interfaces;

namespace NextPassswordAPI.Services
{
    public class TokenService : ITokenService
    {
        private readonly ITokenRepository _tokenRepository;

        public TokenService(ITokenRepository tokenRepository) { 
            _tokenRepository = tokenRepository;
        }

        public async Task<Token?> FindByIdAsync(Guid? id)
        {
            try
            {
                return await _tokenRepository.GetTokenById(id)!;
            }
            catch (Exception ex)
            {
                throw new Exception($"Une erreur s'est produite lors de la récupération du mot de passe: {id}. ", ex);
            }
        }
    }
}
