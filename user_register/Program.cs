using Microsoft.EntityFrameworkCore;
using user_register.repository.Models;
using user_register.Extensions;
using user_register.service.Services;
using user_register.core.OptionsModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication;
using user_register.UserClaimsProviders;
using user_register.Requirements;
using Microsoft.AspNetCore.Authorization;
using user_register.core.Permissions;
using user_register.Seeds;
using user_register.service.MemberServices;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(options =>
{
options.UseSqlServer(builder.Configuration.GetConnectionString("sqlconnect")).UseSqlServer(x => x.MigrationsAssembly("user_register.repository"));

});
builder.Services.AddAuthentication().AddFacebook(options =>
{
   options.AppId = builder.Configuration["Authentication:Facebook:AppId"];
   options.AppSecret = builder.Configuration["Authentication:Facebook:AppSecret"];
   options.AccessDeniedPath = new PathString("/Member/AccessDenied");
});


builder.Services.AddAuthentication().AddGoogle(options => {

    options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
    options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
    options.AccessDeniedPath = new PathString("/Member/AccessDenied");
});

builder.Services.AddSingleton<IFileProvider>(new PhysicalFileProvider(Directory.GetCurrentDirectory()));
builder.Services.AddScoped<IClaimsTransformation, UserClaimsProvider>();

builder.Services.Configure<EmailSettingsOptions>( builder.Configuration.GetSection("EmailConnection"));

builder.Services.Configure<SecurityStampValidatorOptions>(opt=>{
     opt.ValidationInterval=TimeSpan.FromMinutes(30);
});

builder.Services.AddIdentityExtension();

builder.Services.AddScoped<IEmailService,EmailService>();
builder.Services.AddScoped<IAuthorizationHandler,ExchangeExpirationRequirementHandler>();
builder.Services.AddScoped<IMemberSerivce,MemberService>();
builder.Services.AddScoped<IAuthorizationHandler, ViolenceRequirementHandler>();
builder.Services.AddAuthorization(opt => {
    opt.AddPolicy("SalyanPolicy", policy =>
    {
        policy.RequireClaim("city", "Salyan", "Baki");
        policy.RequireRole("admin");
        
    });
    opt.AddPolicy("StockPolicy", policy =>
    {
        policy.RequireClaim("Permission", Permission.AllPermission.GetPermission(Permission.Permission_Name.Stock, Permission.Proses_Name.Read));
        policy.RequireClaim("Permission", Permission.AllPermission.GetPermission(Permission.Permission_Name.Stock, Permission.Proses_Name.Update));
        policy.RequireClaim("Permission", Permission.AllPermission.GetPermission(Permission.Permission_Name.Stock, Permission.Proses_Name.Create));
    });
    opt.AddPolicy("OrderPolicy.Delete",policy =>
    {
        policy.RequireClaim("Permission", Permission.AllPermission.GetPermission(Permission.Permission_Name.Order, Permission.Proses_Name.Delete));
    });
});

builder.Services.AddAuthorization(opt => {
    opt.AddPolicy("ExchangePolicy", policy => {
        policy.AddRequirements(new ExchangeExpireRequirement());
    });

});

builder.Services.AddAuthorization(opt => {
opt.AddPolicy("ViolencePolicy",policy => {
    policy.AddRequirements(new ViolenceRequirement());
});
});



var app = builder.Build();

using (var scope=app.Services.CreateScope())
{
    var roleManager =  scope.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();

   await PermissionSeed.Seed(roleManager);
}

    // Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Home/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
     name: "areas",
     pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
