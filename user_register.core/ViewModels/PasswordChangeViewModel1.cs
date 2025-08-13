using System.ComponentModel.DataAnnotations;

namespace user_register.core.ViewModels
{
    public class PasswordChangeViewModel1
    {

        public PasswordChangeViewModel1(){}
        public PasswordChangeViewModel1(string password, string newpassword, string confirempassword)
        {
            Password = password;
            NewPassword = newpassword;
            PasswordConfirm = confirempassword;
        }

        [DataType(DataType.Password)]
        [Display(Name = "Old Password:")]
        [Required(ErrorMessage = "Password bos ola bilmez")]
        [MinLength(8, ErrorMessage = "Password uzuluqu yeterli deyil.")]
        [MaxLength(256, ErrorMessage = "Password uzuluqu yeterlidir.")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Yeni Password:")]
        [Required(ErrorMessage = "Password bos ola bilmez")]
        [MinLength(8, ErrorMessage = "Password uzuluqu yeterli deyil.")]
        [MaxLength(256, ErrorMessage = "Password uzuluqu yeterlidir.")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Compare(nameof(NewPassword), ErrorMessage = "Password eyni deyil")]
        [Display(Name = "Yeni Password Confirm:")]
        [Required(ErrorMessage = "Password adi bos ola bilmez")]
        public string PasswordConfirm { get; set; }
    }
}
