using System.ComponentModel.DataAnnotations;

namespace WebApplication.Models
{
    public class UserLogins
    {
        public int Id { get; set; }
        [Required]
        public string UserName
        {
            get;
            set;
        }
        [Required]
        public string Password
        {
            get;
            set;
        }
        public UserLogins() { }
    }
}
