﻿using Microsoft.AspNetCore.Mvc;
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

		public IActionResult Index()
		{
			return View();
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
