using NextPassswordAPI.Models;

namespace NextPassswordAPI.Repository.Interfaces
{
    public interface IPasswordRepository
    {
        Task<IEnumerable<Password>> GetAllPasswordByUserAsync(string userId);
        Task AddPasswordAsync(Password password);
        Task<Password?> FindByUserIdAsync(string userId, Guid id);
        Task<Password?> FindByIdAsync(Guid id);
        Task DeletePasswordAsync(string userId, Guid id);
        Task<Password?> UpdatePasswordAsync(Password password, Guid id, string userId);
        Task<Password> UpdatePasswordTokenAsync(Password password);
    }
}
