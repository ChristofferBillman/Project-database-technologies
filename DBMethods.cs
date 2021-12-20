using System;
using System.Data;
using System.Data.SqlClient;

namespace Projekt3
{
	public class DBMethods
	{
        // Update with new connstring from Linus when he is done with DB.
        static readonly string connString = "Server=(localdb)\\mssqllocaldb; Database=ProjectDating; Trusted_Connection=True;";
        
        /// <summary>
        /// Executes a SQL command in the database.
        /// </summary>
        /// <param name="sqlstring"> The SQL string.</param>
        /// <returns> The number of rows affected. </returns>
        public static int ExecCommand(string sqlstring)
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
        /// <summary>
        /// Executes a SQL query in the database.
        /// </summary>
        /// <param name="sqlstring"> The SQL string.  </param>
        /// <returns> The requested data. Is accessible in ds["data"]. </returns>
        public static DataSet ExecQuery(string sqlstring)
        {
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
