using Microsoft.AspNetCore.Identity;

namespace GiftOfTheGivers.Data
{
    public static class DbInitializer
    {
        public static async Task SeedRolesAndUsersAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

            // 1. Define Roles
            string[] roleNames = { "Employee", "Donor" };

            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            // 2. Seed Employee Account
            var employeeEmail = "employee@giftofthegivers.org";
            var employeeUser = await userManager.FindByEmailAsync(employeeEmail);

            if (employeeUser == null)
            {
                var newEmployee = new IdentityUser
                {
                    UserName = employeeEmail,
                    Email = employeeEmail,
                    EmailConfirmed = true
                };

                // Default password for testing
                var createStatus = await userManager.CreateAsync(newEmployee, "Employee@123");
                if (createStatus.Succeeded)
                {
                    await userManager.AddToRoleAsync(newEmployee, "Employee");
                }
            }

            // 3. Seed Donor Account
            var donorEmail = "donor@gmail.com";
            var donorUser = await userManager.FindByEmailAsync(donorEmail);

            if (donorUser == null)
            {
                var newDonor = new IdentityUser
                {
                    UserName = donorEmail,
                    Email = donorEmail,
                    EmailConfirmed = true
                };

                var createStatus = await userManager.CreateAsync(newDonor, "Donor@123");
                if (createStatus.Succeeded)
                {
                    await userManager.AddToRoleAsync(newDonor, "Donor");
                }
            }
        }
    }
}