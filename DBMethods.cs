using System;
using System.Data;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;

namespace Projekt3
{
	public class DBMethods
	{
		// Update with new connstring from Linus when he is done with DB.
		static readonly string connString = Config.CONNSTRING;
		/// <summary>
		/// Executes a SQL command in the database.
		/// </summary>
		/// <param name="sqlstring"> The SQL string.</param>
		/// <returns> The number of rows affected. </returns>
		public static int ExecCommand(string sqlstring)
		{
			if(Config.USEMYSQL){
				MySqlConnection DBConnection = new MySqlConnection();
				DBConnection.ConnectionString = connString;

				MySqlCommand command = new MySqlCommand(sqlstring, DBConnection);

				try
				{
					DBConnection.Open();
					return command.ExecuteNonQuery();
				}
				catch
				{
					throw;
				}
				finally
				{
					DBConnection.Close();
				}
			}
			else
			{
				SqlConnection DBConnection = new SqlConnection();
				DBConnection.ConnectionString = connString;

				SqlCommand command = new SqlCommand(sqlstring, DBConnection);

				try
				{
					DBConnection.Open();
					return command.ExecuteNonQuery();
				}
				catch
				{
					throw;
				}
				finally
				{
					DBConnection.Close();
				}
			}
		}
		/// <summary>
		/// Executes a SQL query in the database.
		/// </summary>
		/// <param name="sqlstring"> The SQL string.  </param>
		/// <returns> The requested data. Is accessible in ds["data"]. </returns>
		public static DataSet ExecQuery(string sqlstring)
		{
			if(Config.USEMYSQL){
				MySqlConnection DBConnection = new MySqlConnection();
				DBConnection.ConnectionString = connString;

				MySqlCommand command = new MySqlCommand(sqlstring, DBConnection);
				MySqlDataAdapter dataAdapter = new MySqlDataAdapter(command);

				DataSet ds = new DataSet();

				try
				{
					DBConnection.Open();
					dataAdapter.Fill(ds, "data");
					return ds;
				}
				catch
				{
					throw;
				}
				finally
				{
					DBConnection.Close();
				}
			}
			else{
				SqlConnection DBConnection = new SqlConnection();
				DBConnection.ConnectionString = connString;

				SqlCommand command = new SqlCommand(sqlstring, DBConnection);
				SqlDataAdapter dataAdapter = new SqlDataAdapter(command);

				DataSet ds = new DataSet();

				try
				{
					DBConnection.Open();
					dataAdapter.Fill(ds, "data");
					return ds;
				}
				catch
				{
					throw;
				}
				finally
				{
					DBConnection.Close();
				}
			}
		}
	}
}
