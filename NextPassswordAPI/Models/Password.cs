using NextPassswordAPI.Models.Interfaces;

namespace NextPassswordAPI.Models
{
    public class Password : IPassword
    {
        public Guid? Id { get; set; }

        public string? Title { get; set; }
        public string? PasswordHash { get; set; }
        public string? Notes { get; set; }
        public string? Username { get; set; }
        public string? Url { get; set; }
    }
}
