using System.Security.Cryptography;
using System.Text;

namespace JavascriptPrecompiler.Utilities
{
	public class MD5Hasher : IFileHasher
	{
		public string GetHash(string fileContents)
		{
			MD5 md5 = MD5.Create();
			var data = md5.ComputeHash(Encoding.UTF8.GetBytes(fileContents));
			var builder = new StringBuilder();
			foreach(var md5Byte in data)
			{
				builder.Append(md5Byte.ToString("x2"));
			}
			return builder.ToString();
		}
	}
}