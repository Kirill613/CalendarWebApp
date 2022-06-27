using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ClientMvc.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View(nameof(Index));
        }

        [Authorize]
        public IActionResult Secret()
        {
            return View(nameof(Secret));
        }

    }
}