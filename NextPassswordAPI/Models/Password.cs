using NextPassswordAPI.Models.Interfaces;
using System.Reflection.Metadata;

namespace NextPassswordAPI.Models
{
    public class Password : IPassword
    {
        public Guid Id { get; set; }

        public string Title { get; set; }
        public string PasswordHash { get; set; }
        public string? Notes { get; set; }
        public string? Username { get; set; }
        public string? Url { get; set; }

        // Link with Tokenid from Password
        public Guid? TokenId { get; set; }
        public Token Token { get; set; }

        // Link with UserId from Identity User
        public string? UserId { get; set; }
        public ApplicationUser? User { get; set; }

        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public Password()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

    }
}
