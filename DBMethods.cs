using System;
using System.Data;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace Projekt3
{
	public class DBMethods
	{
        // Update with new connstring from Linus when he is done with DB.
        static readonly string connString = "Server=127.0.0.1;Database=Dejting;Uid=christoffer;Pwd=sten1234;";
        /// <summary>
        /// Executes a SQL command in the database.
        /// </summary>
        /// <param name="sqlstring"> The SQL string.</param>
        /// <returns> The number of rows affected. </returns>
        public static int ExecCommand(string sqlstring)
        {
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
        /// <summary>
        /// Executes a SQL query in the database.
        /// </summary>
        /// <param name="sqlstring"> The SQL string.  </param>
        /// <returns> The requested data. Is accessible in ds["data"]. </returns>
        public static DataSet ExecQuery(string sqlstring)
        {
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
    }
}
