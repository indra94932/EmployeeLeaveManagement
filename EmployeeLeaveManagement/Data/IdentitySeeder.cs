using EmployeeLeaveManagement.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EmployeeLeaveManagement.Data
{
    public static class IdentitySeeder
    {
        public static async Task SeedAsync(
            IServiceProvider serviceProvider)
        {
            var roleManager =
                serviceProvider.GetRequiredService<
                    RoleManager<IdentityRole>>();

            var userManager =
                serviceProvider.GetRequiredService<
                    UserManager<ApplicationUser>>();

            var context =
                serviceProvider.GetRequiredService<
                    ApplicationDbContext>();

            // -----------------------------
            // Create Roles
            // -----------------------------

            string[] roles =
            {
                "Admin",
                "Employee"
            };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(
                        new IdentityRole(role));
                }
            }

            // -----------------------------
            // Create Admin
            // -----------------------------

            var adminUsername = "admin";

            var admin = await userManager
                .FindByNameAsync(adminUsername);

            if (admin == null)
            {
                admin = new ApplicationUser
                {
                    UserName = adminUsername,
                    Email = "admin@company.com",
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(
                    admin,
                    "Admin@123");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(
                        admin,
                        "Admin");
                }
            }

            // -----------------------------
            // Create Employee User
            // -----------------------------

            var employee = await context.Employees
                .Where(e => e.IsActive)
                .OrderBy(e => e.EmployeeId)
                .FirstOrDefaultAsync();

            if (employee != null)
            {
                var employeeUsername =
                    "employee";

                var employeeUser = await userManager
                    .FindByNameAsync(employeeUsername);

                if (employeeUser == null)
                {
                    employeeUser = new ApplicationUser
                    {
                        UserName = employeeUsername,
                        Email = employee.Email,
                        EmailConfirmed = true,
                        EmployeeId = employee.EmployeeId
                    };

                    var result =
                        await userManager.CreateAsync(
                            employeeUser,
                            "Employee@123");

                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(
                            employeeUser,
                            "Employee");
                    }
                }
            }
        }
    }
}