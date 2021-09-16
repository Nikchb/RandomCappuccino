using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RandomCappuccino.Server.Data.Models
{
    public class TourPair
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [ForeignKey("Tour")]
        public string TourId { get; set; }
        public Participant Tour { get; set; }

        [Required]
        [ForeignKey("Participant1")]
        public string Participant1Id { get; set; }

        public Participant Participant1 { get; set; }

        [Required]
        [ForeignKey("Participant2")]
        public string Participant2Id { get; set; }

        public Participant Participant2 { get; set; }
    }
}
