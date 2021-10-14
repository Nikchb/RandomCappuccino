using System.ComponentModel.DataAnnotations;

namespace RandomCappuccino.Server.Services.TourManager.DTOs
{
    public class TourPairDTO
    {
        [Required]
        public string Participant1 { get; set; }
        [Required]
        public string Participant2 { get; set; }
    }
}
