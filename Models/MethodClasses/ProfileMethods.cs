using System.Data;

namespace Projekt3.Models
{
	public static class ProfileMethods
	{
		public static ProfileModel SelectOne(int ProfileID)
		{
			DataSet ds = DBMethods.ExecQuery(
				"SELECT * FROM dbo.Tbl_Profile " +
				"WHERE Pr_Id = " + ProfileID + ";");

			return SetFields(ds);
		}
		/// <summary>
		/// Insets a profileModel into the table Tbl_Profile.
		/// </summary>
		/// <param name="pm"> The profile to be inserted.</param>
		/// <returns>Returns true if insert was successful, and false if unsucessful.</returns>
		public static bool Insert(ProfileModel pm)
		{
			int result = DBMethods.ExecCommand("INSERT INTO Profile VALUES (" + ConvertToString(pm) + ");");
			return result == 1;
		}

		private static ProfileModel SetFields(DataSet ds)
		{
			return new ProfileModel(ds.Tables["data"].Rows[0]);
		}
		private static string ConvertToString(ProfileModel pm)
		{
			return pm.Firstname + "," +
				   pm.Lastname + "," +
				   pm.Age.ToString()+ ","+
				   pm.Sex+","+
				   pm.SexualPreference+","+
				   pm.Country.ToString()+","+
				   pm.Username+","+
				   pm.Password+","+
				   pm.ProfilePicture+","+
				   pm.Description+","+
				   pm.Email;
		}
	}
}
