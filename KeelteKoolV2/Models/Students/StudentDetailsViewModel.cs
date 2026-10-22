using Microsoft.AspNetCore.Mvc;

namespace KeelteKoolV2.Models.Students
{
    public class StudentDetailsViewModel : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
