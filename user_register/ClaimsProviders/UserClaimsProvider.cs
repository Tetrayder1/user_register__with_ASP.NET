using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using user_register.repository.Models;

namespace user_register.UserClaimsProviders
{
    public class UserClaimsProvider : IClaimsTransformation
    {
        private readonly UserManager<AppUser> _userManager;

        public UserClaimsProvider(UserManager<AppUser> userManager, SignInManager<AppUser> s)
        {
            _userManager = userManager;

        }

        public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            var identityName = principal.Identity as ClaimsIdentity;
            var currentUser = await _userManager.FindByNameAsync(identityName!.Name!);
            if (currentUser is null) return principal;

            if (currentUser!.City == null) return principal;

            if (!principal.HasClaim(x => x.Type == "city"))
            {
                Claim cityClaim = new Claim("city", currentUser.City);

                identityName.AddClaim(cityClaim);
            }
            //if (!principal.HasClaim(x => x.Type == "birthday"))
            //{
            //    Claim birthday = new ("birthday",currentUser.BirthDate.ToString());
            //    identityName.AddClaim(birthday);
            //}

            return principal;
        }
    }
}
