using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NextPassswordAPI.Dto;
using NextPassswordAPI.Models;
using NextPassswordAPI.Services.Interfaces;

namespace NextPassswordAPI.Controllers
{
    [ApiController]
    [Route("api/items"), Authorize]
    public class ItemController : ControllerBase
    {
        public readonly IItemService _itemService;

        public ItemController(IItemService itemService)
        {
            _itemService = itemService;
        }

        [HttpGet()]
        public async Task<IActionResult> GetAllItems()
        {
            var items = await _itemService.GetAllItemAsync();
            return Ok(items);
        }

        [HttpPost]
        public async Task<IActionResult> CreateItem([FromBody] ItemDto itemDto)
        {

            try
            {
                if (itemDto == null)
                {
                    return BadRequest("Item is empty !");
                }

                await _itemService.AddItemAsync(itemDto);
                return Ok();

            }
            catch (Exception e)
            {
                throw new Exception("Une erreur s'est produite à la création du mot de passe", e);
            }
        }

        [HttpGet("{itemId}")]
        public async Task<IActionResult> GetItemById(Guid itemId)
        {
            try
            {
                var item = await _itemService.FindByIdAsync(itemId);

                if (item == null)
                {
                    return NotFound(); // 404 si le produit n'est pas trouvé
                }

                var itemDto = new ItemDto
                {
                    Title = item.Title,
                    Notes = item.Notes,
                    Url = item.Url,
                    Username = item.Username,
                    PasswordHash= item.PasswordHash
                };

                return Ok(itemDto);

            }
            catch (Exception e)
            {
                throw new Exception("Le mot de passe que vous cherchez n'éxiste pas", e);
            }
        }

        [HttpDelete()]
        public async Task<IActionResult> DeleteItem(Guid id)
        {
            try
            {
                Item item = await _itemService.FindByIdAsync(id);

                if (item == null)
                {
                    return NotFound(); 
                }

                await _itemService.DeleteItemAsync(id);

                return Ok("Le mot de passe à bien été supprimé");
            }
            catch (Exception e)
            {
                throw new Exception("Le produit que vous cherchez n'éxiste pas", e);
            }
        }

        [HttpPut()]
        public async Task<IActionResult> UpdateItem(Guid id, [FromBody] Item item)
        {
            try
            {
                var updatedItemResult = await _itemService.UpdateItemAsync(item, id);

                if (updatedItemResult == null)
                {
                    return NotFound(); // 404 si le produit n'est pas trouvé
                }

                return Ok(updatedItemResult);
            }
            catch (Exception e)
            {
                throw new Exception("Une erreur s'est produite lors de la mise à jour du mot de passe", e);
            }
        }
    }
}
