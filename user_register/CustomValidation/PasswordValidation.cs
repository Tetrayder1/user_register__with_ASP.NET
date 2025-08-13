using Microsoft.AspNetCore.Identity;
using user_register.repository.Models;

namespace user_register.CustomValidation
{
    public class PasswordValidation : IPasswordValidator<AppUser>
    {
        public Task<IdentityResult> ValidateAsync(UserManager<AppUser> manager, AppUser user, string? password)
        {   
            List<IdentityError> errors = new List<IdentityError>();

            if (password!.ToLower().Contains(user.UserName!.ToLower()))
            {
                errors.Add(new IdentityError() {Code="PasswordNoContainUserName",Description="Sifreye istifadeci adini daxil etmeyin " });
            }

          
            if (int.TryParse(password,out _)) {
                errors.Add(new IdentityError() {Code="PasswordOnlyDigit",Description="Az reqemden istifade ede bilmezsiniz" });
            }

            if (errors.Any())
            {
                return Task.FromResult(IdentityResult.Failed(errors.ToArray()));
            }
            return Task.FromResult(IdentityResult.Success);

        }
    }
}
