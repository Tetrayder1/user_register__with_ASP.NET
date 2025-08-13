using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using user_register.core.OptionsModels;
using user_register.core.ViewModels;

namespace user_register.service.MemberServices
{
    public interface IMemberSerivce
    {
        Task<UserViewModelForMember> GetUserIndexPage(string user_name);

        Task Logout();
        Task<(UserUpdateViewModel, IEnumerable<IdentityError>?)> UserEdit(UserUpdateViewModel request, string _user_name);

        Task<IEnumerable<IdentityError>> ChangePassword(string user_name,string oldpassword,string newpassword);
    }

}
