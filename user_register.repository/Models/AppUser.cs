using Microsoft.AspNetCore.Identity;
using user_register.core.OptionsModels;

namespace user_register.repository.Models
{
    public class AppUser:IdentityUser
    {
        public string? City { get; set; }
        public string? Picture { get; set; }
        public DateTime? BirthDate { get; set; }
        public Gender?  Gender { get; set; }

    }
}
