using Microsoft.AspNetCore.Mvc;

namespace TimeZoneManager.Controllers
{
    public class SpaController : Controller
    {
        public IActionResult Index()
        {
            var file = System.IO.File.OpenRead("app/index.html");
            return File(file, "text/html");
        }
    }
}
