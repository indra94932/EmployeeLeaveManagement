using System.ComponentModel.DataAnnotations;

namespace EmployeeLeaveManagement.Models
{
    public class LeaveType
    {
        public int LeaveTypeId { get; set; }

        [Required(ErrorMessage = "Leave type name is required")]
        [Display(Name = "Leave Type")]
        public string LeaveTypeName { get; set; } = string.Empty;

        [Required]
        [Range(1, 365)]
        [Display(Name = "Total Days")]
        public int TotalDays { get; set; }

        public bool IsActive { get; set; } = true;

        public ICollection<LeaveApplication> LeaveApplications { get; set; }
            = new List<LeaveApplication>();
    }
}