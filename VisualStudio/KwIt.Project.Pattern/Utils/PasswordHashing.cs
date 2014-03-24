using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace KwIt.Project.Pattern.Utils
{
	public static class PasswordHashing
	{
		public static string CalculateSha1(string password)
		{
			byte[] buffer = Encoding.UTF8.GetBytes(password);
			SHA1CryptoServiceProvider cryptoTransformSha1 = new SHA1CryptoServiceProvider();
			string hash = BitConverter.ToString(cryptoTransformSha1.ComputeHash(buffer)).Replace("-", "");

			return hash;
		}

		public static bool ValidatePassword(string password, string dbPassword)
		{
			string passHash = CalculateSha1(password);
			return passHash.Equals(dbPassword);
		}
	}
}
