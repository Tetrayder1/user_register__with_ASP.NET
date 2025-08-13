using System.ComponentModel.DataAnnotations;

namespace user_register.core.ViewModels
{
    public class ResetPasswordViewModel
    {
        [DataType(DataType.Password)]
        [Display(Name = "New Password:")]
        [Required(ErrorMessage = "Password bos ola bilmez")]
        [MinLength(8, ErrorMessage = "Password uzuluqu yeterli deyil.")]
        [MaxLength(256, ErrorMessage = "Password uzuluqu yeterlidir.")]
        public string Password { get; set; }


        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Password eyni deyil")]
        [Display(Name = "New Password Confirm:")]
        [Required(ErrorMessage = "Password adi bos ola bilmez")]
        [MinLength(8, ErrorMessage = "Password uzuluqu yeterli deyil.")]
        [MaxLength(256, ErrorMessage = "Password uzuluqu yeterlidir.")]
        public string PasswordConfirm { get; set; }
    }
}
