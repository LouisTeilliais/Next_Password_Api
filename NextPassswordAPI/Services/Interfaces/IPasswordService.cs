
using NextPassswordAPI.Dto;
using NextPassswordAPI.Models;

namespace NextPassswordAPI.Services.Interfaces
{
    public interface IPasswordService
    {
        public Task<IEnumerable<Password>> GetAllPasswordByUserAsync(string userId);
        public Task AddPasswordAsync(string userId, PasswordDto passwordDto); 
        public Task<Password?> FindByIdAsync(Guid id);
        public Task DeletePasswordAsync(Guid id);
        public Task<Password?> UpdatePasswordAsync(Password Password, Guid id);
    }
}
