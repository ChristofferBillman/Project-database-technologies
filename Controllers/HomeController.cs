using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Projekt3.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

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
			string output = null;
			if(Login == "Failure" ){ output = "Wrong Username or Password"; }

			ViewBag.Fail = output;

			return View();
        }

		[HttpPost]
		public IActionResult CheckCredentials()
        {
			

			return RedirectToAction("Login", new { Login = "Failure"} );
        }

		[HttpGet]
		public IActionResult CreateUser()
		{
			string error = "";

			List<SelectListItem> Sexes = new List<SelectListItem>();
			List<SelectListItem> SexualPreferences = new List<SelectListItem>();
			List<SelectListItem> Countries = new List<SelectListItem>();


			SexMethods sm = new SexMethods();
			SexualPreferenceMethods spm = SexualPreferenceMethods();
			CountryMethods cm = new CountryMethods();

			Sexes = sm.SexesMethod();
			SexualPreferences = spm.SexualPreferencesMethod();
			Countries = cm.CountriesMethod();

			ViewBag.Sex = Sexes;
			ViewBag.SexPref = SexualPreferences;
			ViewBag.Country = Countries;

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
