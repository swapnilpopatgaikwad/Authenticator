using System.ComponentModel.DataAnnotations;

namespace Authenticator.Model
{
    public class Role: BaseModel
    {
        [Required]
        [StringLength(256)]
        public string Name { get; set; }

        [Required]
        [StringLength(256)]
        public string NormalizedName { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
