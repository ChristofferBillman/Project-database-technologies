using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Projekt3.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System;

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
		public IActionResult Login(IFormCollection form)
		{
			
			ViewBag.fail = "Username: " + form["Username"] + "    Pass: " + form["Password"];
			// Do nullcheck on form before accessing values.

			Console.WriteLine("contains username: " + form.ContainsKey("Username"));
			Console.WriteLine("username is '': " + form["Username"] != "");

			if(form.ContainsKey("Username") && form.ContainsKey("Password")){
				
				// Get user.
				ProfileModel pm = ProfileMethods.SelectOne(form["Username"]);

				// If auth successful, redirect to home.
				if(Auth.Authenticate(pm,form["Password"])){
					// Append token to response as cookie.
					Response.Cookies.Append("token",pm.ID + "_" +Auth.Hash(form["Password"],pm.Salt));
					return RedirectToAction("Home","Home");
				}
				else{
					ViewBag.fail = "Wrong password or username.";
				}
				Console.WriteLine("Vi kom hit :(");
			}
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
			Sexes.Add(new SelectListItem() { Text = "Male", Value = "Male" });
			Sexes.Add(new SelectListItem() { Text = "Female", Value = "Female" });
			Sexes.Add(new SelectListItem() { Text = "Non-binary", Value = "Non-binary" });

			List<SelectListItem> SexualPreferences = new List<SelectListItem>();
			SexualPreferences.Add(new SelectListItem(){Text="Heterosexual", Value="Hetero"});
			SexualPreferences.Add(new SelectListItem(){Text="Homosexual", Value="Homo"});
			SexualPreferences.Add(new SelectListItem(){Text="Bisexual", Value="Bi"});

			List<SelectListItem> Countries = new List<SelectListItem>();
			Countries = CountryMethods.SelectAll();

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
