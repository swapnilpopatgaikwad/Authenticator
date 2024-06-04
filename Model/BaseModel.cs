using System.ComponentModel.DataAnnotations;

namespace Authenticator.Model
{
    public abstract class BaseModel
    {
        [Key] 
        public virtual int Id { get; set; }

        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
    }
}
