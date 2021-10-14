using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RandomCappuccino.Server.Data.Models
{
 
    public class UserRole
    {
        [Required]
        [ForeignKey("User")]        
        public string UserId { get; set; }

        public User User { get; set; }

        [Required]
        public string Role { get; set; }
    }
}
