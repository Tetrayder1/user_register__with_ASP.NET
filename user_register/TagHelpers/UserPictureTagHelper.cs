using Microsoft.AspNetCore.Razor.TagHelpers;

namespace user_register.TagHelpers
{
    public class UserPictureTagHelper:TagHelper
    {
        public string? PictureUrl { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "img";

            if (string.IsNullOrEmpty(PictureUrl))
            {
                output.Attributes.SetAttribute("src", "/userPicture/default_userPicture.jpg");
            }
            else {
                output.Attributes.SetAttribute("src", $"/userPicture/{PictureUrl}");
            }
        }
    }
}
