using Authenticator.Enum;
using Authenticator.Model;
using System.ComponentModel.DataAnnotations;

namespace Authenticator.DTO
{
    public class UserDto
    {
        public int UserId { get; set; }

        [Required]
        [StringLength(256)]
        public required string Username { get; set; }

        [Required]
        [StringLength(256)]
        public string Email { get; set; }

        [Required]
        [StringLength(8, MinimumLength = 4, ErrorMessage = "You must specify a password between 4 and 8 characters.")]
        public string Password { get; set; }

        public GenderType Gender { get; set; }

        [StringLength(256)]
        public string FirstName { get; set; }

        [StringLength(256)]
        public string MiddleName { get; set; }

        [StringLength(256)]
        public string LastName { get; set; }

        public long PhoneNumber { get; set; }

        [StringLength(256)]
        public string Address { get; set; }

        public User ToUser()
        {
            return new User()
            {
                Username = Username,
                Email = Email,
                FirstName = FirstName,
                MiddleName = MiddleName,
                LastName = LastName,
                PhoneNumber = PhoneNumber,
                Address = Address,
                Gender = Gender,
                PasswordHash = null,
                PasswordSalt = null,
            };
        }
    }

}
