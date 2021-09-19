using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RandomCappuccino.Server.Services.UserManager.DTOs
{
    public class UserDTO
    {
        [Required]
        public string Id { get; set;  }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public IEnumerable<string> Roles {  get; set; }
    }
}
