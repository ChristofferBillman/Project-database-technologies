using System;
namespace Projekt3.Models
{
	public static class MatchMethods
	{
		public static bool Insert(int id1, int id2){
			DateTime date = DateTime.Now;
			string dateString = date.Year + "-"+ date.Month + "-" + date.Day;

			int result = DBMethods.ExecCommand("INSERT INTO Tbl_Match " +
				"(Ma_User1, Ma_User2, Ma_Date) "+
				"VALUES ("+ id1 +","+ id2 + ",'"+ dateString +"');");
            return result == 1;
		}
	}
}
