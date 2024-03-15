
using NextPassswordAPI.Dto;
using NextPassswordAPI.Models;

namespace NextPassswordAPI.Services.Interfaces
{
    public interface IPasswordService
    {
        public Task<IEnumerable<Password>> GetAllPasswordByUserAsync(string userId);
        public Task AddPasswordAsync(string userId, PasswordDto passwordDto); 
        public Task<Password?> FindByIdAsync(string userId, Guid id);
        public Task DeletePasswordAsync(string userId, Guid id);
        public Task<Password?> UpdatePasswordAsync(Password Password, Guid id);
    }
}
