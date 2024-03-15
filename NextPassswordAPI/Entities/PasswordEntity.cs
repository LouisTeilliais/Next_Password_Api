using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

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

        [Column(name: "passwordHash")]
        public string? PasswordHash { get; set; }

        [Column(name: "notes")]
        public string? Notes { get; set; }

        [Column(name: "username")]
        public string? Username { get; set; }

        [Column(name: "url")]
        public string? Url { get; set; }

        public PasswordEntity(DateTime createdAt, DateTime updatedAt) : base(createdAt, updatedAt) { }
    }
}
