using Microsoft.AspNetCore.Mvc;

namespace KeelteKoolV2.Models.Students
{
    public class StudentCreateViewModel : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
