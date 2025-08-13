using System.ComponentModel.DataAnnotations;

namespace user_register.core.ViewModels
{
    public class SignInViewModel
    {
        public SignInViewModel() { }
        public SignInViewModel(string email, string password, bool remebeme)
        {
            Email = email;
            Password = password;
            RememberMe = remebeme;
        }
     
        [Display(Name = "Email:")]
        [Required(ErrorMessage = "Email bos ola bilmez")]
        [EmailAddress(ErrorMessage = "Email formati dogru deyil!")]
        public string Email { get; set; }

        [Display(Name = "Password:")]
        [Required(ErrorMessage = "Password bos ola bilmez")]
        public string Password { get; set; }

        [Display(Name = "Yada sal meni")]
        public bool RememberMe { get; set; }





    }
}
