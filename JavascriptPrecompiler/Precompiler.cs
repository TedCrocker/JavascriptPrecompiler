using System.Collections.Generic;

namespace JavascriptPrecompiler
{
	public class Precompiler
	{
		public static IDictionary<string, string> OutputCache = new Dictionary<string, string>(); 

		public static IPrecompiler Templates()
		{
			return new DustJsPrecompiler();
		}
	}
}