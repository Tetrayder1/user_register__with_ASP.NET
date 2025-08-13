using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using user_register.Areas.Admin.Models;
using user_register.repository.Models;

namespace user_register.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class HomeController : Controller
    {

        private readonly UserManager<AppUser> _userManager;
        public HomeController(UserManager<AppUser> user)
        {
            _userManager = user;   
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> UserList()
        {
         var userList=  await _userManager.Users.ToListAsync();

            var users = userList.Select(t=>new UserViewModel() {
            Name=t.UserName,
            Email=t.Email,
            Id=t.Id,
            }).ToList();

            return View(users);
        }
    }
}
