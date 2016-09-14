using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace _7days2mod_core.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            string access_token = HttpContext.Session.GetString("access_token");
            

            ViewBag.Body = access_token;
            return View();
        }

    }
}
