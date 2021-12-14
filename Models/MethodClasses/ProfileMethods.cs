using System;
using System.Data;
using System.Reflection;
using System.Text;

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
		public static bool Insert(ProfileModel pm)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("INSERT INTO Profile VALUES ( ");

			foreach (PropertyInfo prop in pm.GetType().GetProperties())
			{
				Console.WriteLine($"{prop.Name}: {prop.GetValue(pm, null)}");
			}


			DBMethods.ExecCommand(sb.ToString());
			return true;
		}

		private static ProfileModel SetFields(DataSet ds)
		{
			Console.WriteLine("Tja");
			PropertyInfo[] profileModelProperties = typeof(ProfileModel).GetProperties();

			for (int i = 0; i < profileModelProperties.Length; i++)
			{
				Console.WriteLine(profileModelProperties[i].Name);
			}

			ProfileModel pm = new ProfileModel();
			pm.ID = int.Parse(ds.Tables["data"].Rows[0]["ID"].ToString());


			return pm;
		}
	}
}
