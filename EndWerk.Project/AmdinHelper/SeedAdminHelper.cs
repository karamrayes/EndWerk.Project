
using Microsoft.AspNetCore.Identity;
using Order.Object;

namespace Order.Project.Web.AmdinHelper
{
    public class SeedAdminHelper
    {
        public static async Task SeedRolesAndAdmin(IServiceProvider service)
        {
            //Seed Roles
            var userManager = service.GetService<UserManager<User>>();
            var roleManager = service.GetService<RoleManager<IdentityRole>>();
            //add two roles
            await roleManager.CreateAsync(new IdentityRole(AdminRoles.Admin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(AdminRoles.User.ToString()));


            // creating admin
            var user = new User
            {
                UserName = "admin@gmail.com",
                Email = "admin@gmail.com",
                Address = "TestAddress",
                City = "Testcity",
                //PostalCode = "9000",
                Country = "BE",
                DateBirth = new DateTime(1997, 01, 01),
                Name = "admin",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };
            var userInDb = await userManager.FindByEmailAsync(user.Email);
            if (userInDb == null)
            {
                await userManager.CreateAsync(user, "Testing*123");
                await userManager.AddToRoleAsync(user, AdminRoles.Admin.ToString());
            }

        }

    }
}
