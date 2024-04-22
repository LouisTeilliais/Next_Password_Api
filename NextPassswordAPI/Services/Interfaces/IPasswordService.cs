
using NextPassswordAPI.Dto;
using NextPassswordAPI.Models;

namespace NextPassswordAPI.Services.Interfaces
{
    public interface IPasswordService
    {
        public Task<IEnumerable<Password>> GetAllPasswordByUserAsync(string userId);
        public Task AddPasswordAsync(string userId, PasswordDto passwordDto); 
        public Task<Password?> GetPasswordByUserIdAsync(string userId, Guid passwordId);
        public Task<Password?> GetPasswordByIdAsync(Guid passwordId);
        public Task DeletePasswordAsync(string userId, Guid id);
        public Task<Password?> UpdatePasswordAsync(Guid passwordId, PasswordDto passwordDto, string userId);
    }
}
