using System.ComponentModel.DataAnnotations.Schema;

namespace Authenticator.Model
{
    public class UserRole : BaseModel
    {
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        public int RoleId { get; set; }

        [ForeignKey("RoleId")]
        public Role Role { get; set; }
    }
}
