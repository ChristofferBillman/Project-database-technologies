using System.Data;

namespace Projekt3.Models
{
	public static class ProfileMethods
	{
		public static ProfileModel SelectOne(int ProfileID)
		{
			DataSet ds = DBMethods.ExecQuery(
				"SELECT * FROM Profile " +
				"WHERE ID = " + ProfileID + ";");

			return SetFields(ds);
		}

		private static ProfileModel SetFields(DataSet ds)
		{
			ProfileModel pm = new ProfileModel();
			pm.ID = int.Parse(ds.Tables["data"].Rows[0]["ID"].ToString());
			return pm;
		}
	}
}
