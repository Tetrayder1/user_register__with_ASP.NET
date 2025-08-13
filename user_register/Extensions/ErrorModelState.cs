using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace user_register.Extensions
{
    public static  class ErrorModelState
    {
        public static void AddErrorModelState(this ModelStateDictionary modelstate,List<string> errors) {

            errors.ForEach(x => {
                modelstate.AddModelError(string.Empty,x);
            });

        }

        public static void AddErrorModelState(this ModelStateDictionary modelstate, IEnumerable<IdentityError> errors)
        {

            errors.ToList().ForEach(x => {
                modelstate.AddModelError(string.Empty,x.Description);
            });

        }
    }
}
