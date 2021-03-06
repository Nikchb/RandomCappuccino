using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.ComponentModel.DataAnnotations;

namespace RandomCappuccino.Server.Data.Models
{
    [Index(nameof(Email), IsUnique = true)]
    public class User
    {
        [Key]
        public string Id {  get; set; } = Guid.NewGuid().ToString();
       
        [Required]
        public string Email {  get; set; }

        [Required]
        public string Password {  get; set; }
    }
}
