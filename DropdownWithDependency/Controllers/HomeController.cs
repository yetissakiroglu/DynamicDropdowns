using DropdownWithDependency.HtmlHelpers;
using DropdownWithDependency.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;

namespace DropdownWithDependency.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var configs = new List<DropdownConfig>
            {
                new DropdownConfig
                {
                    Label = "Ýl",
                    InitialItems = new List<SelectListItem>
                    {
                        new SelectListItem { Value = "1", Text = "Ýstanbul" },
                        new SelectListItem { Value = "2", Text = "Ankara" }
                    },
                    SelectedValue = "1" // Varsayýlan Ýstanbul seçili
                },
                new DropdownConfig
                {
                    Label = "Ýlçe",
                    InitialItems = null, // Dinamik yüklenecek
                    SelectedValue = null
                },
                new DropdownConfig
                {
                    Label = "Okul",
                    InitialItems = null, // Dinamik yüklenecek
                    SelectedValue = null
                },
                new DropdownConfig
                {
                    Label = "Sýnýf",
                    InitialItems = null, // Dinamik yüklenecek
                    SelectedValue = null
                }
            };

            return View(configs);
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
