using Microsoft.AspNetCore.Mvc;

namespace KeelteKoolV2.Models.Students
{
    public class StudentIndexViewModel : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
