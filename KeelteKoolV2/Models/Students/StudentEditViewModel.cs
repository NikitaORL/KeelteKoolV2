using Microsoft.AspNetCore.Mvc;

namespace KeelteKoolV2.Models.Students
{
    public class StudentEditViewModel : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
