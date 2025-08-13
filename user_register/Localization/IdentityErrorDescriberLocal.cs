using Microsoft.AspNetCore.Identity;

namespace user_register.Localization
{
    public class IdentityErrorDescriberLocal:IdentityErrorDescriber
    {
        public override IdentityError DuplicateUserName(string userName)
        {
          return new() {Code="DuplicateUserName",Description=$"{userName} -username i daha once istifade edilib." };
        }

        public override IdentityError DuplicateEmail(string email)
        {
            return new IdentityError() {Code="DuplicateEmailName",Description=$"{email} -email i daha once istifade edilib" };
        }

        public override IdentityError PasswordTooShort(int length)
        {
            return new IdentityError() { Code="passwordlegth",Description = "Sifre simvolu  uzunluqu en azi 8 olmalidir!" };
        }
    }
}
