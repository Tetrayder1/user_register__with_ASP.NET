using System.ComponentModel.DataAnnotations;

namespace user_register.core.ViewModels
{
    public class SignUpViewModel
    {

        public SignUpViewModel()
        {
            
        }
        public SignUpViewModel(string userName,  string email, string phone, string password,string passwordconfirm)
        {
            UserName = userName;
            Email = email;
            Password = password;
            PasswordConfirm = passwordconfirm;
            Phone = phone;
        }
        [Required(ErrorMessage ="Istifadeci adi bos ola bilmez")]
        [Display(Name="User Name:")]
        public string UserName { get; set; }

        [Display(Name = "Email:")]
        [Required(ErrorMessage = "Email bos ola bilmez")]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Password:")]
        [Required(ErrorMessage = "Password bos ola bilmez")]
        public string Password { get; set; }


        [DataType(DataType.Password)]
        [Compare(nameof(Password),ErrorMessage ="Password eyni deyil")]
        [Display(Name = "PasswordConfirm:")]
        [Required(ErrorMessage = "Password adi bos ola bilmez")]
        public string PasswordConfirm { get; set; }

        [Display(Name = "Cell:")]
        [Required(ErrorMessage = "Telefon nomresi bos ola bilmez")]
        public string Phone {  get; set; }

    }
}
