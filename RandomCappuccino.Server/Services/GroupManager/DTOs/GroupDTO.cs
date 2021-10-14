using System.ComponentModel.DataAnnotations;

namespace RandomCappuccino.Server.Services.GroupManager.DTOs
{
    public class GroupDTO
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
