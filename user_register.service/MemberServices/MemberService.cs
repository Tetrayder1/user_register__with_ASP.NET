using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using user_register.core.OptionsModels;
using user_register.core.ViewModels;
using user_register.repository.Models;

namespace user_register.service.MemberServices
{
    public class MemberService:IMemberSerivce
    {

        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IFileProvider _fileProvider;

        public MemberService(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, IFileProvider fileProvider)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _fileProvider = fileProvider;
        }

        public async Task<IEnumerable<IdentityError?>> ChangePassword(string user_name, string oldpassword,string newpassword)
        {
            var user = await _userManager.FindByNameAsync(user_name);
            bool hasPassword = await _userManager.CheckPasswordAsync(user!, oldpassword);


            if (!hasPassword) return new List<IdentityError>() {new IdentityError() { Code = "Password", Description = "Sifre yanlisdir." }};
                      
            
            var resultPasswordChange = await _userManager.ChangePasswordAsync(user!,    oldpassword, newpassword);
            if (!resultPasswordChange.Succeeded)
            {
                return resultPasswordChange.Errors;
            }
            await _userManager.UpdateSecurityStampAsync(user!);
            await _signInManager.SignOutAsync();
            await _signInManager.PasswordSignInAsync(user!, newpassword, true, false);
            return null;
        }

        public  async Task<UserViewModelForMember> GetUserIndexPage(string user_name)
        {
            var currentUser = await _userManager.FindByNameAsync(user_name);

            var userViewModel = new UserViewModelForMember()
            {
                Email = currentUser!.Email,
                UserName = currentUser!.UserName,
                Phone = currentUser!.PhoneNumber,
                PictureUser = currentUser!.Picture

            };

            return userViewModel;
        }

        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }


        public async Task<(UserUpdateViewModel, IEnumerable<IdentityError>?)> UserEdit(UserUpdateViewModel request,string _user_name) {

            
            if (request.Gender == null)
            {
                return (request, new List<IdentityError>() { new IdentityError() { Code = string.Empty, Description = "Gender qeyd edin." } });
            }



            var currentUser = await _userManager.FindByNameAsync(_user_name);

            currentUser!.UserName = request.UserName;
            currentUser.BirthDate = request.BirthDate;
            currentUser.City = request.City;
            currentUser.Email = request.Email;
            currentUser.Gender = request.Gender;
            currentUser.PhoneNumber = request.Phone;

            if (request.PictureUser != null)
            {
                var wwwrootfolder = _fileProvider.GetDirectoryContents("wwwroot");

                var PictureFileName = $"{Guid.NewGuid().ToString()}{Path.GetExtension(request.PictureUser.FileName)}";

                var newPicturePaht = Path.Combine(wwwrootfolder.First(x => x.Name == "userPicture").PhysicalPath!, PictureFileName);

                using var stream = new FileStream(newPicturePaht, FileMode.Create);

                await request.PictureUser.CopyToAsync(stream);

                currentUser.Picture = PictureFileName;
            }

            var successUser = await _userManager.UpdateAsync(currentUser);
            if (!successUser.Succeeded)
            {
                return (request, successUser.Errors);
            }
            //var authResult = await HttpContext.AuthenticateAsync();
            //var isPersistent = authResult.Properties?.IsPersistent ?? false;


           


            UserUpdateViewModel userUpdate = new UserUpdateViewModel()
            {
                BirthDate = currentUser.BirthDate,
                City = currentUser.City,
                Email = currentUser.Email,
                Gender = currentUser.Gender,
                Phone = currentUser.PhoneNumber,
                UserName = currentUser.UserName
            };

            return (userUpdate, null);
        }
    }
}
