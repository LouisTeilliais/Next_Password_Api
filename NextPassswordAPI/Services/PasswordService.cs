using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Identity;
using NextPassswordAPI.Data;
using NextPassswordAPI.Dto;
using NextPassswordAPI.Models;
using NextPassswordAPI.Repository.Interfaces;
using NextPassswordAPI.Services.Interfaces;
namespace NextPassswordAPI.Services
{
    public class PasswordService : IPasswordService
    {
        private readonly IPasswordRepository _passwordRepository;
        private readonly ITokenRepository _tokenRepository;
        private readonly IHashPasswordService _hashPasswordService;
        private readonly UserManager<ApplicationUser> _userManager;

        public PasswordService(IPasswordRepository passwordRepository, IHashPasswordService hashPassword, UserManager<ApplicationUser> userManager, ITokenRepository tokenRepository)
        {
            _passwordRepository = passwordRepository ?? throw new ArgumentNullException(nameof(passwordRepository));
            this._hashPasswordService = hashPassword;
            _userManager = userManager;
            _tokenRepository = tokenRepository;
        }

        /// <summary>
        /// Get all password by user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<IEnumerable<Password>> GetAllPasswordByUserAsync(string userId)
        {
            try
            {
                return await _passwordRepository.GetAllPasswordByUserAsync(userId);
            }
            catch (Exception ex) { 
            
                throw new Exception($"Une erreur s'est produite lors de la récupération des mots de passe. ", ex);
            }
        }


        /// <summary>
        /// Add Password hashed
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="passwordDto"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="Exception"></exception>
        public async Task AddPasswordAsync(string userId, PasswordDto passwordDto)
        {
            try
            {
                if (passwordDto == null)
                {
                    throw new ArgumentNullException(nameof(passwordDto));
                }

                // Generate a unique token value
                string tokenValue = _hashPasswordService.GenerateToken();

                // Create a new Token entity
                string hashed = _hashPasswordService.EncryptPassword(passwordDto.PasswordHash!, tokenValue);

                // Create a new Password entity
                var password = new Password
                {
                    Title = passwordDto!.Title,
                    Notes = passwordDto.Notes,
                    Url = passwordDto.Url,
                    Username = passwordDto.Username,
                    PasswordHash = hashed,
                    UserId = userId,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                };

                await _passwordRepository.AddPasswordAsync(password);

                // Récupère l'ID généré du mot de passe
                Guid passwordId = password.Id;

                var token = new Token
                {
                    TokenValue = tokenValue,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    PasswordId = passwordId
                };

                await _tokenRepository.AddTokenAsync(token);

                // Met à jour le mot de passe avec l'ID du token
                password.TokenId = token.Id;

                await _passwordRepository.UpdatePasswordAsync(password, password.Id, userId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Une erreur s'est produite lors de la création du mot de passe.", ex);
            }
        }

        /// <summary>
        /// Delete Password
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task DeletePasswordAsync(string userId, Guid id)
        {
            try
            {
                await _passwordRepository.DeletePasswordAsync(userId, id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Une erreur s'est produite lors de la supression du mot de passe.", ex);
            }
        }

        /// <summary>
        /// Find Password by Id
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<Password?> FindByIdAsync(string userId, Guid id)
        {
            try
            {
                return await _passwordRepository.FindByIdAsync(userId, id)!;
            }
            catch (Exception ex)
            {
                throw new Exception($"Une erreur s'est produite lors de la récupération du mot de passe: {id}. ", ex);
            }
        }

        /// <summary>
        /// Update Password
        /// </summary>
        /// <param name="id"></param>
        /// <param name="passwordDto"></param>
        /// <param name="securityStamp"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="Exception"></exception>
        public Task<Password?> UpdatePasswordAsync(Guid id, PasswordDto passwordDto, string securityStamp, string userId)
        {
            try
            {
                if (passwordDto == null)
                {
                    throw new ArgumentNullException(nameof(passwordDto));
                }

                string hashed = _hashPasswordService.EncryptPassword(passwordDto.PasswordHash!, securityStamp);

                var password = new Password
                {
                    Title = passwordDto.Title,
                    Notes = passwordDto.Notes,
                    Url = passwordDto.Url,
                    Username = passwordDto.Username,
                    PasswordHash = hashed,
                    UpdatedAt = DateTime.UtcNow
                };

                return _passwordRepository.UpdatePasswordAsync(password, id, userId);
            }
            catch (Exception ex)
            {
                throw new Exception("Une erreur s'est produite lors de la mise à jour du mot de passe.", ex);
            }
        }
    }
}
