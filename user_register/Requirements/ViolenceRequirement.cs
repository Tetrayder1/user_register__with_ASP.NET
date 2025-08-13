using Microsoft.AspNetCore.Authorization;

namespace user_register.Requirements
{
    public class ViolenceRequirement : IAuthorizationRequirement
    {
        public int YasQanuni { get; set; }
    }
    public class ViolenceRequirementHandler : AuthorizationHandler<ViolenceRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ViolenceRequirement requirement)
        {
            if (!context.User.HasClaim(x => x.Type == "birthdate"))
            {
                context.Fail();
                return Task.CompletedTask;
            }

            var today = DateTime.Now;
            var userbirthday = Convert.ToDateTime(context.User.FindFirst("birthdate").Value);
            var ferq = today.Year - userbirthday.Year;

            if (userbirthday > today.AddYears(-ferq)) ferq--;

            if (requirement.YasQanuni > ferq)
            {
                context.Fail();
                return Task.CompletedTask;
            }

            context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }
}
