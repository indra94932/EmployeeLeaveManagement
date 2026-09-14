using Microsoft.AspNetCore.Identity;

namespace EmployeeLeaveManagement.Models
{
    public class ApplicationUser : IdentityUser
    {
        public int? EmployeeId { get; set; }

        public Employee? Employee { get; set; }
    }
}