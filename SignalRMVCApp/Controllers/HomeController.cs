using Microsoft.AspNetCore.Mvc;
using SignalRMVCApp.Models;
using System.Diagnostics;

namespace SignalRMVCApp.Controllers
{
    public class HomeController : Controller
    {

        #region Methods

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult SendAll()
        {
            return View();
        }

        public IActionResult SendUser()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        #endregion /Methods

    }
}
