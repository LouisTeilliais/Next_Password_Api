using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Identity;
using NextPassswordAPI.Data;
using NextPassswordAPI.Dto;
using NextPassswordAPI.Migrations;
using NextPassswordAPI.Models;
using NextPassswordAPI.Repository.Interfaces;
using NextPassswordAPI.Services.Interfaces;
using System.Collections.Generic;
namespace NextPassswordAPI.Services
{
    public class PasswordService : IPasswordService
    {
        private readonly IPasswordRepository _passwordRepository;
        private readonly ITokenRepository _tokenRepository;
        private readonly IHashPasswordService _hashPasswordService;
        private readonly ITokenService _tokenService;
        private readonly UserManager<ApplicationUser> _userManager;

        public PasswordService(
            IPasswordRepository passwordRepository,
            IHashPasswordService hashPassword,
            UserManager<ApplicationUser> userManager,
            ITokenRepository tokenRepository,
            ITokenService tokenService)
        {
            _passwordRepository = passwordRepository ?? throw new ArgumentNullException(nameof(passwordRepository));
            this._hashPasswordService = hashPassword;
            _userManager = userManager;
            _tokenRepository = tokenRepository;
            _tokenService = tokenService;
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
            catch (Exception ex)
            {

                throw new Exception($"Une erreur s'est produite lors de la récupération des mots de passe. ", ex);
            }
        }

     

        /// <summary>
        /// Add Password encypted
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
                    PasswordId = passwordId,
                    NumberUses = 3
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
        /// Get Password By Id Async
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="passwordId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<Password?> GetPasswordByUserIdAsync(string userId, Guid passwordId)
        {
            try
            {
                var item = await _passwordRepository.FindByUserIdAsync(userId, passwordId);

                if (item == null)
                {
                    throw new Exception($"Le mot de passe avec l'ID {passwordId} n'a pas été trouvé.");
                }

                var token = await _tokenService.FindByIdAsync(item.TokenId);

                if (token == null)
                {
                    throw new Exception("Une erreur est survenue");
                }

               /* var password = _hashPasswordService.DecryptPassword(item.PasswordHash, token.TokenValue);*/

                var password = new Password
                {
                    Id = item.Id,
                    PasswordHash = item.PasswordHash,
                    Notes = item.Notes,
                    Url = item.Url,
                    Token = token
                };

                return password;
            }
            catch (Exception ex)
            {
                throw new Exception($"Une erreur s'est produite lors de la récupération du mot de passe: {passwordId}. ", ex);
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
        public async Task<Password?> UpdatePasswordAsync(Guid passwordId, PasswordDto passwordDto, string userId)
        {
            try
            {
                if (passwordDto == null)
                {
                    throw new ArgumentNullException(nameof(passwordDto));
                }

                var password = await this.GetPasswordByIdAsync(passwordId);

                var token = await _tokenService.FindByIdAsync(password!.TokenId);

                if (token == null)
                {
                    throw new Exception("Une erreur est survenue");
                }


                if (token.NumberUses > 0)
                {
                    token.NumberUses--;
                    await _tokenService.UpdateToken(token);

                    var passwordHashed = _hashPasswordService.EncryptPassword(passwordDto.PasswordHash, token.TokenValue);

                    var updatePassword = new Password
                    {
                        Title = passwordDto.Title,
                        Notes = passwordDto.Notes,
                        Url = passwordDto.Url,
                        Username = passwordDto.Username,
                        PasswordHash = passwordHashed,
                        UpdatedAt = DateTime.UtcNow
                    };

                    return await _passwordRepository.UpdatePasswordAsync(updatePassword, passwordId, userId);

                }else
                {
                    string tokenValue = _hashPasswordService.GenerateToken();

                    // Déchiffrez le mot de passe
                    var actualPassword = _hashPasswordService.DecryptPassword(password.PasswordHash, token.TokenValue);

                    var passwordHashed = _hashPasswordService.EncryptPassword(actualPassword, tokenValue);

                    var newToken = new Token
                    {
                        TokenValue = tokenValue,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        PasswordId = passwordId,
                        NumberUses = 3
                    };

                    await _tokenRepository.AddTokenAsync(newToken);

                    password.Title = passwordDto.Title;
                    password.Notes = passwordDto.Notes;
                    password.Url = passwordDto.Url;
                    password.Username = passwordDto.Username;
                    password.PasswordHash = passwordHashed;
                    password.UpdatedAt = DateTime.UtcNow;
                    password.TokenId = newToken.Id;

                    return await _passwordRepository.UpdatePasswordAsync(password, passwordId, userId);

                }


            }
            catch (Exception ex)
            {
                throw new Exception("Une erreur s'est produite lors de la mise à jour du mot de passe.", ex);
            }
        }

        /// <summary>
        /// Get password by Id for the middleware
        /// </summary>
        /// <param name="passwordId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<Password?> GetPasswordByIdAsync(Guid passwordId)
        {
            try
            {
                var item = await _passwordRepository.FindByIdAsync(passwordId);

                if (item == null)
                {
                    throw new Exception($"Le mot de passe avec l'ID {passwordId} n'a pas été trouvé.");
                }

                var password = new Password
                {
                    Id = item.Id,
                    PasswordHash = item.PasswordHash,
                    Notes = item.Notes,
                    Url = item.Url,
                    TokenId = item.TokenId,
                };

                return password;
            }
            catch (Exception ex)
            {
                throw new Exception($"Une erreur s'est produite lors de la récupération du mot de passe: {passwordId}. ", ex);
            }
        }
    }
}
