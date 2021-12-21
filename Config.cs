namespace Projekt3
{
	public static class Config
	{
		// Your connection string for your specific system.
		public static readonly string CONNSTRING = "Server=(localdb)\\mssqllocaldb; Database=ProjectDating; Trusted_Connection=True; ";
		// Set this to true if using MySQL instead of Microsoft SQL Server.
		public static readonly bool USEMYSQL = false;
	}
}