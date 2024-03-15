using NextPassswordAPI.Models;

namespace NextPassswordAPI.Repository.Interfaces
{
    public interface IPasswordRepository
    { 
         Task<IEnumerable<Password>> GetAllPasswordByUserAsync(string userId);
         Task AddPasswordAsync(Password password);

         Task<Password?> FindByIdAsync(string userId, Guid id);
         Task DeletePasswordAsync(string userId, Guid id);

         Task<Password?> UpdatePasswordAsync(Password password, Guid id);
    }
}
