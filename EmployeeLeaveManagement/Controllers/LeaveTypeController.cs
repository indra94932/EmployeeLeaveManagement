using EmployeeLeaveManagement.Data;
using EmployeeLeaveManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace EmployeeLeaveManagement.Controllers
{
    [Authorize(Roles = "Admin")]
    public class LeaveTypeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LeaveTypeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /LeaveType
        public async Task<IActionResult> Index()
        {
            var leaveTypes = await _context.LeaveTypes
                .ToListAsync();

            return View(leaveTypes);
        }

        // GET: /LeaveType/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: /LeaveType/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LeaveType leaveType)
        {
            if (!ModelState.IsValid)
            {
                return View(leaveType);
            }

            leaveType.IsActive = true;

            _context.LeaveTypes.Add(leaveType);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: /LeaveType/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var leaveType = await _context.LeaveTypes
                .FindAsync(id);

            if (leaveType == null)
            {
                return NotFound();
            }

            return View(leaveType);
        }

        // POST: /LeaveType/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            LeaveType leaveType)
        {
            if (id != leaveType.LeaveTypeId)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(leaveType);
            }

            _context.LeaveTypes.Update(leaveType);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: /LeaveType/Details/5
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var leaveType = await _context.LeaveTypes
                .FindAsync(id);

            if (leaveType == null)
            {
                return NotFound();
            }

            return View(leaveType);
        }

        // GET: /LeaveType/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var leaveType = await _context.LeaveTypes
                .FindAsync(id);

            if (leaveType == null)
            {
                return NotFound();
            }

            return View(leaveType);
        }

        // POST: /LeaveType/DeleteConfirmed
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var leaveType = await _context.LeaveTypes
                .FindAsync(id);

            if (leaveType == null)
            {
                return NotFound();
            }

            leaveType.IsActive = false;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}