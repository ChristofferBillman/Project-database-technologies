using System.Data;

namespace Projekt3.Models.MethodClasses
{
	public static class HobbyMethods
	{

		/// <summary>
		/// Returns one specific Hobby based on ID.
		/// </summary>
		/// <param HobbyID> The ID of the requested Hobby.</param>
		/// <returns>Returns the selected item.</returns>
		public static HobbyModel SelectOne(int HobbyID)
        {
			DataSet ds = DBMethods.ExecQuery(
				"SELECT * FROM dbo.Tbl_Hobby " +
				"WHERE Ho_Id = " + HobbyID + ";");

			return SetFields(ds);
        }

		/// <summary>
		/// Takes a dataset and inserts it into fields into a HobbyModel
		/// </summary>
		/// <param ds> The dataset that should be stored in the HobbyModel.</param>
		/// <returns> Returns the new HobbyModel.</returns>
		private static HobbyModel SetFields(DataSet ds)
		{
			return new HobbyModel(ds.Tables["data"].Rows[0]);
		}

		/// <summary>
		/// Inserts new information inside the database and tells us wether or not it went through.
		/// </summary>
		/// <param cm> The HobbyModel that is to be inserted into the database.</param>
		/// <returns> Returns true if the insert went through and false if it did not.</returns>
		public static bool Insert(HobbyModel hm)
		{
			int result = DBMethods.ExecCommand("INSERT INTO dbo.Tbl_Hobby VALUES (" + ConvertToString(hm) + ");");
			return result == 1;
		}

		/// <summary>
		/// Turns a HobbyModel into a string that can be used for inserts in SQL Queries like Insert, Update, etc.
		/// </summary>
		/// <param cm> The HobbyModel that is to be turned into string.</param>
		/// <returns> Returns the new string.</returns>
		private static string ConvertToString(HobbyModel hm)
		{
			return hm.Name + "," + hm.Emoji + ",";
		}

	}
}
