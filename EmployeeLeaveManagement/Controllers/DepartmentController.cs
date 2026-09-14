using EmployeeLeaveManagement.Data;
using EmployeeLeaveManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace EmployeeLeaveManagement.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DepartmentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DepartmentController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Department
        public async Task<IActionResult> Index()
        {
            var departments = await _context.Departments
                .ToListAsync();

            return View(departments);
        }

        // GET: /Department/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Department/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Department department)
        {
            if (!ModelState.IsValid)
            {
                return View(department);
            }

            department.IsActive = true;

            _context.Departments.Add(department);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: /Department/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var department = await _context.Departments
                .FindAsync(id);

            if (department == null)
            {
                return NotFound();
            }

            return View(department);
        }

        // POST: /Department/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            Department department)
        {
            if (id != department.DepartmentId)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(department);
            }

            _context.Departments.Update(department);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: /Department/Details/5
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var department = await _context.Departments
                .FindAsync(id);

            if (department == null)
            {
                return NotFound();
            }

            return View(department);
        }

        // GET: /Department/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var department = await _context.Departments
                .FindAsync(id);

            if (department == null)
            {
                return NotFound();
            }

            return View(department);
        }

        // POST: /Department/DeleteConfirmed
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var department = await _context.Departments
                .FindAsync(id);

            if (department == null)
            {
                return NotFound();
            }

            department.IsActive = false;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}