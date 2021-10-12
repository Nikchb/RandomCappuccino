using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RandomCappuccino.Server.Data.Models
{
    public class Tour
    {
        [Key]
        public string Id {  get; set; } = Guid.NewGuid().ToString();

        [Required]
        public DateTime CreationTime { get; set; } = DateTime.UtcNow + (DateTime.Now - DateTime.UtcNow);

        [Required]
        [ForeignKey("Group")]
        public string GroupId { get; set; }

        public Group Group { get; set; }   
        
        public List<TourPair> Pairs { get; set; }
    }
}
