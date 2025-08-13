using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.FileProviders;
using System.Security.Claims;
using user_register.Extensions;
using user_register.repository.Models;
using user_register.core.OptionsModels;
using user_register.core.ViewModels;
using user_register.service.MemberServices;

namespace user_register.Controllers
{

    [Authorize]
    public class MemberController : Controller
    {

        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IFileProvider _fileProvider;
        private readonly IMemberSerivce _memberService;
        public MemberController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, IFileProvider fileProvider,IMemberSerivce memberSerivce)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _fileProvider = fileProvider;
            _memberService = memberSerivce;
        }

        public async Task<IActionResult> IndexAsync()
        {
            return View(await _memberService.GetUserIndexPage(User.Identity!.Name!));
        }

        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(PasswordChangeViewModel1? newpassword1)
        {
            IEnumerable<IdentityError> identityErrors = await _memberService.ChangePassword(User.Identity!.Name!, newpassword1!.Password, newpassword1.NewPassword);

            if (identityErrors.Count()!=0) {
                ModelState.AddErrorModelState(identityErrors);
            }

            return RedirectToAction("Index", "Member");
        }

        public async Task<IActionResult> Logout()
        {

             await _memberService.Logout();

            return RedirectToAction("Index", "home");

        }

        [HttpGet]
        public async Task<IActionResult> UserEdit()
        {
            var user = await _userManager.FindByNameAsync(User.Identity!.Name!);
            ViewBag.genderList = new SelectList(Enum.GetNames(typeof(Gender)));

            if (user is null)
            {
                TempData["SuccessMessage"] = "Sistem xetasi bas verid .Zehmet olmasa sehifeni yenileyin";
                return View();
            }

            UserUpdateViewModel userUpdate = new()
            {
                UserName = user!.UserName!,
                BirthDate = user!.BirthDate,
                City = user.City,
                Email = user.Email!,
                Gender = user.Gender,
                Phone = user.PhoneNumber!,

            };

            return View(userUpdate);
        }

        [HttpPost]
        public async Task<IActionResult> UserEdit(UserUpdateViewModel request)
        {
            

            ViewBag.genderList = new SelectList(Enum.GetNames(typeof(Gender)));
            if (!ModelState.IsValid)
            {
                ViewBag.genderList = new SelectList(Enum.GetNames(typeof(Gender)));
                return View(request);
            }

            (UserUpdateViewModel, IEnumerable<IdentityError>) userUpdate =
                await _memberService.UserEdit(request, User.Identity.Name);

            if (userUpdate.Item2 != null) {
                ModelState.AddErrorModelState(userUpdate.Item2);
                return View(request);
            }

            var appuser = await _userManager.FindByNameAsync(request.UserName);

            await _userManager.UpdateSecurityStampAsync(appuser!);
            await _signInManager.SignOutAsync();

            if (appuser!.BirthDate.HasValue)
            {
                await _signInManager.SignInWithClaimsAsync(appuser, true, new[]
                {
                   new Claim("birthdate",appuser.BirthDate.Value.ToString())
               });
            }
            else await _signInManager.SignInAsync(appuser, true);

            @TempData["SuccessMessage"] = "User uqurla update olundu.";

            return View(userUpdate.Item1);
        }

        public IActionResult AccessDenied() {

            ViewBag.message = "Siz bu Sehifeye kecid ede bilmezsiniz.";
        return View();
        }

        public IActionResult Claims()
        {
           var claims= HttpContext.User.Claims.Select(x=>new ClaimsTableModelView() {
              Issuer= x.Issuer,
              Type=x.Type,
              Value=x.Value,
           }
           ).ToList();
           
            return View(claims);
        }

        [Authorize(Policy = "SalyanPolicy")]
        public IActionResult EnYaxsiSeher()
        {
            return View();
        }


        [Authorize(Policy = "ExchangePolicy")]
        public IActionResult Hediyyeler()
        {
            return View();
        }


        [Authorize(Policy = "ViolencePolicy")]
        public IActionResult ViolencePage()
        {
            return View();
        }
    }
}
