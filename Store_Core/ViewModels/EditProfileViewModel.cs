using System.ComponentModel.DataAnnotations;

namespace Store_Core.ViewModels
{
    public class EditProfileViewModel
    {
        [Required(ErrorMessage = "الاسم الكامل مطلوب")]
        [Display(Name = "الاسم الكامل")]
        public string FullName { get; set; }

        [Display(Name = "الصورة الشخصية")]
        public IFormFile ProfilePicture { get; set; }
    }
}
