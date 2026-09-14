using EmployeeLeaveManagement.Data;
using EmployeeLeaveManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeeLeaveManagement.Controllers
{
    [Authorize]
    public class LeaveController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public LeaveController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // =========================
        // LEAVE LIST
        // =========================
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var query = _context.LeaveApplications
                .Include(l => l.Employee)
                .Include(l => l.LeaveType)
                .AsQueryable();

            // Employee can see only their own leaves
            if (User.IsInRole("Employee"))
            {
                var user = await _userManager.GetUserAsync(User);

                if (user?.EmployeeId == null)
                {
                    return Unauthorized();
                }

                query = query.Where(
                    l => l.EmployeeId == user.EmployeeId.Value);
            }

            // Admin can see all leaves
            var leaves = await query
                .OrderByDescending(l => l.AppliedDate)
                .ToListAsync();

            return View(leaves);
        }

        // =========================
        // APPLY LEAVE - GET
        // =========================
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await LoadDropdowns();

            return View();
        }

        // =========================
        // APPLY LEAVE - POST
        // =========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            LeaveApplication leaveApplication)
        {
            // -------------------------
            // Employee Login
            // -------------------------
            if (User.IsInRole("Employee"))
            {
                var user = await _userManager.GetUserAsync(User);

                if (user?.EmployeeId == null)
                {
                    return Unauthorized();
                }

                // Set employee from logged-in user
                leaveApplication.EmployeeId =
                    user.EmployeeId.Value;

                // Remove old validation result
                // because EmployeeId was not submitted
                // by the employee form.
                ModelState.Remove(
                    nameof(LeaveApplication.EmployeeId));
            }

            // -------------------------
            // Validate Leave Type
            // -------------------------
            if (leaveApplication.LeaveTypeId <= 0)
            {
                ModelState.AddModelError(
                    nameof(LeaveApplication.LeaveTypeId),
                    "Please select a leave type.");
            }

            // -------------------------
            // Validate Dates
            // -------------------------
            if (leaveApplication.ToDate <
                leaveApplication.FromDate)
            {
                ModelState.AddModelError(
                    nameof(LeaveApplication.ToDate),
                    "To Date cannot be earlier than From Date.");
            }

            // -------------------------
            // Calculate Number of Days
            // -------------------------
            if (leaveApplication.ToDate >=
                leaveApplication.FromDate)
            {
                leaveApplication.NumberOfDays =
                    (leaveApplication.ToDate -
                     leaveApplication.FromDate).Days + 1;
            }

            // -------------------------
            // Check Validation
            // -------------------------
            if (!ModelState.IsValid)
            {
                await LoadDropdowns();

                return View(leaveApplication);
            }

            // -------------------------
            // Set Default Values
            // -------------------------
            leaveApplication.Status = "Pending";
            leaveApplication.AppliedDate = DateTime.Now;

            // -------------------------
            // Save Leave
            // -------------------------
            _context.LeaveApplications.Add(
                leaveApplication);

            await _context.SaveChangesAsync();

            // -------------------------
            // Redirect to My Leaves
            // -------------------------
            return RedirectToAction(nameof(Index));
        }

        // =========================
        // LEAVE DETAILS
        // =========================
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var leaveApplication =
                await _context.LeaveApplications
                    .Include(l => l.Employee)
                    .Include(l => l.LeaveType)
                    .FirstOrDefaultAsync(
                        l => l.LeaveApplicationId == id);

            if (leaveApplication == null)
            {
                return NotFound();
            }

            // Employee can only view own leave
            if (User.IsInRole("Employee"))
            {
                var user = await _userManager.GetUserAsync(User);

                if (user?.EmployeeId == null ||
                    leaveApplication.EmployeeId !=
                    user.EmployeeId.Value)
                {
                    return Forbid();
                }
            }

            return View(leaveApplication);
        }

        // =========================
        // LOAD DROPDOWNS
        // =========================
        private async Task LoadDropdowns()
        {
            ViewBag.LeaveTypes = await _context.LeaveTypes
                .Where(l => l.IsActive)
                .OrderBy(l => l.LeaveTypeName)
                .ToListAsync();

            // Only Admin needs employee dropdown
            if (User.IsInRole("Admin"))
            {
                ViewBag.Employees = await _context.Employees
                    .Where(e => e.IsActive)
                    .OrderBy(e => e.EmployeeName)
                    .ToListAsync();
            }
        }
    }
}