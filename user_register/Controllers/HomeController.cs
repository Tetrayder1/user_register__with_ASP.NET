using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using user_register.Extensions;
using user_register.repository.Models;
using user_register.service.Services;
using user_register.core.ViewModels;
using user_register.core.OptionsModels;

namespace user_register.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly UserManager<AppUser> _UserManager;
        private readonly SignInManager<AppUser> _SignInManager;

        private readonly IEmailService _emailService;

        public HomeController(ILogger<HomeController> logger, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IEmailService emailService)
        {
            _logger = logger;
            _UserManager = userManager;
            _SignInManager = signInManager;
            _emailService = emailService;
        }

        public IActionResult Index()
        {

            //_UserManager.UpdateSecurityStampAsync(user);

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordViewModel forgetPassword)
        {
            var hasUser = await _UserManager.FindByEmailAsync(forgetPassword.Email!);
            if (hasUser is null)
            {
                ModelState.AddErrorModelState(new List<string> { " email-i doqru bir sekilde daxil edin" });
                TempData["failed"] = "Proses uqursuz oldu . Prosesi yeniden doqru bir sekilde yerine yetirn";
            }
            else
            {
                string passwordResetToken = await _UserManager.GeneratePasswordResetTokenAsync(hasUser);
                var passwordResetLink = Url.Action("ResetPassword", "Home", new { UserId = hasUser.Id, Token = passwordResetToken }, HttpContext.Request.Scheme);

                await _emailService.SendResetPasswordEmail(passwordResetLink!, forgetPassword.Email!);


                TempData["successSend"] = "Send prosesi uqurla yerine yetirildi.";
            }


            return RedirectToAction(nameof(SignIn), "Home");
        }


        public IActionResult ResetPassword(string userId, string token)
        {
            TempData["user_id"] = userId;
            TempData["token"] = token;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel resetPassword)
        {
            string? userId = TempData["user_id"]!.ToString();
            string? token = TempData["token"]!.ToString();

            if (userId == null || token == null)
            {
                ModelState.AddErrorModelState(new List<string>() { "Address or token are not right." });
            }

            var user = await _UserManager.FindByIdAsync(userId);
            if (user is null)
            {
                ModelState.AddErrorModelState(new List<string>() { "No such user has been assigned." });
            }

            var resetpasswordResult = await _UserManager.ResetPasswordAsync(user, token, resetPassword.PasswordConfirm);

            if (resetpasswordResult.Succeeded)
            {
                TempData["resetpassword"] = "Password Yenileme uqurla yerine yetirildi";
                return RedirectToAction("SignIn");
            }

            ModelState.AddErrorModelState(resetpasswordResult.Errors.Select(x => x.Description).ToList());

            return RedirectToAction("ResetPassword", new { userId = userId, token = token });
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult SignIn()
        {
            return View();
        }

        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(SignInViewModel model, string? returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Action("Index", "Member");

            var hasUser = await _UserManager.FindByEmailAsync(model.Email);

          

            if (hasUser is null)
            {

                ModelState.AddModelError(string.Empty, "Zehmet olmasa etibarli email daxil edin!");
                return View(model);
            }

            var signInResult = await _SignInManager.PasswordSignInAsync(hasUser, model.Password, model.RememberMe, true);

            if (signInResult.IsLockedOut)
            {

                ModelState.AddModelError(string.Empty, "1 deqiqe boyunca signin prosesi dayandirildi.");
                return View();
            }


            if (!signInResult.Succeeded)
            {
                ModelState.AddErrorModelState(new List<string> { "Zehmet olmasa etibarli email daxil edin!", $"{_UserManager.GetAccessFailedCountAsync(hasUser).Result}/3 uqursuz giris" });
                return View();

            }
            if (hasUser.BirthDate.HasValue)
            {
                await _SignInManager.SignInWithClaimsAsync(hasUser, model.RememberMe, new[] { new Claim("birthdate", hasUser.BirthDate.Value.ToString()) });
            }
    

            return Redirect(returnUrl!);
           


        }

        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpViewModel request)
        {

            if (!ModelState.IsValid)
            {
                return View(request);
            }

            var identityResult = await _UserManager.CreateAsync(new AppUser() { Email = request.Email, UserName = request.UserName, PhoneNumber = request.Phone }, request.Password);

            if (!identityResult.Succeeded)
            {
                ModelState.AddErrorModelState(identityResult.Errors.Select(x => x.Description).ToList());
                return View();
            }

            Claim ExchangeExpireClaim = new Claim("ExchangeExpireDate", DateTime.Now.AddDays(3).ToString());
            var user = await _UserManager.FindByNameAsync(request.UserName);
            var ClaimConfirm = await _UserManager.AddClaimAsync(user, ExchangeExpireClaim);

            if (!ClaimConfirm.Succeeded)
            {
                ModelState.AddErrorModelState(ClaimConfirm.Errors);
                return View(request);
            }

            TempData["SuccessMessage"] = "Qeydiyyat uqurla basa catdi.";
            return RedirectToAction(nameof(HomeController.SignUp));

        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(List<ErrorViewModel> errors)
        {
            return View(errors);
        }



        public IActionResult FacebookLogin(string ReturnUrl) {
            string RedirectUrl = Url.Action("Respons", "home", new { ReturnUrl=ReturnUrl});

           var properties= _SignInManager.ConfigureExternalAuthenticationProperties("Facebook", RedirectUrl);

            return new ChallengeResult("Facebook",properties);
        }

        public IActionResult GoogleLogin(string ReturnUrl) {
            string RedirectUrl = Url.Action("Respons", "home", new
            {
                ReturnUrl=ReturnUrl
            });

            var properties = _SignInManager.ConfigureExternalAuthenticationProperties("Google",RedirectUrl);

            return new ChallengeResult("Google",properties);

        }

        public  async Task<IActionResult> Respons(string ReturnUrl ="/")
        {
            ExternalLoginInfo info = await _SignInManager.GetExternalLoginInfoAsync();

            if (info == null) return RedirectToAction(nameof(SignIn));

            Microsoft.AspNetCore.Identity.SignInResult signInResult = await _SignInManager.ExternalLoginSignInAsync(info.LoginProvider,info.ProviderKey,true);

            if (signInResult.Succeeded) return Redirect(ReturnUrl);

            AppUser user= new AppUser();

            var Email = info.Principal.FindFirst(ClaimTypes.Email).Value;
            user.Email = Email;
            var Id = info.Principal.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (info.Principal.HasClaim(x => x.Type == ClaimTypes.Name)) {
                string name = info.Principal.FindFirst(ClaimTypes.Name).Value.Replace(' ', '_') + Id.Substring(0,6);
                user.UserName=name;
            }

            user.UserName = Email.Split('@')[0]+Id.Substring(0,6);
            
            var userCreateResult=await _UserManager.CreateAsync(user);
            if (!userCreateResult.Succeeded)
            {
      
                List<ErrorViewModel> errors=userCreateResult.Errors.Select(x=>new ErrorViewModel() { Code=x.Code,Description=x.Description}).ToList();
              
                return View("Error",errors);
            }   
            IdentityResult identityResult = await _UserManager.AddLoginAsync(user,info);
            if(!identityResult.Succeeded) {ModelState.AddErrorModelState(identityResult.Errors);
                return RedirectToAction(nameof(SignIn));
            }

            //await _SignInManager.SignInAsync(user,true);
            await _SignInManager.ExternalLoginSignInAsync(info.LoginProvider,info.ProviderKey,true);


            return Redirect(ReturnUrl);
        }
    }
}
