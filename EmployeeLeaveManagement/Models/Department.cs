using System.ComponentModel.DataAnnotations;

namespace EmployeeLeaveManagement.Models
{
    public class Department
    {
        public int DepartmentId { get; set; }

        [Required(ErrorMessage = "Department name is required")]
        [Display(Name = "Department Name")]
        public string DepartmentName { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;

        public ICollection<Employee> Employees { get; set; }
            = new List<Employee>();
    }
}