using System.ComponentModel.DataAnnotations;

namespace Store_Core.ViewModels
{
    public class RegisterViewModel
    {
        public string Email { get; set; }

        [DataType(DataType.Password)]

        public string Password { get; set; }
        public string FullName { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "كلمتا المرور غير متطابقتين.")]
        public string ConfirmPassword { get; set; }

        
    }
}
