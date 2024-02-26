using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using NextPassswordAPI.Dto;
using NextPassswordAPI.Models;
using NextPassswordAPI.Repository.Interfaces;
using NextPassswordAPI.Services.Interfaces;
using System.Security.Cryptography;

namespace NextPassswordAPI.Services
{
    public class ItemService : IItemService
    {
        private readonly IItemRepository _itemRepository;

        public ItemService(IItemRepository itemRepository)
        {
            _itemRepository = itemRepository ?? throw new ArgumentNullException(nameof(itemRepository));
        }

        public async Task<List<Item>> GetAllItemAsync()
        {
            try
            {
                return await _itemRepository.GetAllItemAsync();
            }
            catch (Exception ex) { 
            
                throw new Exception($"Une erreur s'est produite lors de la récupération des mots de passe. ", ex);
            }
        }


        public async Task AddItemAsync(ItemDto itemDto)
        {
            try
            {
                if (itemDto == null)
                {
                    throw new ArgumentNullException(nameof(itemDto));
                }

                byte[] salt = RandomNumberGenerator.GetBytes(128 / 8); // divide by 8 to convert bits to bytes
                string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: itemDto?.PasswordHash!,
                    salt: salt,
                    prf: KeyDerivationPrf.HMACSHA256,
                    iterationCount: 100000,
                    numBytesRequested: 256 / 8));

                var item = new Item
                {
                    Title = itemDto.Title,
                    Notes = itemDto.Notes,
                    Url = itemDto.Url,
                    Username = itemDto.Username,
                    PasswordHash = hashed,
                };

                await _itemRepository.AddItemAsync(item);

            }
            catch (Exception ex)
            {
                throw new Exception($"Une erreur s'est produite lors de la création du mot de passe.", ex);
            }
        }


        public async Task DeleteItemAsync(Guid id)
        {
            try
            {
                await _itemRepository.DeleteItemAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Une erreur s'est produite lors de la supression du mot de passe.", ex);
            }
        }

        public async Task<Item?> FindByIdAsync(Guid id)
        {
            try
            {
                return await _itemRepository.FindByIdAsync(id)!;
            }
            catch (Exception ex)
            {
                throw new Exception($"Une erreur s'est produite lors de la récupération du mot de passe: {id}. ", ex);
            }
        }

        public Task<Item?> UpdateItemAsync(Item item, Guid id)
        {
            try
            {
                if (item == null)
                {
                    throw new ArgumentNullException(nameof(item));
                }

                return _itemRepository.UpdateItemAsync(item, id);
                    


            }catch (Exception ex)
            {
                throw new Exception("Une erreur s'est produite lors de la mise à jour du mot de passe.", ex);
            }
        }
    }
}
