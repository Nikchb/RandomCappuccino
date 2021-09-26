using System;
using System.ComponentModel.DataAnnotations;

namespace RandomCappuccino.Server.Services.TourManager.DTOs
{
    public class TourDTO
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public DateTime CreationTime { get; set; }
    }
}
