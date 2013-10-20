using System.Web.Mvc;

namespace JavascriptPrecompiler
{
	public interface IPrecompiler
	{
		string GetLibraryRuntimeFileContents();
		string GetJavascript(string templateName, string template);
	}
}