using Microsoft.AspNetCore.Http;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using Projekt3.Models;

namespace Projekt3
{
	public static class Auth
	{
		/// <summary>
		/// Checks if given token corresponds with the one from DB.
		/// </summary>
		/// <param name="token">The user token, should be avaliable in Response/Request.Cookies.</param>
		/// <returns>True if token is valid, otherwise false.</returns>
		/// <exception cref="System.NotImplementedException"></exception>
		public static bool Authenticate(string token)
		{
			// Get the profileId from the token.
			int profileId = int.Parse(token.Split('_')[0]);

			//Get profile from DB.
			ProfileModel pm = ProfileMethods.SelectOne(profileId);

			// If no users are found, return false.
			if (pm == null) return false;

			// If the recieved token is the same as the same
			// as the one created from DB credentials, the token is valid.
			if (token == (pm.ID.ToString() +"_" + pm.Password))
			{
				return true;
			}
			return false;
		}
		/// <summary>
		/// Authenticates a user.
		/// </summary>
		/// <param name="profileId">The users profileId.</param>
		/// <param name="password">The users password.</param>
		/// <returns>True if password is correct, otherwise false.</returns>
		public static bool Authenticate(ProfileModel pm, string password)
		{
			if (pm == null) return false;

			if (Hash(password,pm.Salt) == pm.Password)
			{
				return true;
			}
			return false;
		}
		/// <summary>
		/// Hashes a given password.
		/// </summary>
		/// <param name="password">The cleartext password.</param>
		/// <param name="salt">The salt. Use method getRandomSalt to get a new one.</param>
		/// <returns> The hashed password.</returns>
		/// <exception cref="System.NotImplementedException"></exception>
		public static string Hash(string password, string salt)
		{
			return Convert.ToBase64String(KeyDerivation.Pbkdf2(
				password,
				Convert.FromBase64String(salt),
				KeyDerivationPrf.HMACSHA256,
				100000,
				256 / 8));
		}
		/// <summary>
		/// Generates a random salt.
		/// NOTE: Don't forget to set the profiles "Salt"-variabe before inserting into DB.
		/// </summary>
		/// <returns>The salt.</returns>
		public static string GetRandomSalt()
		{
			byte[] salt = new byte[128 / 8];

			RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider();

			try
			{
				rngCsp.GetNonZeroBytes(salt);
			}
			finally
			{
				rngCsp.Dispose();
			}

			return Convert.ToBase64String(salt);
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
