using Microsoft.AspNetCore.Identity;

namespace NextPassswordAPI.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<Password> Passwords { get; set; }
    }
}
