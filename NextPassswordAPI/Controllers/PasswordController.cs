using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NextPassswordAPI.Dto;
using NextPassswordAPI.Models;
using NextPassswordAPI.Services.Interfaces;

namespace NextPassswordAPI.Controllers
{
    [ApiController]
    [Route("api/password"), Authorize]
    public class PasswordController : ControllerBase
    {
        public readonly IPasswordService _passwordService;
        private readonly UserManager<ApplicationUser> _userManager;

        public PasswordController(UserManager<ApplicationUser> userManager, IPasswordService passwordService)
        {
            _userManager = userManager;
            _passwordService = passwordService;
        }

        [HttpGet()]
        public async Task<IActionResult> GetAllPasswordByUserAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            var items = await _passwordService.GetAllPasswordByUserAsync(user.Id);
            return Ok(items);
        }

        [HttpPost]
        public async Task<IActionResult> CreateItem([FromBody] PasswordDto passwordDto)
        {

            try
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return Unauthorized(); // User not found or not authenticated
                }

                if (passwordDto == null)
                {
                    return BadRequest("Item is empty !");
                }

                await _passwordService.AddPasswordAsync(user.Id, passwordDto);
                return Ok();

            }
            catch (Exception e)
            {
                throw new Exception("Une erreur s'est produite à la création du mot de passe", e);
            }
        }

        [HttpGet("{passwordId}")]
        public async Task<IActionResult> GetPasswordById(Guid itemId)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);

                if (user == null)
                {
                    return NotFound(); // User not found or not authenticated
                }


                var item = await _passwordService.FindByIdAsync(user.Id, itemId);

                if (item == null)
                {
                    return NotFound(); // 404 si le produit n'est pas .
                }

                var passwordDto = new PasswordDto
                {
                    Title = item.Title,
                    Notes = item.Notes,
                    Url = item.Url,
                    Username = item.Username,
                    PasswordHash= item.PasswordHash
                };

                return Ok(passwordDto);

            }
            catch (Exception e)
            {
                throw new Exception("Le mot de passe que vous cherchez n'éxiste pas", e);
            }
        }

        [HttpDelete()]
        public async Task<IActionResult> DeletePassword(Guid id)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);

                Password password = await _passwordService.FindByIdAsync(user!.Id, id);

                if (password == null)
                {
                    return NotFound(); 
                }

                await _passwordService.DeletePasswordAsync(user.Id, id);

                return Ok("Le mot de passe à bien été supprimé");
            }
            catch (Exception e)
            {
                throw new Exception("Le produit que vous cherchez n'éxiste pas", e);
            }
        }

        [HttpPut()]
        public async Task<IActionResult> UpdatePassword(Guid id, [FromBody] Password password)
        {
            try
            {
                var updatedPasswordResult = await _passwordService.UpdatePasswordAsync(password, id);

                if (updatedPasswordResult == null)
                {
                    return NotFound(); // 404 si le produit n'est pas trouvé
                }

                return Ok(updatedPasswordResult);
            }
            catch (Exception e)
            {
                throw new Exception("Une erreur s'est produite lors de la mise à jour du mot de passe", e);
            }
        }
    }
}
