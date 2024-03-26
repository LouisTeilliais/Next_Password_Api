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

/*        private const int SaltSize = 16;

        private const int HashSize = 20;
        public static string Hash(string password, int iterations)
        {
            // Create salt
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[SaltSize]);

            // Create hash
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations);
            var hash = pbkdf2.GetBytes(HashSize);

            // Combine salt and hash
            var hashBytes = new byte[SaltSize + HashSize];
            Array.Copy(salt, 0, hashBytes, 0, SaltSize);
            Array.Copy(hash, 0, hashBytes, SaltSize, HashSize);

            // Convert to base64
            var base64Hash = Convert.ToBase64String(hashBytes);

            // Format hash with extra information
            return string.Format("$MYHASH$V1${0}${1}", iterations, base64Hash);
        }
*/


        public async Task AddPasswordAsync(string userId, PasswordDto passwordDto, string securityStamp)
        {
            try
            {
                if (passwordDto == null)
                {
                    throw new ArgumentNullException(nameof(passwordDto));
                }

                /*string hashed = Hash(securityStamp, 10);*/


                /*byte[] salt = RandomNumberGenerator.GetBytes(128 / 8); // divide by 8 to convert bits to bytes
                string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: passwordDto?.PasswordHash!,
                    salt: salt,
                    prf: KeyDerivationPrf.HMACSHA256,
                    iterationCount: 100000,
                    numBytesRequested: 256 / 8));
*/
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
