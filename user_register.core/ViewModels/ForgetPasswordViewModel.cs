using System.ComponentModel.DataAnnotations;

namespace user_register.core.ViewModels
{
    public class ForgetPasswordViewModel
    {
        [Display(Name = "Email")]
        [Required(ErrorMessage = "Email bos ola bilmez")]
        [EmailAddress(ErrorMessage = "Email formati dogru deyil!")]
        public string? Email { get; set; }

    }
}
