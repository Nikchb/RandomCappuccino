using System.ComponentModel.DataAnnotations;

namespace RandomCappuccino.Server.Services.SignManager.DTOs
{
    public class SignRequestDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }
    }
}
