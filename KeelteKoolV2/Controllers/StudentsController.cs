using KeelteKoolV2.ApplicationServices.Services;
using KeelteKoolV2.Core.ServiceInterface;
using KeelteKoolV2.Data;
using Microsoft.AspNetCore.Mvc;

namespace KeelteKoolV2.Controllers
{
    public class StudentsController : Controller
    {
        private readonly KeelteKoolV2Context _context;
        private readonly IStudentsServices _studentsServices;
        private readonly IFilesServices _filesServices;

        public StudentsController(
        KeelteKoolV2Context context,
        IStudentsServices studentsServices,
        IFilesServices filesServices)
        {
            _context = context;
            _studentsServices = studentsServices;
            _filesServices = filesServices;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
