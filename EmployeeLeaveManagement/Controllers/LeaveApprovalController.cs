using EmployeeLeaveManagement.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace EmployeeLeaveManagement.Controllers
{
    [Authorize(Roles = "Admin")]
    public class LeaveApprovalController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LeaveApprovalController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var leaves = await _context.LeaveApplications
                .Include(l => l.Employee)
                .Include(l => l.LeaveType)
                .OrderByDescending(l => l.AppliedDate)
                .ToListAsync();

            return View(leaves);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Approve(int id)
        {
            var leave = await _context.LeaveApplications
                .FindAsync(id);

            if (leave == null)
            {
                return NotFound();
            }

            leave.Status = "Approved";
            leave.ApprovedDate = DateTime.Now;
            leave.ApprovedBy = null;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reject(int id)
        {
            var leave = await _context.LeaveApplications
                .FindAsync(id);

            if (leave == null)
            {
                return NotFound();
            }

            leave.Status = "Rejected";
            leave.ApprovedDate = DateTime.Now;
            leave.ApprovedBy = null;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}