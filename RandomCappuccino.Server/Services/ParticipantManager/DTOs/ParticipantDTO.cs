using System.ComponentModel.DataAnnotations;

namespace RandomCappuccino.Server.Services.ParticipantManager.DTOs
{
    public class ParticipantDTO
    {
        [Required]
        public string Id { get; set; } 

        [Required]
        public string Name { get; set; }
    }
}
