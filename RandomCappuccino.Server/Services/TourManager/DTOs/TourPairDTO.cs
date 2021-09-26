using System.ComponentModel.DataAnnotations;

namespace RandomCappuccino.Server.Services.TourManager.DTOs
{
    public class TourPairDTO
    {
        [Required]
        public string Participant1Id { get; set; }
        [Required]
        public string Participant2Id { get; set; }
    }
}
