using NextPassswordAPI.Models;

namespace NextPassswordAPI.Repository.Interfaces
{
    public interface IItemRepository
    { 
     Task AddItemAsync(Item item);

        Task<Item?> FindByIdAsync(Guid id);
     Task DeleteItemAsync(Guid id);

    }
}
