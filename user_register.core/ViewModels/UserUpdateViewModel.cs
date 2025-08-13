using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using user_register.core.OptionsModels;

namespace user_register.core.ViewModels
{
    public class UserUpdateViewModel
    {
        [Required(ErrorMessage = "Istifadeci adi bos ola bilmez.")]
        [Display(Name = "User Name")]
        public string UserName { get; set; } = null!;

        [Display(Name = "Email")]
        [Required(ErrorMessage = "Email bos ola bilmez")]
        [EmailAddress(ErrorMessage = "Email formati dogru deyil!")]
        public string Email { get; set; } = null!;

        [Display(Name = "Cell")]
        [Required(ErrorMessage = "Telefon nomresi bos ola bilmez.")]
        public string Phone { get; set; } = null!;

        [Display(Name = "City")]
        public string? City { get; set; }

        [Display(Name = "Picture")]
        public IFormFile? PictureUser { get; set; }

        [Display(Name = "BirthDate")]
        [DataType(DataType.Date)]
        public DateTime? BirthDate { get; set; }

        [Display(Name = "Gender")]
        [Required(ErrorMessage = "Gender bos ola bilmez")]
        public Gender? Gender { get; set; }

    }
}
