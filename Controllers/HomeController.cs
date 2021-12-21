using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Projekt3.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Linq;

namespace Projekt3.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;

		public HomeController( ILogger<HomeController> logger)
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
		public IActionResult Login(IFormCollection form, bool success)
		{
			
			/*if(!success){
				ViewBag.fail = "Something went wrong when creating user. Try again.";
			}*/
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

		[HttpPost]
		public async Task<IActionResult> InsertUser(IFormCollection form, IFormFile uploadFile)
		{
			// Get user
			ProfileModel pm = new ProfileModel(form);
			// Generate salt and hash for user.
			pm.Salt = Auth.GetRandomSalt();
			pm.Password = Auth.Hash(form["password"],pm.Salt);
			// Insert into DB.
			bool success = ProfileMethods.Insert(pm);

			await RecieveFile(uploadFile, pm);

			if (success){
				return RedirectToAction("Home", "Home");
			}
			return RedirectToAction("CreateUser","Home", new {success = false});
		}

		public IActionResult Home()
        {
			return View();
        }

		public IActionResult Profile()
        {
			return View();
        }

		[HttpGet]
		public IActionResult ProfileEdit()
        {
			string token = Request.Cookies["Token"];
			int profileId = int.Parse(token.Split('_')[0]);


			ProfileModel pm = ProfileMethods.SelectOne(profileId);

			ViewBag.pm = pm;

			return View();
        }

		[HttpPost]
		public IActionResult ProfileEditApply(IFormCollection form, IFormFile upload)
        {
			ProfileModel pm = new ProfileModel(form);
			ProfileMethods.Update(pm);

			//ADD FILE UPLOAD

			return RedirectToAction("Profile", "Home");
        }

		public IActionResult Explore()
        {
			return View();
        }

		public IActionResult Matches()
        {
			string token = Request.Cookies["Token"];
			int profileId = int.Parse(token.Split('_')[0]);

			//HERE WE WANT TO FETCH A LIST OF MATCHES FOR THE LOGGED IN USER.

			List<ProfileModel> matches = new List<ProfileModel>();

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
		/// <summary>
		/// Saves the provided IFormFile into the directory
		/// wwwroot/uploadedfiles and sets this file as
        /// the profile picture of the provided ProfileModel.
		/// </summary>
		/// <param name="uploadFile"></param>
		private async Task RecieveFile(IFormFile uploadFile,ProfileModel pm)
        {
			if (uploadFile != null && uploadFile.Length > 0)
			{
				// Get the type of file (png, jpeg, webp, etc...)
				string fileExtension = System.IO.Path.GetExtension(
					uploadFile.FileName);

				// The purpose of count.txt is to keep track of how many
				// images have been uploaded and to make sure no duplicate
				// file names exist. New files are namned to the next index.

				int index;
				try
				{
					// Try to read the first line of file count.txt.
					index = int.Parse(System.IO.File.ReadLines(
					"wwwroot/uploadedfiles/count.txt")
					.First());
				}
				// If the file does not exist, start index from 0.
				catch (FileNotFoundException)
				{
					index = 0;
				}

				index++;

				// Write index to count.txt. If count.txt does not
				// exist, WriteAllText creates a file and writes to it.
				System.IO.File.WriteAllText(
					"wwwroot/uploadedfiles/count.txt",
					index.ToString());

				// Set the name of the incoming file to index.
				string fileName = index.ToString() + fileExtension;
				string filePath = Path.Combine("wwwroot/uploadedfiles/", fileName);

				pm.ProfilePicture = filePath;

				using (var fileSrteam = new FileStream(filePath, FileMode.Create))
				{
					await uploadFile.CopyToAsync(fileSrteam);
				}
			}
		}
	
    }
}
