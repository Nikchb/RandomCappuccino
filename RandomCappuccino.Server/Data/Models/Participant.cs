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
        
        public string Name
        {
            get => IsActive ? PName : "Diactivated participant";
            set => PName = value;
        }

        [Required]
        public string PName { get; set; }

        [Required]
        public bool IsActive { get; set; } = true;
    }
}
