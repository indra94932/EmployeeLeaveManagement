using System.ComponentModel.DataAnnotations;

namespace EmployeeLeaveManagement.Models
{
    public class LeaveApplication
    {
        public int LeaveApplicationId { get; set; }

        [Required(ErrorMessage = "Please select an employee")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select an employee")]
        [Display(Name = "Employee")]
        public int EmployeeId { get; set; }

        [Required(ErrorMessage = "Please select a leave type")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a leave type")]
        [Display(Name = "Leave Type")]
        public int LeaveTypeId { get; set; }

        [Required(ErrorMessage = "Please select from date")]
        [DataType(DataType.Date)]
        [Display(Name = "From Date")]
        public DateTime FromDate { get; set; }

        [Required(ErrorMessage = "Please select to date")]
        [DataType(DataType.Date)]
        [Display(Name = "To Date")]
        public DateTime ToDate { get; set; }

        [Display(Name = "Number of Days")]
        public int NumberOfDays { get; set; }

        [StringLength(500)]
        [Display(Name = "Reason")]
        public string? Reason { get; set; }

        public string Status { get; set; } = "Pending";

        public DateTime AppliedDate { get; set; } = DateTime.Now;

        public DateTime? ApprovedDate { get; set; }

        public int? ApprovedBy { get; set; }

        // Navigation properties
        public Employee? Employee { get; set; }

        public LeaveType? LeaveType { get; set; }
    }
}