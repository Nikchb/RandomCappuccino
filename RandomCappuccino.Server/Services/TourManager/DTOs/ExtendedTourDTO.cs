using System.ComponentModel.DataAnnotations;

namespace RandomCappuccino.Server.Services.TourManager.DTOs
{
    public class ExtendedTourDTO : TourDTO
    {
        [Required]
        public TourPairDTO[] Pairs { get; set; }
    }
}
