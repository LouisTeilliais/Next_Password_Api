using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using NextPassswordAPI.Dto;
using NextPassswordAPI.Models;
using NextPassswordAPI.Repository.Interfaces;
using NextPassswordAPI.Services.Interfaces;
using System.Security.Cryptography;

namespace NextPassswordAPI.Services
{
    public class PasswordService : IPasswordService
    {
        private readonly IPasswordRepository _passwordRepository;

        public PasswordService(IPasswordRepository passwordRepository)
        {
            _passwordRepository = passwordRepository ?? throw new ArgumentNullException(nameof(passwordRepository));
        }

        public async Task<List<Password>> GetAllPasswordAsync()
        {
            try
            {
                return await _passwordRepository.GetAllPasswordAsync();
            }
            catch (Exception ex) { 
            
                throw new Exception($"Une erreur s'est produite lors de la récupération des mots de passe. ", ex);
            }
        }


        public async Task AddPasswordAsync(PasswordDto passwordDto)
        {
            try
            {
                if (passwordDto == null)
                {
                    throw new ArgumentNullException(nameof(passwordDto));
                }

                byte[] salt = RandomNumberGenerator.GetBytes(128 / 8); // divide by 8 to convert bits to bytes
                string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: passwordDto?.PasswordHash!,
                    salt: salt,
                    prf: KeyDerivationPrf.HMACSHA256,
                    iterationCount: 100000,
                    numBytesRequested: 256 / 8));

                var password = new Password
                {
                    Title = passwordDto.Title,
                    Notes = passwordDto.Notes,
                    Url = passwordDto.Url,
                    Username = passwordDto.Username,
                    PasswordHash = hashed,
                };

                await _passwordRepository.AddPasswordAsync(password);

            }
            catch (Exception ex)
            {
                throw new Exception($"Une erreur s'est produite lors de la création du mot de passe.", ex);
            }
        }


        public async Task DeletePasswordAsync(Guid id)
        {
            try
            {
                await _passwordRepository.DeletePasswordAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Une erreur s'est produite lors de la supression du mot de passe.", ex);
            }
        }

        public async Task<Password?> FindByIdAsync(Guid id)
        {
            try
            {
                return await _passwordRepository.FindByIdAsync(id)!;
            }
            catch (Exception ex)
            {
                throw new Exception($"Une erreur s'est produite lors de la récupération du mot de passe: {id}. ", ex);
            }
        }

        public Task<Password?> UpdatePasswordAsync(Password password, Guid id)
        {
            try
            {
                if (password == null)
                {
                    throw new ArgumentNullException(nameof(password));
                }

                return _passwordRepository.UpdatePasswordAsync(password, id);
                    


            }catch (Exception ex)
            {
                throw new Exception("Une erreur s'est produite lors de la mise à jour du mot de passe.", ex);
            }
        }
    }
}
