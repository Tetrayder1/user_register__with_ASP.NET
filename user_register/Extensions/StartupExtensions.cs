using Microsoft.AspNetCore.Identity;
using user_register.CustomValidation;
using user_register.Localization;
using user_register.repository.Models;

namespace user_register.Extensions
{
    public static class StartupExtensions
    {
        public static void AddIdentityExtension(this IServiceCollection services)
        {

            services.Configure<DataProtectionTokenProviderOptions>(opt => {
                opt.TokenLifespan = TimeSpan.FromMinutes(10);
            });

            services.ConfigureApplicationCookie(opt => {
                var cookieBuilder = new CookieBuilder();

                cookieBuilder.Name = "IdentityDersleri";
                opt.LoginPath = new PathString("/Home/SignIn");
                opt.Cookie = cookieBuilder;
                opt.AccessDeniedPath = new PathString("/Member/AccessDenied"); 
                opt.ExpireTimeSpan = TimeSpan.FromDays(30);
                opt.SlidingExpiration = true;//bu kod bize yeniden signin etdikde 30 gun vaxt verir
            });

            services.AddIdentity<AppUser, AppRole>(options => {
                //User ile baqli olanlar
                options.User.RequireUniqueEmail = true;
                options.User.AllowedUserNameCharacters = "asdfghjklqwertyuiopzxcvbnm_1234567890.QWERTYUIOPASDFGHJKLZXCVBNM";
                //password ile baqli olanlar
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 8;//min 8
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;

                //lockoutfailure optimizasiyalari
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);
                options.Lockout.MaxFailedAccessAttempts = 3;

               

            }).AddPasswordValidator<PasswordValidation>()
            .AddUserValidator<EmailValidation>()
            .AddUserValidator<UserValidation>()
            .AddErrorDescriber<IdentityErrorDescriberLocal>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();
        }
    }
}
