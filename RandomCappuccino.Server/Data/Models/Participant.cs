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
        [ForeignKey("Group")]
        public string GroupId { get; set; }

        public Group Group { get; set; }

        [Required]
        public string Name { get; set; }
        
        [Required]
        public string IsActive { get; set; }
    }
}
