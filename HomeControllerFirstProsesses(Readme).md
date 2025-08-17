<h4 align="left">ForgetPassword Action -nı.</h4>

```c#
public class HomeController : Controller
{
 private readonly UserManager<AppUser> _UserManager;
 private readonly SignInManager<AppUser> _SignInManager;
 private readonly IEmailService _emailService;//Custom bir servisi , DI üsulu ilə çağırırıq.
 public HomeController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IEmailService emailService)
 {
          _UserManager = userManager;
          _SignInManager = signInManager;
          _emailService = emailService;
 }
 [HttpPost]
 public async Task<IActionResult> ForgetPassword(ForgetPasswordViewModel forgetPassword)
 {
     var hasUser = await _UserManager.FindByEmailAsync(forgetPassword.Email!);//Burada istifadəçini email-ına görə tapırıq
     if (hasUser is null)
     {
         ...
     }
     else
     {
         string passwordResetToken = await _UserManager.GeneratePasswordResetTokenAsync(hasUser);
         var passwordResetLink = Url.Action("ResetPassword", "Home", new { UserId = hasUser.Id, Token = passwordResetToken }, HttpContext.Request.Scheme);
         await _emailService.SendResetPasswordEmail(passwordResetLink!, forgetPassword.Email!);//Burada həmin servisin metodunu işə salırıq.
       ...
     }
     ...
 }
}


```
<h4 align="left"> IEmailService </h4>

```c#
 public interface IEmailService
 {
     public Task SendResetPasswordEmail(string resetEmailLink,string ToEmail);
 }
 public class EmailSettingsOptions
 {
     public string?  Host { get; set; }
     public string? Password { get; set; }
     public string? Email { get; set; } 
 }
 public class EmailService : IEmailService
 {
     private readonly EmailSettingsOptions _emailSettings;

     public EmailService(IOptions<EmailSettingsOptions> options)
     {
      //bu deməkdir ki, sən appsettings.json və ya başqa konfiqurasiya
      //  mənbəsindən (appsettings.Development.json, environment variables və s.) oxunan EmailSettingsOptions
      // adında bir class-a dəyərləri bağlayırsan və onu injection ilə alırsan.

         _emailSettings = options.Value;
     }

     public async Task SendResetPasswordEmail(string resetEmailLink, string ToEmail)
     {
         var smptClinet = new SmtpClient();

         smptClinet.Host = _emailSettings.Host!;
         smptClinet.DeliveryMethod= SmtpDeliveryMethod.Network;
         smptClinet.UseDefaultCredentials = false;
         smptClinet.Port = 587;
         smptClinet.Credentials = new NetworkCredential(_emailSettings.Email, _emailSettings.Password);
         smptClinet.EnableSsl = true;
         

         var MailMessage = new MailMessage();
         MailMessage.From = new MailAddress(_emailSettings.Email);
         MailMessage.To.Add(ToEmail);

         MailMessage.Subject = "Password change link";
         MailMessage.Body = @$"<h3>Sifreni yenilemek ucun asaqidaki linke click et</h3>
                                <p><a href='{resetEmailLink}'>Password Change Link</a></p>";
         MailMessage.IsBodyHtml=true;    

         await smptClinet.SendMailAsync(MailMessage);
       
     }
 }

```

<h4 align="left">Bu Classı işə salmaq üçün Program.cs içində AddScope dan istifadə edirik.</h4>

```c#
builder.Services.AddScoped<IEmailService,EmailService>();
//Bu bizə hər dəfəsində  IEmailService -dən implement eden hər bir  classi işə sal.
builder.Services.Configure<EmailSettingsOptions>( builder.Configuration.GetSection("EmailConnection"));
//konfiqurasia mənbəsindən məlumat çəkir
```

<h4 align="left">Konfiqurasia mənbəsi:</h4>

```js
 "EmailConnection": {
   "Host": "smtp.gmail.com",
   "Email": "xxxxxxxx@gmail.com",
   "Password": "zzzzzzzzzzz"
 }
```

<h4 align="left">Signİn Action metodu.</h4>

```c#
   [HttpPost]
   public async Task<IActionResult> SignIn(SignInViewModel model, string? returnUrl = null)
   {
       returnUrl = returnUrl ?? Url.Action("Index", "Member");
       var hasUser = await _UserManager.FindByEmailAsync(model.Email);
       if (hasUser is null){ ... }

       var signInResult = await _SignInManager.PasswordSignInAsync(hasUser, model.Password, model.RememberMe, true);

       if (signInResult.IsLockedOut)
       {
           ModelState.AddModelError(string.Empty, "1 deqiqe boyunca signin prosesi dayandirildi.");
           return View();
       }
       if (!signInResult.Succeeded){...}
       return Redirect(returnUrl!);
   }
```
