using System.ComponentModel.DataAnnotations;

namespace RandomCappuccino.Server.Services.ParticipantManager.DTOs
{
    public class CreateParticipantDTO
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string GroupId { get; set; }
    }
}
