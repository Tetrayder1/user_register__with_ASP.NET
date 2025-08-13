using Microsoft.AspNetCore.Identity;
using user_register.repository.Models;

namespace user_register.CustomValidation
{
    public class UserValidation : IUserValidator<AppUser>
    {
        public Task<IdentityResult> ValidateAsync(UserManager<AppUser> manager, AppUser user)
        {
           
            List<IdentityError> errors= new List<IdentityError>();

            if (int.TryParse(user.UserName[0]!.ToString(),out _))
            {
                errors.Add(new IdentityError() {Code="UserNameFirstElementDigit",Description="Ilk element reqem ola bilmez." });
            }

            if (user.UserName[0] == '.' )
            {
                errors.Add(new IdentityError() { Code = "UserNameFirstElement.", Description = "Ilk element '.' ola bilmez" });
            }

            if ( user.UserName[0] == '_')
            {
                errors.Add(new IdentityError() { Code = "UserNameFirstElement_", Description = "Ilk element  '_' ola bilmez" });
            }

            if (errors.Any())
            {
                return Task.FromResult(IdentityResult.Failed(errors.ToArray()));
            }

            return Task.FromResult(IdentityResult.Success);
        }
    }
}
