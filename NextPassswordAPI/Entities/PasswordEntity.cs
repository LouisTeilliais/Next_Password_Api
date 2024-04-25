using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using NextPassswordAPI.Models;
using System.Reflection.Metadata;

namespace NextPassswordAPI.Entities
{
    [Table("passwords")]
    public class PasswordEntity : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(name: "id")]
        public Guid Id { get; set; }

        [Column(name: "title")]
        public string? Title{ get; set; }

        [Column(name: "password")]
        public string? Password { get; set; }

        [Column(name: "notes")]
        public string? Notes { get; set; }

        [Column(name: "username")]
        public string? Username { get; set; }

        [Column(name: "url")]
        public string? Url { get; set; }

        [ForeignKey("token_id")]
        public Guid TokenId { get; set; } // Renommé TokenEntityId en TokenId
        public TokenEntity Token { get; set; } // Renommé TokenEntity en Token

        [Key, ForeignKey("UserId")]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }

        public PasswordEntity(DateTime createdAt, DateTime updatedAt) : base(createdAt, updatedAt)
        {
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
        }
        public PasswordEntity() : base(DateTime.UtcNow, DateTime.UtcNow) { }
    }
}