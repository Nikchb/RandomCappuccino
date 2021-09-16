using System;
using System.ComponentModel.DataAnnotations;

namespace RandomCappuccino.Server.Data.Models
{
    public class Tour
    {
        [Key]
        public string Id {  get; set; } = Guid.NewGuid().ToString();

        [Required]
        public DateTime CreationTime { get; set; }
    }
}
