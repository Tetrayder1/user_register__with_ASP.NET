using Microsoft.AspNetCore.Identity;
using System.Data;
using System.Security.Claims;
using user_register.repository.Models;
using user_register.core.Permissions;

namespace user_register.Seeds
{
    public static class PermissionSeed
    {
       
        public static async Task Seed(RoleManager<AppRole> roleManager)
        {
            var hasBasicRole = await roleManager.FindByNameAsync("BasicRole");
            var AdvancedRole = await roleManager.FindByNameAsync("AdvancedRole");
            var AdminRole = await roleManager.FindByNameAsync("AdminRole");


            if (hasBasicRole ==null) {
                await roleManager.CreateAsync(new AppRole() { Name="BasicRole" });

                var role = await roleManager.FindByNameAsync("BasicRole");
                await AddReadRoleClaim(role!,roleManager);

            }
            if (AdvancedRole == null)
            {
                await roleManager.CreateAsync(new AppRole() { Name = "AdvancedRole" });

                var role = await roleManager.FindByNameAsync("AdvancedRole");
                 await AddCreateAndUpdateRoleClaim(role!, roleManager);
                await AddReadRoleClaim(role!, roleManager);

            }
            if (AdminRole == null)
            {
                await roleManager.CreateAsync(new AppRole() { Name = "AdminRole" });

                var role = await roleManager.FindByNameAsync("AdminRole");
                await AddDeleteRoleClaim(role, roleManager);
                await AddCreateAndUpdateRoleClaim(role, roleManager);
                await AddReadRoleClaim(role, roleManager);

            }
        }

        public static async Task AddReadRoleClaim(AppRole role,RoleManager<AppRole> roleManager)
        {
            await roleManager.AddClaimAsync(role!, new Claim("Permission", Permission.AllPermission.GetPermission(Permission.Permission_Name.Stock, Permission.Proses_Name.Read)));
            await roleManager.AddClaimAsync(role!, new Claim("Permission", Permission.AllPermission.GetPermission(Permission.Permission_Name.Catalog, Permission.Proses_Name.Read)));
            await roleManager.AddClaimAsync(role!, new Claim("Permission", Permission.AllPermission.GetPermission(Permission.Permission_Name.Order, Permission.Proses_Name.Read)));
        }

        public static async Task AddCreateAndUpdateRoleClaim(AppRole role, RoleManager<AppRole> roleManager)
        {
            await roleManager.AddClaimAsync(role!, new Claim("Permission", Permission.AllPermission.GetPermission(Permission.Permission_Name.Stock, Permission.Proses_Name.Create)));
            await roleManager.AddClaimAsync(role!, new Claim("Permission", Permission.AllPermission.GetPermission(Permission.Permission_Name.Catalog, Permission.Proses_Name.Create)));
            await roleManager.AddClaimAsync(role!, new Claim("Permission", Permission.AllPermission.GetPermission(Permission.Permission_Name.Order, Permission.Proses_Name.Create)));
            await roleManager.AddClaimAsync(role!, new Claim("Permission", Permission.AllPermission.GetPermission(Permission.Permission_Name.Stock, Permission.Proses_Name.Update)));
            await roleManager.AddClaimAsync(role!, new Claim("Permission", Permission.AllPermission.GetPermission(Permission.Permission_Name.Catalog, Permission.Proses_Name.Update)));
            await roleManager.AddClaimAsync(role!, new Claim("Permission", Permission.AllPermission.GetPermission(Permission.Permission_Name.Order, Permission.Proses_Name.Update)));
        }

        public static async Task AddDeleteRoleClaim(AppRole role, RoleManager<AppRole> roleManager)
        {
            await roleManager.AddClaimAsync(role!, new Claim("Permission", Permission.AllPermission.GetPermission(Permission.Permission_Name.Stock, Permission.Proses_Name.Delete)));
            await roleManager.AddClaimAsync(role!, new Claim("Permission", Permission.AllPermission.GetPermission(Permission.Permission_Name.Catalog, Permission.Proses_Name.Delete)));
            await roleManager.AddClaimAsync(role!, new Claim("Permission", Permission.AllPermission.GetPermission(Permission.Permission_Name.Order, Permission.Proses_Name.Delete)));
        }
    }
}
