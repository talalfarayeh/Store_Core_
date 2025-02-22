using System.ComponentModel.DataAnnotations;

namespace Store_Core.ViewModels
{
    public class ResetPasswordViewModel
    {
        [Required(ErrorMessage = "البريد الإلكتروني مطلوب.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "الـ Token مطلوب.")]
        public string Token { get; set; }

        [Required(ErrorMessage = "كلمة المرور الجديدة مطلوبة.")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "كلمة المرور غير متطابقة.")]
        public string ConfirmPassword { get; set; }
    }
}
