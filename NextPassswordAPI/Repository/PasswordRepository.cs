using Microsoft.EntityFrameworkCore;
using NextPassswordAPI.Data;
using NextPassswordAPI.Models;
using NextPassswordAPI.Repository.Interfaces;

namespace NextPassswordAPI.Repository
{
    public class PasswordRepository : IPasswordRepository
    {

        private readonly DataContext? _dataContext;

        public PasswordRepository(DataContext dataContext)
        {
            _dataContext = dataContext ?? throw new ArgumentNullException(nameof(dataContext)); ;
        }

        public async Task<IEnumerable<Password>> GetAllPasswordByUserAsync(string userId)
        {
            try
            {
                return await _dataContext!.Passwords.Where(p => p.UserId == userId).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Une erreur s'est produite lors de la récupération des mots de passe.", ex);
            }
        }

        public async Task AddPasswordAsync(Password password)
        {

            try
            {
                _dataContext!.Passwords.Add(password);
                await _dataContext.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                {
                    throw new Exception("Une erreur s'est produite lors de l'ajout du mot de passe.", ex);
                }
            }
        }

        public async Task DeletePasswordAsync(string userId, Guid id)
        {
            try
            {
                var password = await _dataContext!.Passwords.FirstOrDefaultAsync(p => p.Id == id && p.UserId == userId);

                if (password == null)
                {
                    throw new InvalidOperationException($"Le mot de passe à l'ID {password?.Id} n'est pas trouvé.");
                }

                _dataContext!.Passwords.Remove(password);
                await _dataContext!.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while deleting the product.", ex);
            }
        }

        public async Task<Password?> FindByIdAsync(string userId, Guid id)
        {
            try
            {
                return await _dataContext!.Passwords.FirstOrDefaultAsync(p => p.Id == id && p.UserId == userId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Une erreur s'est produite lors de la récupération du mot de passe {id}. ", ex);
            }

        }

        public async Task<Password> UpdatePasswordAsync(Password password, Guid id, string userId)
        {
            try
            {
                var existingPassword = await _dataContext.Passwords.FirstOrDefaultAsync(p => p.Id == id && p.UserId == userId);

                if (existingPassword == null)
                {
                    throw new InvalidOperationException($"Le mot de passe à l'ID {id} n'est pas trouvé.");
                }

                existingPassword.Title = password.Title;
                existingPassword.Url = password.Url;
                existingPassword.Username = password.Username;
                existingPassword.Notes = password.Notes;
                existingPassword.PasswordHash = password.PasswordHash;

                await _dataContext.SaveChangesAsync();

                return existingPassword;
            }
            catch (Exception ex)
            {
                throw new Exception($"Une erreur s'est produite lors de la récupération ou de la mise à jour du mot de passe {id}. ", ex);
            }
        }
    }
}
