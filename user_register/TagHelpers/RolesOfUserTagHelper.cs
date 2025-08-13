using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Text;
using user_register.repository.Models;

namespace user_register.TagHelpers
{
    public class RolesOfUserTagHelper : TagHelper
    {
        private readonly UserManager<AppUser> _userManager;
        public string  UserId { get; set; }

        public RolesOfUserTagHelper(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var user = await _userManager.FindByIdAsync(UserId);
             var roleUser=await _userManager.GetRolesAsync(user!);
          
            StringBuilder stringBuilder = new StringBuilder();

            roleUser.ToList().ForEach(x =>
            {
                stringBuilder.Append(@$"<span class='badge text-bg-secondary mx-1'>{x.ToLower()}</span>");
            });
            
            output.Content.SetHtmlContent(stringBuilder.ToString());
        }
    }
}
