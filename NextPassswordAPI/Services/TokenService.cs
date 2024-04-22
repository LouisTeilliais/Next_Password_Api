using NextPassswordAPI.Models;
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

        public async Task AddToken(Token token)
        {
            try
            {
                if (token == null)
                {
                    throw new ArgumentNullException("Erreur de création du token");
                }

                var newToken = new Token
                {
                    TokenValue = token.TokenValue,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    PasswordId = token.PasswordId,
                    NumberUses = token.NumberUses
                }; 

                await _tokenRepository.AddTokenAsync(newToken);

            } catch (Exception ex)
            {
                throw new Exception($"Une erreur s'est produite lors de la création du mot de passe ", ex);
            }
        }

        public async Task<Token?> UpdateToken(Token token)
        {
            try
            {
                var existingToken = await _tokenRepository.GetTokenById(token.Id);

                if (existingToken == null)
                {
                    throw new InvalidOperationException($"Le token à l'ID {token.Id} n'est pas trouvé.");
                }

                // Mettre à jour les propriétés du token
                existingToken.TokenValue = token.TokenValue;
                existingToken.PasswordId = token.PasswordId;
                existingToken.NumberUses = token.NumberUses;

                return await _tokenRepository.UpdateToken(token);
            }
            catch (Exception ex)
            {
                throw new Exception("Une erreur s'est produite lors de la mise à jour du token.", ex);
            }
        }
    }
}
