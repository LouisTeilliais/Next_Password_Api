using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Identity;
using NextPassswordAPI.Dto;
using NextPassswordAPI.Models;
using NextPassswordAPI.Repository.Interfaces;
using NextPassswordAPI.Services.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace NextPassswordAPI.Services
{
    public class PasswordService : IPasswordService
    {
        private readonly IPasswordRepository _passwordRepository;
        private readonly IHashPassword _hashPassword;
        private readonly UserManager<ApplicationUser> _userManager;


        public PasswordService(IPasswordRepository passwordRepository, IHashPassword hashPassword, UserManager<ApplicationUser> userManager)
        {
            _passwordRepository = passwordRepository ?? throw new ArgumentNullException(nameof(passwordRepository));
            this._hashPassword = hashPassword;
            _userManager = userManager;
        }

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
        /// <param name="securityStamp"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="Exception"></exception>
        public async Task AddPasswordAsync(string userId, PasswordDto passwordDto, string securityStamp)
        {
            try { 
            
                if (passwordDto == null)
                {
                    throw new ArgumentNullException(nameof(passwordDto));
                }

                string hashed = _hashPassword.HashPasswordWithUniqueSalt(passwordDto.PasswordHash!, securityStamp);

                var password = new Password
                {
                    Title = passwordDto!.Title,
                    Notes = passwordDto.Notes,
                    Url = passwordDto.Url,
                    Username = passwordDto.Username,
                    PasswordHash = hashed,
                    UserId= userId
                    
                };

                await _passwordRepository.AddPasswordAsync(password);

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

                string hashed = _hashPassword.HashPasswordWithUniqueSalt(passwordDto.PasswordHash!, securityStamp);

                var password = new Password
                {
                    Title = passwordDto.Title,
                    Notes = passwordDto.Notes,
                    Url = passwordDto.Url,
                    Username = passwordDto.Username,
                    PasswordHash = hashed
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
