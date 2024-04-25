using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NextPassswordAPI.Dto;
using NextPassswordAPI.Models;
using NextPassswordAPI.Repository.Interfaces;
using NextPassswordAPI.Services.Interfaces;

namespace NextPassswordAPI.Controllers
{
    [ApiController]
    [Route("api/password")]
    public class PasswordController : ControllerBase
    {
        public readonly IPasswordService _passwordService;
        private readonly IHashPasswordService _hashPasswordService;
        private readonly ITokenService _tokenService;
        private readonly UserManager<ApplicationUser> _userManager;

        public PasswordController(UserManager<ApplicationUser> userManager, IPasswordService passwordService, IHashPasswordService hashPasswordService, ITokenService tokenService)
        {
            _userManager = userManager;
            _passwordService = passwordService;
            _hashPasswordService = hashPasswordService;
            _tokenService = tokenService;
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
                    return BadRequest("Password is empty !");
                }

                await _passwordService.AddPasswordAsync(user.Id, passwordDto);
                return Ok("Mot de passe ajouté avec succès.");
            }
            catch (Exception e)
            {
                throw new Exception("Une erreur s'est produite à la création du mot de passe", e);
            }
        }

        [HttpGet("{passwordId}")]
        public async Task<IActionResult> GetPasswordById(Guid passwordId)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);

                if (user == null)
                {
                    return NotFound(); // User not found or not authenticated
                }

                var item = await _passwordService.GetPasswordByUserIdAsync(user.Id, passwordId);

                return Ok(item);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception e)
            {
                throw new Exception("Une erreur s'est produite lors de la récupération du mot de passe.", e);
            }
        }

        [HttpDelete("{passwordId}")]
        public async Task<IActionResult> DeletePassword(Guid passwordId)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);

                Password password = await _passwordService.GetPasswordByUserIdAsync(user!.Id, passwordId);

                if (password == null)
                {
                    return NotFound(); 
                }

                await _passwordService.DeletePasswordAsync(user.Id, passwordId);

                return Ok("Le mot de passe à bien été supprimé");
            }
            catch (Exception e)
            {
                throw new Exception("Le produit que vous cherchez n'éxiste pas", e);
            }
        }

        [HttpPut("{passwordId}")]
        public async Task<IActionResult> UpdatePassword(Guid passwordId, [FromBody] PasswordDto passwordDto)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);

                var updatedPasswordResult = await _passwordService.UpdatePasswordAsync(passwordId, passwordDto, user.Id);

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
