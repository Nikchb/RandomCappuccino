using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RandomCappuccino.Server.Services.UserManager.DTOs
{
    public class CreateUserDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }

        [Required]
        public IEnumerable<string> Roles { get; set; }

        public CreateUserDTO()
        {

        }

        public CreateUserDTO(string email, string password, params string[] roles)
        {
            Email = email;
            Password = password;
            Roles = roles;
        }
    }
}
