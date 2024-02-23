
using NextPassswordAPI.Dto;
using NextPassswordAPI.Models;

namespace NextPassswordAPI.Services.Interfaces
{
    public interface IItemService
    {
        public Task AddItemAsync(ItemDto itemDto);
        public Task<Item?> FindByIdAsync(Guid id);
        public Task DeleteItemAsync(Guid id);

    }
}
