using System.ComponentModel.DataAnnotations;

namespace EmployeeLeaveManagement.Models
{
    public class Employee
    {
        public int EmployeeId { get; set; }

        [Required(ErrorMessage = "Employee name is required")]
        [Display(Name = "Employee Name")]
        public string EmployeeName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Enter a valid email address")]
        public string Email { get; set; } = string.Empty;

        [Display(Name = "Phone Number")]
        [Phone(ErrorMessage = "Enter a valid phone number")]
        public string? Phone { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Please select a department")]
        [Display(Name = "Department")]
        public int DepartmentId { get; set; }

        [Display(Name = "Date of Joining")]
        [DataType(DataType.Date)]
        public DateTime? DateOfJoining { get; set; }

        public bool IsActive { get; set; } = true;

        // Navigation property
        public Department? Department { get; set; }
    }
}