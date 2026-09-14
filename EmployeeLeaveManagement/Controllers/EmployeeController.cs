using EmployeeLeaveManagement.Data;
using EmployeeLeaveManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeeLeaveManagement.Controllers
{
    [Authorize(Roles = "Admin")]
    public class EmployeeController : Controller
    
    {
        private readonly ApplicationDbContext _context;

        public EmployeeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Employee
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var employees = await _context.Employees
                .Include(e => e.Department)
                .ToListAsync();

            return View(employees);
        }

        // GET: /Employee/Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await LoadDepartments();

            return View();
        }

        // POST: /Employee/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Employee employee)
        {
            if (!ModelState.IsValid)
            {
                await LoadDepartments();

                return View(employee);
            }

            employee.IsActive = true;

            _context.Employees.Add(employee);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: /Employee/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var employee = await _context.Employees
                .FindAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            await LoadDepartments();

            return View(employee);
        }

        // POST: /Employee/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            Employee employee)
        {
            if (id != employee.EmployeeId)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                await LoadDepartments();

                return View(employee);
            }

            var existingEmployee = await _context.Employees
                .FindAsync(id);

            if (existingEmployee == null)
            {
                return NotFound();
            }

            existingEmployee.EmployeeName = employee.EmployeeName;
            existingEmployee.Email = employee.Email;
            existingEmployee.Phone = employee.Phone;
            existingEmployee.DepartmentId = employee.DepartmentId;
            existingEmployee.DateOfJoining = employee.DateOfJoining;
            existingEmployee.IsActive = employee.IsActive;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: /Employee/Details/5
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var employee = await _context.Employees
                .Include(e => e.Department)
                .FirstOrDefaultAsync(
                    e => e.EmployeeId == id);

            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // GET: /Employee/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var employee = await _context.Employees
                .Include(e => e.Department)
                .FirstOrDefaultAsync(
                    e => e.EmployeeId == id);

            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: /Employee/DeleteConfirmed
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employee = await _context.Employees
                .FindAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            employee.IsActive = false;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private async Task LoadDepartments()
        {
            ViewBag.Departments = await _context.Departments
                .Where(d => d.IsActive)
                .OrderBy(d => d.DepartmentName)
                .ToListAsync();
        }
    }
}