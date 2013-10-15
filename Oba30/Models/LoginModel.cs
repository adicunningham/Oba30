using System.ComponentModel.DataAnnotations;

namespace Oba30.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "User name is required")]
        public string UserName { get; set; }
        
        [Required(ErrorMessage = "Password is requried")]
        [Display(Name = "Password (*)")]
        public string Password { get; set; }
    }
}