using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Projekt3.Models;
using System.Collections.Generic;
using System.Diagnostics;

namespace Projekt3.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;

		public HomeController(ILogger<HomeController> logger)
		{
			_logger = logger;
		}

		[HttpGet]
		public IActionResult Index()
		{
			return View();
		}

		[HttpGet]
		[HttpPost]
		public IActionResult Login(string Login)
        {
			//If login fails, this will set the output for the ViewBag.Fail to login error string.
			string output = null;
			if(Login == "Failure" ){ output = "Wrong Username or Password"; }

			ViewBag.Fail = output;

			return View();
        }

		[HttpPost]
		public IActionResult CheckCredentials()
        {
			
			//When credentials don't match, redirect back to the Login screen, triggering a failure message.
			return RedirectToAction("Login", new { Login = "Failure"} );
        }

		[HttpGet]
		public IActionResult CreateUser()
		{
			string error = "";

			// Creating lists to store the different database information to later be loaded into dropdown in view.
			List<SelectListItem> Sexes = new List<SelectListItem>();
			List<SelectListItem> SexualPreferences = new List<SelectListItem>();
			List<SelectListItem> Countries = new List<SelectListItem>();

			// Temporarily commented out this since it causes compilation errors. /Christoffer
			/*// Running methods that fetches the database information for each category.
			SexMethods sm = new SexMethods();
			SexualPreferenceMethods spm = new SexualPreferenceMethods();
			CountryMethods cm = new CountryMethods();

			// Store the fetched database information inside the lists.
			Sexes = sm.SexesMethod();
			SexualPreferences = spm.SexualPreferencesMethod();
			Countries = cm.CountriesMethod();*/

			// ViewBag to send the lists to the views for display.
			ViewBag.Sex = Sexes;
			ViewBag.SexPref = SexualPreferences;
			ViewBag.Country = Countries;

			return View();
		}

		public IActionResult UserValidation()
        {
			return View();
        }

		public IActionResult Home()
        {
			return View();
        }

		public IActionResult Profile()
        {
			return View();
        }

		public IActionResult ProfileEdit()
        {
			return View();
        }

		public IActionResult Explore()
        {
			return View();
        }

		public IActionResult Matches()
        {
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
