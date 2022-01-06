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
using System.Net;
using System.Net.Mail;
using System.Text;

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
		public IActionResult ForgotPassword()
		{
			return View();
		}
		public IActionResult SendEmail(string email)
		{
			//Create a new password
			ProfileModel pm = ProfileMethods.SelectEmail(email);

			Random random = new Random();

			int[] randompassword = new int[6];

			for (int i = 0; i < 6; i++)
			{
				randompassword[i] = random.Next(0, 9);
			}

			string s = String.Join("", new List<int>(randompassword).ConvertAll(i => i.ToString()).ToArray());

			pm.Password = Auth.Hash(s, pm.Salt);
			ProfileMethods.Update(pm);

			// Send email to user.
			string to = email; //To address
			string from = "projekdejting@gmail.com"; //From address    
			MailMessage message = new MailMessage(from, to);

			string mailbody = "A password reset has been requested. Your new password is: " + s;

			message.Subject = "Password reset from Dating App.";
			message.Body = mailbody;
			message.BodyEncoding = Encoding.UTF8;
			message.IsBodyHtml = true;
			SmtpClient client = new SmtpClient("smtp.gmail.com", 587); //Gmail smtp    
			NetworkCredential basicCredential1 = new
			NetworkCredential("projekdejting@gmail.com", "sten1234");
			client.EnableSsl = true;
			client.UseDefaultCredentials = false;
			client.Credentials = basicCredential1;
			try
			{
				client.Send(message);
			}

			catch (Exception ex)
			{
				throw ex;
			}

			return RedirectToAction("Login");
		}
		public IActionResult  ResetPassword()
		{
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
			List<List<SelectListItem>> dropdowns = GetFormDropdowns();

			// ViewBag to send the lists to the views for display.
			ViewBag.Sex = dropdowns[0];
			ViewBag.SexPref = dropdowns[1];
			ViewBag.Country = dropdowns[2];

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

			await RecieveFile(uploadFile, pm);

			// Insert into DB.
			bool success = ProfileMethods.Insert(pm);

			if (success){
				int id = ProfileMethods.SelectOne(form["username"]).ID;
				Response.Cookies.Append("token", id + "_" + pm.Password);
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
			string token = Request.Cookies["token"];
			int profileId = int.Parse(token.Split('_')[0]);

			ProfileModel pm = ProfileMethods.SelectOne(profileId);
			ViewBag.country = CountryMethods.SelectOne(pm.Country);
			
			ViewBag.picture = pm.ProfilePicture;
			return View(pm);
        }


		public async Task<IActionResult> DownloadFile()
		{
			string token = Request.Cookies["token"];
			int profileId = int.Parse(token.Split('_')[0]);
			ProfileModel pm = ProfileMethods.SelectOne(profileId);

			var path = Path.GetFullPath("/wwwroot" + pm.ProfilePicture);
			MemoryStream memory = new MemoryStream();
			using (FileStream stream = new FileStream(path, FileMode.Open))
			{
				await stream.CopyToAsync(memory);
			}

			string[] subs = path.Split('/');

			memory.Position = 0;
			return File(memory, "image/png", subs[2]);
		}


		[HttpGet]
		public IActionResult ProfileEdit()
        {
			string token = Request.Cookies["token"];
			int profileId = int.Parse(token.Split('_')[0]);

			List<List<SelectListItem>> dropdowns = GetFormDropdowns();

			// ViewBag to send the lists to the views for display.
			ViewBag.Sex = dropdowns[0];
			ViewBag.SexPref = dropdowns[1];
			ViewBag.CountryDropdown = dropdowns[2];

			ProfileModel pm = ProfileMethods.SelectOne(profileId);

			ViewBag.country = CountryMethods.SelectOne(pm.Country);

			ViewBag.pm = pm;

			return View();
        }

		[HttpPost]
		public IActionResult ProfileEditApply(IFormCollection form, IFormFile upload)
        {
			ProfileModel pm = new ProfileModel(form);

			int id = int.Parse(Request.Cookies["token"].Split('_')[0]);
			string salt = ProfileMethods.SelectOne(id).Salt;
			pm.ID = id;

			if (!(form["Password"] == "" || form["Password"] == (IFormCollection)null))
			{
				pm.Password = Auth.Hash(form["Password"], salt);
				pm.Salt = salt;
				ProfileMethods.Update(pm); 
			}
			else
			{
				ProfileMethods.UpdateNoPassword(pm);
			}

			//ADD FILE UPLOAD

			return RedirectToAction("Profile", "Home");
        }

		public IActionResult Explore(IFormCollection form)
		{
			if(!Auth.Authenticate(Request.Cookies["token"])){
				ViewBag.fail = "Something went wrong. Try to log out and in again to resolve the problem.";
				return View();
			}

			// Problems with this approach:
			// * User may be presented with the same profile many times.
			// * The matches table will be filled with duplicates.
			// * Poor performance. Fetches profiles one at a time.
			//   Ideally this should be handled with AJAX so that
			//   we can create a buffer of profiles on the client end.

			ProfileModel person = ProfileMethods.SelectRandom();
			ViewBag.country = CountryMethods.SelectOne(person.Country).Name;

			// If there is no form, just send a person to be liked.
			if(form.Count == 0){
				return View(person);
			}

			// If the user liked the person presented to them.
			if(form.ContainsKey("Like")){
				int id = int.Parse(Request.Cookies["token"].Split('_')[0]);
				ProfileModel user = ProfileMethods.SelectOne(id);

				bool success = MatchMethods.Insert(id,int.Parse(form["PersonId"]));

				if(!success){
					ViewBag.fail = "Could not process like. Try logging in and out and try again.";
				}
			}
			// If the user disliked the person, do nothing in DB and send them a new one.
			return View(person);
		}
		
		[HttpGet]
		public IActionResult Matches()
        {
			string token = Request.Cookies["token"];
			int profileId = int.Parse(token.Split('_')[0]);

			//HERE WE WANT TO FETCH A LIST OF MATCHES FOR THE LOGGED IN USER.

			List<ProfileModel> matches = MatchMethods.Select(profileId);

			return View(matches);
        }



		[HttpGet]
		public IActionResult Unmatch(int matchID)
        {
			string token = Request.Cookies["token"];
			int profileId = int.Parse(token.Split('_')[0]);

			ProfileMethods.RemoveMatch(profileId, matchID);

			return RedirectToAction("Matches", "Home");
        }

		public IActionResult Signout()
        {

			Response.Cookies.Delete("token");

			return RedirectToAction("Index", "Home");
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
		private List<List<SelectListItem>> GetFormDropdowns(){
			List<List<SelectListItem>> dropdowns = new List<List<SelectListItem>>();

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

			dropdowns.Add(Sexes);
			dropdowns.Add(SexualPreferences);
			dropdowns.Add(Countries);
			return dropdowns;
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

				pm.ProfilePicture = "/uploadedfiles/" + fileName;

				using (var fileSrteam = new FileStream(filePath, FileMode.Create))
				{
					await uploadFile.CopyToAsync(fileSrteam);
				}
			}
		}
	
    }
}
