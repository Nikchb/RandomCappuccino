using System.ComponentModel.DataAnnotations;

namespace RandomCappuccino.Server.Services.TourManager.DTOs
{
    public class CreateTourDTO
    {
        [Required]
        public string GroupId { get; set; }
    }
}
