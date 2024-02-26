using NextPassswordAPI.Models;

namespace NextPassswordAPI.Repository.Interfaces
{
    public interface IItemRepository
    { 
         Task<List<Item>> GetAllItemAsync();
         Task AddItemAsync(Item item);

         Task<Item?> FindByIdAsync(Guid id);
         Task DeleteItemAsync(Guid id);

         Task<Item?> UpdateItemAsync(Item item, Guid id);
    }
}
