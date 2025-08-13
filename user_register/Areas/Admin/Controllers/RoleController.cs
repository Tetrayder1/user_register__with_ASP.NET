using AspNetCoreGeneratedDocument;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using user_register.Areas.Admin.Models;
using user_register.Extensions;
using user_register.repository.Models;

namespace user_register.Areas.Admin.Controllers
{

    [Area("Admin")]
    [Authorize(Roles ="admin")]
    public class RoleController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly SignInManager<AppUser> _signManager;
        public RoleController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, SignInManager<AppUser> signManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _signManager = signManager;
        }
        [Authorize(Roles = "role-manager")]
        public async Task<IActionResult> RoleIndex()
        {
            List<RoleListDTO> rolelist = await _roleManager.Roles.Select(x => new RoleListDTO() {
                Name = x.Name,
                Id = x.Id
            }).ToListAsync();


            return View(rolelist);
        }
        [Authorize(Roles = "role-manager")]
        public IActionResult RoleCreate()
        {
            return View();
        }
        [Authorize(Roles = "role-manager")]
        [HttpPost]
        public async Task<IActionResult> RoleCreate(RoleCreateModelDTO roleCreate)
        {
            var result = await _roleManager.CreateAsync(new AppRole() { Name = roleCreate.Name });

            if (!result.Succeeded)
            {
                ModelState.AddErrorModelState(result.Errors);
                return View();
            }

            TempData["role"] = "Rol uqurla qeyd edildi.";
            return RedirectToAction(nameof(RoleIndex), "Role");
        }

        [Authorize(Roles = "role-manager")]

        public async Task<IActionResult> RoleUpdate(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            

            return View(new UpdateRoleDTO() { Id=role.Id,Name=role.Name});
        }
        [Authorize(Roles = "role-manager")]
        [HttpPost]
        public async Task<IActionResult> RoleUpdate(UpdateRoleDTO updateRole)
        {
            var hasRole=await _roleManager.FindByIdAsync(updateRole.Id);
            if (hasRole !=null) {
                TempData["role"] = "Bu adda artiq role var.";
                return View();
            }

            var role = await _roleManager.FindByIdAsync(updateRole.Id);
            role!.Name = updateRole.Name;

            var result= await _roleManager.UpdateAsync(role);

            if (!result.Succeeded) {
                ModelState.AddErrorModelState(result.Errors);
                return View();
            }
            
           

            TempData["role"] = "Role uqurla yenilendi.";

            return RedirectToAction(nameof(RoleIndex), "Role");
        }
        [Authorize(Roles = "role-manager")]
        public async Task <IActionResult> DeleteRole(string id) {

            var  role=await _roleManager.FindByIdAsync(id);
            var result = await _roleManager.DeleteAsync(role);
            if (!result.Succeeded) {
                ModelState.AddErrorModelState(result.Errors);
            }

            TempData["role"] = "Role uqurla silindi.";
            return RedirectToAction( nameof(RoleIndex),"Role");
        }

        public  async Task<IActionResult> AppointRoleToUser(string id)
        {
            ViewBag.Id = id;

            var currentUser=await _userManager.FindByIdAsync(id);

            var roles = await _roleManager.Roles.ToListAsync();

           List< AppointRoleToUserDTO> appointRoles= new List<AppointRoleToUserDTO> ();

            var userRole=await _userManager.GetRolesAsync(currentUser);

            foreach (var role in roles) {

                AppointRoleToUserDTO appointRoleToUserDTO = new AppointRoleToUserDTO() {
                    Name = role.Name!,
                    Id = role.Id
                };
                if (userRole.Contains(role.Name!))
                {
                    appointRoleToUserDTO.Exist = true;
                }

                appointRoles.Add(appointRoleToUserDTO);
            }

            return View(appointRoles);
        }

        [HttpPost]
        public async Task<IActionResult> AppointRoleToUser(string id,List<AppointRoleToUserDTO> appointRoles)
        {
            var currentUser = await _userManager.FindByIdAsync(id);

            foreach (var role in appointRoles)
            {
                if(role.Exist) await _userManager.AddToRoleAsync(currentUser!,role.Name);
                else await _userManager.RemoveFromRoleAsync(currentUser!,role.Name);
            }
            //string name1 =HttpContext.User.Identity.Name;
            string? name = User.Identity!.Name;
            var user = await _userManager.FindByNameAsync(name!);

            await _signManager.SignOutAsync();
            await _signManager.SignInAsync(user!, true);
            return RedirectToAction("UserList","home");
        }

        

    }
}
