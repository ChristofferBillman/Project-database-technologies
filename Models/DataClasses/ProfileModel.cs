using System.Data;

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
		public ProfileModel(DataRow values)
		{
			ID = int.Parse(values[0].ToString());
			Firstname = values[1].ToString();
			Lastname = values[2].ToString();
			Age = int.Parse(values[3].ToString());
			Sex = values[4].ToString();
			SexualPreference = values[5].ToString();
			Country = int.Parse(values[6].ToString());
			Username = values[7].ToString();
			Password = values[8].ToString();
			ProfilePicture = values[9].ToString();
			Description = values[10].ToString();
			Email = values[11].ToString();
		}
	}
}
