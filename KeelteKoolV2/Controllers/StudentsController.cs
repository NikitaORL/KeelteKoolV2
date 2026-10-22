using KeelteKoolV2.ApplicationServices.Services;
using KeelteKoolV2.Core.DTO;
using KeelteKoolV2.Core.ServiceInterface;
using KeelteKoolV2.Models;
using KeelteKoolV2.Models.Students;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace KeelteKoolV2.Controllers
{
    public class StudentsController : Controller
    {
        private readonly IStudentsServices _studentsService;

        public StudentsController(IStudentsServices studentsService)
        {
            _studentsService = studentsService;
        }

        // GET: Students
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var students = await _studentsService.GetAllAsync();
            return View(students);
        }

        // GET: Students/Details/5
        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var student = await _studentsService.GetByIdAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            var vm = new StudentDetailsViewModel
            {
                Id = student.Id,
                FirstName = student.FirstName,
                LastName = student.LastName,
                Email = student.Email,
                CreatedAt = student.CreatedAt
            };

            return View(vm);
        }

        // GET: Students/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Students/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(StudentCreateUpdateViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var dto = new StudentDTO
                {
                    FirstName = vm.FirstName,
                    LastName = vm.LastName,
                    Email = vm.Email,
                    CreatedAt = DateTime.Now
                };

                await _studentsService.CreateAsync(dto);
                return RedirectToAction(nameof(Index));
            }
            return View(vm);
        }

        // GET: Students/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var student = await _studentsService.GetByIdAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            var vm = new StudentCreateUpdateViewModel
            {
                Id = student.Id,
                FirstName = student.FirstName,
                LastName = student.LastName,
                Email = student.Email,
                CreatedAt = student.CreatedAt
            };

            return View(vm);
        }

        // POST: Students/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, StudentCreateUpdateViewModel vm)
        {
            if (id != vm.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var dto = new StudentDTO
                {
                    Id = vm.Id.Value,
                    FirstName = vm.FirstName,
                    LastName = vm.LastName,
                    Email = vm.Email,
                    CreatedAt = vm.CreatedAt
                };

                await _studentsService.UpdateAsync(dto);
                return RedirectToAction(nameof(Index));
            }
            return View(vm);
        }

        // GET: Students/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var student = await _studentsService.GetByIdAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            var vm = new StudentDeleteViewModel
            {
                Id = student.Id,
                FirstName = student.FirstName,
                LastName = student.LastName,
                Email = student.Email,
            };

            return View(vm);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _studentsService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}