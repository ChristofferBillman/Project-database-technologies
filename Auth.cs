using Microsoft.AspNetCore.Http;

namespace Projekt3
{
	public static class Auth
	{
		/// <summary>
		/// Checks if a user token is valid.
		/// </summary>
		/// <param name="token">The user token, should be avaliable in Response/Request.Cookies.</param>
		/// <returns>True if token is valid, otherwise false.</returns>
		/// <exception cref="System.NotImplementedException"></exception>
		public static bool Authenticate(string token)
		{	
			// Fetch user from DB
			// Compare password in token to password in DB.
			throw new System.NotImplementedException();
		}
		/// <summary>
		/// Encrypts a given password.
		/// </summary>
		/// <param name="password"></param>
		/// <returns> The encrypted password.</returns>
		/// <exception cref="System.NotImplementedException"></exception>
		public static string Encrypt(string password)
		{
			throw new System.NotImplementedException();
		}
		/// <summary>
		/// Performs a nullcheck on all fields in a form.
		/// </summary>
		/// <param name="f"></param>
		/// <returns>True if no fields are null, otherwise false.</returns>
		/// <exception cref="System.NotImplementedException"></exception>
		public static bool NullCheckForm(IFormCollection f)
		{
			throw new System.NotImplementedException();
		}
	}
}
