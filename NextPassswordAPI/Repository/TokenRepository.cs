using Microsoft.EntityFrameworkCore;
using NextPassswordAPI.Data;
using NextPassswordAPI.Models;
using NextPassswordAPI.Repository.Interfaces;

namespace NextPassswordAPI.Repository
{
    public class TokenRepository : ITokenRepository
    {

        private readonly DataContext? _dataContext;

        public TokenRepository(DataContext dataContext)
        {
            _dataContext = dataContext ?? throw new ArgumentNullException(nameof(dataContext)); ;
        }

        public async Task AddTokenAsync(Token token)
        {

            try
            {
                _dataContext!.Tokens.Add(token);
                await _dataContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                {
                    throw new Exception("Une erreur s'est produite lors de l'ajout du mot de passe.", ex);
                }
            }
        }

        public async Task<Token?> GetTokenById(Guid? id)
        {
            try
            {
                return await _dataContext!.Tokens.FirstOrDefaultAsync(token => token.Id == id);
            }
            catch (Exception ex)
            {
                throw new Exception("Une erreur s'est produite lors de la récupération du token.", ex);
            }
        }
    }
}
