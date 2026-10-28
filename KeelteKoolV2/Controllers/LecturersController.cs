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


    }
}
