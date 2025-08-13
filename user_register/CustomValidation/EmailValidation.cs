using Microsoft.AspNetCore.Identity;
using user_register.repository.Models;

namespace user_register.CustomValidation
{
    public class EmailValidation : IUserValidator<AppUser>
    {
        public Task<IdentityResult> ValidateAsync(UserManager<AppUser> manager, AppUser user)
        {
           var errors= new List<IdentityError>();
            if (!(user.Email.Contains("@gmail.com") || user.Email.Contains("@outlook.com")))
            {
                errors.Add(new IdentityError() {Code="NoGmailContainInEmail",Description="Email unvanini tam sekilde yazin!" });
            }

            if (errors.Any()) {
                return Task.FromResult(IdentityResult.Failed(errors.ToArray()));
            }

            return Task.FromResult(IdentityResult.Success);

        }
    }
}
