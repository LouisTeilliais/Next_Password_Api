
using NextPassswordAPI.Dto;
using NextPassswordAPI.Models;

namespace NextPassswordAPI.Services.Interfaces
{
    public interface IItemService
    {
        public Task<List<Item>> GetAllItemAsync();
        public Task AddItemAsync(ItemDto itemDto); 
        public Task<Item?> FindByIdAsync(Guid id);
        public Task DeleteItemAsync(Guid id);
        public Task<Item?> UpdateItemAsync(Item item, Guid id);
    }
}
