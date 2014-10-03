using System.Web;

namespace JavascriptPrecompiler.Utilities
{
	public class DebugStatusChecker : IDebugStatusChecker
	{
		public bool InDebugMode { get { return HttpContext.Current.IsDebuggingEnabled; } }
	}
}