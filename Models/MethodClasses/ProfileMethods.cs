using System.Collections.Generic;
using System.Data;
using System.Text;
using System;

namespace Projekt3.Models
{
	public static class ProfileMethods
	{
		/// <summary>
		/// Gets one profile from the database, provided the ID of the profile.
		/// </summary>
		/// <param name="ProfileID"> The ID of the profile to get.</param>
		/// <returns>The ProfileModel of the profile.</returns>
		public static ProfileModel SelectOne(int ProfileID)
		{
			DataSet ds = DBMethods.ExecQuery(
				"SELECT * FROM dbo.Tbl_Profile " +
				"WHERE Pr_Id = " + ProfileID + ";");

			return SetFields(ds);
		}

		public static ProfileModel SelectOne(string username)
		{
			DataSet ds = DBMethods.ExecQuery(
				"SELECT * FROM dbo.Tbl_Profile " + "WHERE Pr_Username = '" + username + "'" );
				Console.WriteLine("SELECT * FROM dbo.Tbl_Profile " + "WHERE Pr_Username = '" + username + "'" );
			return SetFields(ds);
		}
		/// <summary>
		/// Selects profiles.
		/// </summary>
		/// <param name="query">The value to select profiles by.</param>
		/// <param name="field">
		/// The corresponding field to to select profiles by.
		/// Allowed values are: "Age", "Sex" and "Pref".
		/// </param>
		/// <returns>A list of profiles matching the given query. Returns an empty list if there are no matches.</returns>
		/// <exception cref="System.ArgumentException">
		/// If a non-allowed value is given to parameter field an ArgumentException will be thrown.
		/// </exception>
		public static List<ProfileModel> SelectMany(string query, string field)
		{
			// Check if the field to select by is implemented.
			if(field != "Age" || field != "Sex"|| field != "Pref")
			{
				throw new System.ArgumentException(
						"Allowed types are Age, Sex or Pref. " +
						"The field you want to select by may not be implemented yet, " +
						"in that case, ask Christoffer to implment it.");
			}

			// Generate the SQL-string.
			string SQLString = 
				"SELECT * FROM dbo.Tbl_Profile " +
				"WHERE Pr_" + field + "=" + (field == "Age" ? query : "'" + query + "'") + ";";

			// Execute the SQL string.
			DataSet ds = DBMethods.ExecQuery(SQLString.ToString());

			// Move the results from the DataSet to a list of profiles.
			List<ProfileModel> results = new List<ProfileModel>();
			for(int i = 0; i < ds.Tables[0].Rows.Count; i++)
			{
				results.Add(SetFields(ds));
			}
			return results;
		}

		/// <summary>
		/// Insets a profileModel into the table Tbl_Profile.
		/// </summary>
		/// <param name="pm"> The profile to be inserted.</param>
		/// <returns>Returns true if successful, false otherwise.</returns>
		public static bool Insert(ProfileModel pm)
		{
			Console.WriteLine("INSERT INTO dbo.Tbl_Profile " +
				"VALUES (" + GetString(pm) + ");");
			
			int result = DBMethods.ExecCommand(
				"INSERT INTO dbo.Tbl_Profile " +
				"(Pr_Firstname, Pr_Lastname, Pr_Age, Pr_Sex, Pr_Pref, Pr_Country, Pr_Username, Pr_Password, Pr_Salt, Pr_Pic, Pr_Desc, Pr_Email) " +
				"VALUES (" + GetString(pm) + ");");
			return result > 0;
		}

		/// <summary>
		/// Removes a profile from the database, given its ID.
		///	NOTE: Only executes a delete on given profile.
		///		  May cause a database exception if profile is associated with other tables.
		/// </summary>
		/// <param name="ID"> The id of the profile to be removed.</param>
		/// <returns>True if successful, false otherwise.</returns>
		public static bool Delete(int ID)
		{
			int result = DBMethods.ExecCommand(
				"DELETE FROM dbo.Tbl_Profile " +
				"WHERE Pr_Id = " + ID + ");");
			return result == 1;
		}


		/// <summary>
		/// Finds the corresponding profile in the database (with the given ProfileModel's ID),
		/// and replaces the values in the DB with the values in the given ProfileModel.
		/// NOTE: Probably does not work... I butchered the SQL-string given to ExecCommand.
		/// </summary>
		/// <param name="pm"> The ProfileModel </param>
		/// <returns>True if successful, otherwise false.</returns>
		public static bool Update(ProfileModel pm)
		{
			int result = DBMethods.ExecCommand(
				"UPDATE dbo.Tbl_Profile " +
				"SET VALUES(" + GetString(pm) + ") " +
				"WHERE Pr_Id=" + pm.ID + ";");
			return result == 1;
		}

		/// <summary>
		/// Creates a ProfileModel given a DataRow containing profile data.
		/// NOTE: The values must be in order in the DataRow, which is the case
		///		  when retrived from DB.
		/// </summary>
		/// <param name="ds">DataTable!</param>
		/// <returns>The ProfileModel, now populated with the data from the DataRow.</returns>
		private static ProfileModel SetFields(DataSet ds)
		{
			return new ProfileModel(ds.Tables["data"].Rows[0]);
		}


		/// <summary>
		/// Generates a string representation of the object.
		/// All values except ID held in the ProfileModel are put into a string, comma delimited.
		/// Eg. "Tobias, Bergström, 23, [etc...]".
		/// </summary>
		/// <param name="pm">The ProfileModel which to generate a string from.</param>
		/// <returns> A string representation of the given ProfileModel.</returns>
		private static string GetString(ProfileModel pm)
		{
			return "'" + pm.Firstname + "' ," +
				   "'" + pm.Lastname + "'," +
				   pm.Age.ToString()+ ","+
				   "'" + pm.Sex+"',"+
				   "'" + pm.SexualPreference+"',"+
				   pm.Country.ToString()+","+
				   "'" + pm.Username+"',"+
				   "'" + pm.Password+"',"+
				   "'" + pm.Salt + "',"+
				   "'" + pm.ProfilePicture+"',"+
				   "'" + pm.Description+"',"+
				   "'" + pm.Email + "'";
		}
	}
}
