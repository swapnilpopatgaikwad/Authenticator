using System.ComponentModel.DataAnnotations;

namespace Authenticator.DTO
{
    public class LoginDto
    {
        [Required]
        [StringLength(256)]
        public required string Username { get; set; }

        [Required]
        [StringLength(8, MinimumLength = 4, ErrorMessage = "You must specify a password between 4 and 8 characters.")]
        public string Password { get; set; }
    }
}
