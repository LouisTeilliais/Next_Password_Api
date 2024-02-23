using NextPassswordAPI.Data;
using NextPassswordAPI.Models;
using NextPassswordAPI.Repository.Interfaces;

namespace NextPassswordAPI.Repository
{
    public class ItemRepository : IItemRepository
    {

        private readonly DataContext? _dataContext;

        public ItemRepository(DataContext dataContext)
        {
            _dataContext = dataContext ?? throw new ArgumentNullException(nameof(dataContext)); ;
        }

        public async Task AddItemAsync(Item item)
        {

            try
            {
                _dataContext.Items.Add(item);
                await _dataContext.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                {
                    throw new Exception("Une erreur s'est produite lors de l'ajout du mot de passe.", ex);
                }
            }
        }

        public async Task DeleteItemAsync(Guid id)
        {
            try
            {
                var item = await _dataContext.Items.FindAsync(id);

                if (item == null)
                {
                    throw new InvalidOperationException($"Le mot de passe à l'ID {item?.Id} n'est pas trouvé.");
                }

                _dataContext.Items.Remove(item);
                await _dataContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while deleting the product.", ex);
            }
        }

        public async Task<Item?> FindByIdAsync(Guid id)
        {
            try
            {
                return await _dataContext.Items.FindAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Une erreur s'est produite lors de la récupération du mot de passe {id}. ", ex);
            }

        }
    }
}
