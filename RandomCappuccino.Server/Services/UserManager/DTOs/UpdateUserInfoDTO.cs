using System.ComponentModel.DataAnnotations;

namespace RandomCappuccino.Server.Services.UserManager.DTOs
{
    public class UpdateUserInfoDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
