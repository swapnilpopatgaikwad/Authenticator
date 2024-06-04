using Authenticator.Enum;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Authenticator.Model
{
    [Index(nameof(Email), IsUnique = true)]
    [Index(nameof(PhoneNumber), IsUnique = true)]
    public class User : BaseModel
    {
        public Guid Guid { get; set; }
        [Required]
        [StringLength(256)]
        public required string Username { get; set; }

        [Required]
        [StringLength(256)]
        public string Email { get; set; }

        [Required]
        [MaxLength]
        public required byte[] PasswordHash { get; set; }

        [Required]
        [MaxLength]
        public required byte[] PasswordSalt { get; set; }

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

        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
