namespace Projekt3.Models
{
	public class CountryMethods
	{

		/// <summary>
		/// Returns one specific Country based on ID.
		/// </summary>
		/// <param CountryID> The ID of the requested Country.</param>
		/// <returns>Returns the selected.</returns>
		public static CountryModel SelectOne(int CountryID)
        {
			DataSet ds = DBMethods.ExecQuery(
				"SELECT * FROM dbo.Tbl_Country " +
				"WHERE Co_Id = " + CountryID + ";");

			return SetFields(ds);
        }

		/// <summary>
		/// Takes a dataset and inserts it into fields into a CountryModel
		/// </summary>
		/// <param ds> The dataset that should be stored in the CountryModel.</param>
		/// <returns> Returns the new CountryModel.</returns>
		private static CountryModel SetFields(DataSet ds)
		{
			return new CountryModel(ds.Tables["data"].Rows[0]);
		}

		/// <summary>
		/// Inserts new information inside the database and tells us wether or not it went through.
		/// </summary>
		/// <param cm> The CountryModel that is to be inserted into the database.</param>
		/// <returns> Returns true if the insert went through and false if it did not.</returns>
		public static bool Insert(CountryModel cm)
		{
			int result = DBMethods.ExecCommand("INSERT INTO dbo.Tbl_Country VALUES (" + ConvertToString(cm) + ");");
			return result == 1;
		}

		/// <summary>
		/// Turns a CountryModel into a string that can be used for inserts in SQL Queries like Insert, Update, etc.
		/// </summary>
		/// <param cm> The CountryModel that is to be turned into string.</param>
		/// <returns> Returns the new string.</returns>
		private static string ConvertToString(CountryModel cm)
		{
			return cm.Name + "," + cm.Emoji + ",";
		}


	}
}
