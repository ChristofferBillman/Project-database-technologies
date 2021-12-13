namespace Projekt3.Models
{
	public class ProfileModel
	{
		public int ID { get; set; }
		public string Firstname { get; set; }
		public string Lastname { get; set; }
		public int Age { get; set; }
		public string Sex { get; set; }
		public string SexualPreference { get; set; }
		public int Country { get; set; }
		public string Username { get; set; }
		public string Password { get; set; }
		public string ProfilePicture { get; set;}
		public string Description { get; set; }
		public string Email { get; set; }
		public ProfileModel()
		{

		}
	}
}
