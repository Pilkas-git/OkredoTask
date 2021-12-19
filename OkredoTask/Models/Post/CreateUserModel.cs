using System.ComponentModel.DataAnnotations;

namespace OkredoTask.Web.Models
{
    public class CreateUserModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}