using System.ComponentModel.DataAnnotations;

namespace RandomCappuccino.Server.Services.SignManager.DTOs
{
    public class SignResponseDTO
    {
        [Required]
        public string Token {  get; set; }
    }
}
