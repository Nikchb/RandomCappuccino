using System.ComponentModel.DataAnnotations;

namespace RandomCappuccino.Server.Services.GroupManager.DTOs
{
    public class CreateGroupDTO
    {
        [Required]
        public string Name { get; set; }
    }
}
