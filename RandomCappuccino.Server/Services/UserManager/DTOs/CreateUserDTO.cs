using System.ComponentModel.DataAnnotations;

namespace RandomCappuccino.Server.Services.UserManager.DTOs
{
    public class CreateUserDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [MinLength(6)]
        public string Password {  get; set; }     
    }
}
