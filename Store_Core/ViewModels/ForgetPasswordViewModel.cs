using System.ComponentModel.DataAnnotations;

namespace Store_Core.ViewModels
{
    public class ForgetPasswordViewModel
    {
        [Required(ErrorMessage = "البريد الإلكتروني مطلوب.")]
        [EmailAddress(ErrorMessage = "البريد الإلكتروني غير صالح.")]
        public string Email { get; set; }
    }
}
