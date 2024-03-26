﻿using Microsoft.EntityFrameworkCore;
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

        public async Task<Password> UpdatePasswordAsync(Password password, Guid id)
        {
            try
            {

                var updatedProduct = await _dataContext!.Passwords.FindAsync(id);

                if (updatedProduct == null)
                {
                    throw new InvalidOperationException($"Le mot de passe n'a pas été trouvé.");
                }

                updatedProduct.Title = password.Title;
                updatedProduct.Url = password.Url;
                updatedProduct.Username = password.Username;
                updatedProduct.Notes = password.Notes;
                updatedProduct.PasswordHash = password.PasswordHash;

                _dataContext!.Entry(updatedProduct).State = EntityState.Modified;

                await _dataContext!.SaveChangesAsync();

                return updatedProduct;
            }
            catch (Exception ex)
            {
                throw new Exception($"Une erreur s'est produite lors de la récupération du mot de passe {id}. ", ex);
            }
        }
    }
}