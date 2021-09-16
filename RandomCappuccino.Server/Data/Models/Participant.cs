using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RandomCappuccino.Server.Data.Models
{
    public class Participant
    {
        [Key]
        public string Id {  get; set; } = Guid.NewGuid().ToString();

        [Required]
        [ForeignKey("User")]
        public string UserId { get; set; }

        public User User { get; set; }

        public string Name { get; set; }
    }
}
