using _7days2mod_core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace _7days2mod_core.Controllers
{
    public class HomeController : Controller
    {
        private readonly IGitHubAPIService _githubAPI;

        public HomeController(IGitHubAPIService githubAPI)
        {
            _githubAPI = githubAPI;
        }

        public async Task<IActionResult> Index()
        {
            var data = await _githubAPI.requestAsync();
            if (data != null)
            {
                if (data.login != null)
                {
                    ViewBag.login = data.login;
                }
            }
            return View();
        }

    }
}
