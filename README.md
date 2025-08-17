<h1 align="center">Salam 👋 Bu ASP.NET web  tətbiqidir.</h1>
<h3 align="center">Mən bu WF tətbiqində .Net framework dən istifadə etmişəm və bunu sayəsində, codefirst ederek DB tərəfini asanlıqla həll olunub.</h3>
<h3 align="center">DB tərəfində daha etibarlı cədvəllər qurmaq  üçün Microsoft.AspNetCore.Identity.EntityFrameworkCore framework-ündən  istifadə etmişəm .</h3>
<br/>
<h4 align="left">İstifadə edilən kitabxanalar:</h4>

```c#
Microsoft.AspNetCore.Autation.Facebook
Microsoft.AspNetCore.Razor
Microsoft.AspNetCore.Authentichentication.Google
Microsoft.EntityFrameworkCore
Microsoft.EntityFrameworkCore.SqlServer
Microsoft.EntityFrameworkCore.Tools
```
<h4 align="left">Birdə nəzərinizə çatdırım ki , bu proyekt N-layer arxetekturasına əsaslanaraq yaradılmışdı.</h4>
<br/>
<h4 align="left">Gəlin əsas kodlari bir-bir gözdən keçirək:</h4>
<hr/>

<h4 align="left">İlk öncə İdentity DB -ni yaradaq.Bunu framwork özü etdiyi üçün  hazır cədvəllər(User,Role,Claim,Login və s.) var və biz bu cədvəllərin  bəzilərini(İdentityUser)  custom hala salacıq.</h4>

```c#
public class AppUser:IdentityUser
{
    public string? City { get; set; }
    public string? Picture { get; set; }
    public DateTime? BirthDate { get; set; }
    public Gender?  Gender { get; set; }

}
public class AppDbContext:IdentityDbContext<AppUser,AppRole,string>
{
    public AppDbContext(DbContextOptions<AppDbContext> options):base(options) { }
}
```
<h4 align="left">Daha sonra isə Program.cs də DB-ni işə salırıq.</h4>

```c#
builder.Services.AddDbContext<AppDbContext>(options =>
{
options.UseSqlServer(builder.Configuration.GetConnectionString("sqlconnect")).UseSqlServer(x => x.MigrationsAssembly("user_register.repository"));
});
//Burada əslində UseSqlServer-in içi boş olur , sadəcə biz N-Layer arxetekturasından istidadə edrirk deye, DB -ni başqa bir class library-dən çəkidiyimiz üçün MigrationAssembly metodundan istifadə edirik
```
<h4 align="left">Gəlin artıq istifadəçi ilə bağlı məsələlərə toxunaq.</h4>

```c#
//Bunlardan ilki SignUp -dır
 public class HomeController : Controller
 {
     private readonly UserManager<AppUser> _UserManager;
     private readonly SignInManager<AppUser> _SignInManager;
     public HomeController( UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
     {
         _UserManager = userManager;
         _SignInManager = signInManager;
     }
     public async Task<IActionResult> SignUp(SignUpViewModel request)
     {
     var identityResult = await _UserManager.CreateAsync(new AppUser() { Email = request.Email, UserName = request.UserName, PhoneNumber = request.Phone }, request.Password);

     
     if (!ClaimConfirm.Succeeded)
     {
         ModelState.AddErrorModelState(ClaimConfirm.Errors);
         return View(request);
     }
      ...

     }
        
  }
```





