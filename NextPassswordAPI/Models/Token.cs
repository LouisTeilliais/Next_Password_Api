    using NextPassswordAPI.Models.Interfaces;
    using System.Reflection.Metadata;

    namespace NextPassswordAPI.Models
    {
        public class Token : IToken
        {
            public Guid? Id { get; set; }

            public string TokenValue { get; set; }

            public Guid? PasswordId { get; set; }
            public Password Password { get; set; }

            /*public int? numberUses { get; set; }*/


            public DateTime? CreatedAt { get; set; }
            public DateTime? UpdatedAt { get; set; }

            public Token()
            {
                CreatedAt = DateTime.Now;
                UpdatedAt = DateTime.Now;
            }
    }
    }
