using EmployeeLeaveManagement.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EmployeeLeaveManagement.Data
{
    public class ApplicationDbContext
        : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; } = null!;

        public DbSet<Department> Departments { get; set; } = null!;

        public DbSet<LeaveType> LeaveTypes { get; set; } = null!;

        public DbSet<LeaveApplication> LeaveApplications
        { get; set; } = null!;
    }
}