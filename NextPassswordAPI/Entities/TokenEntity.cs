using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using NextPassswordAPI.Models;

namespace NextPassswordAPI.Entities
{
    [Table("passwords")]
    public class TokenEntity : BaseEntity
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(name: "token_id")]
        public Guid Id { get; set; }


        [Column(name: "token_value")]
        public string? TokenValue { get; set; }

        [Column(name: "password_id")]

        public PasswordEntity Password { get; set; }


        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }

        public TokenEntity(DateTime createdAt, DateTime updatedAt) : base(createdAt, updatedAt)
        {
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
        }
        public TokenEntity() : base(DateTime.UtcNow, DateTime.UtcNow) { }

    }
}
