using BusyPop.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BusyPop.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHttpContextAccessor contxt;

        public HomeController(IHttpContextAccessor httpContextAccessor)
        {
            contxt = httpContextAccessor;
        }
    

        public IActionResult Index()
        {
            contxt.HttpContext.Session.SetString("username", "ivo");
            contxt.HttpContext.Session.SetInt32("id", 1);

            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
