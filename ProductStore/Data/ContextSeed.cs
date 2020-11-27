using Microsoft.AspNetCore.Identity;
using ProductStore.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductStore.Data
{
    public static class ContextSeed
    {
        public static async Task SeedRolesAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            //Seed Roles
            await roleManager.CreateAsync(new IdentityRole(Enums.Roles.SuperAdmin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Enums.Roles.Admin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Enums.Roles.Moderator.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Enums.Roles.Basic.ToString()));
        }

        public static async Task SeedSuperAdminAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            //Seed Default User
            var defaultUser = new ApplicationUser
            {
                UserName = "superadmin",
                Email = "superadmin@gmail.com",
                FirstName = "Ilya",
                LastName = "Shevchenya",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };
            if (userManager.Users.All(u => u.Id != defaultUser.Id))
            {
                await userManager.CreateAsync(defaultUser, "123Pa$$word.");
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                await userManager.AddToRoleAsync(user, Enums.Roles.Basic.ToString());
                await userManager.AddToRoleAsync(user, Enums.Roles.Moderator.ToString());
                await userManager.AddToRoleAsync(user, Enums.Roles.Admin.ToString());
                await userManager.AddToRoleAsync(user, Enums.Roles.SuperAdmin.ToString());

            }
        }
    }
}
