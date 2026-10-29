using KeelteKoolV2.ApplicationServices.Services;
using KeelteKoolV2.Core.DTO;
using KeelteKoolV2.Core.ServiceInterface;
using KeelteKoolV2.Data;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace KeelteKoolV2.Controllers
{
    public class LecturersController : Controller
    {
        private readonly KeelteKoolV2Context _context;
        private readonly ILecturersServices _lecturersServices;
        private readonly IFilesServices _filesServices;

        public LecturersController(
            KeelteKoolV2Context context,
            ILecturersServices lecturersServices,
            IFilesServices filesServices)
        {
            _context = context;
            _lecturersServices = lecturersServices;
            _filesServices = filesServices;

        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            
            var lecturers = await _lecturersServices.GetAllAsync();
            return View(lecturers);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(LecturerDTO dto)
        {
            if (ModelState.IsValid)
            {
                await _lecturersServices.Create(dto);
                return RedirectToAction(nameof(Index));
            }
            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _lecturersServices.Delete(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var lecturer = await _lecturersServices.DetailAsync(id);
            if (lecturer == null) return NotFound();
            return View(lecturer);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(LecturerDTO dto)
        {
            if (ModelState.IsValid)
            {
                await _lecturersServices.Update(dto);
                return RedirectToAction(nameof(Index));
            }
            return View(dto);
        }


    }

}

