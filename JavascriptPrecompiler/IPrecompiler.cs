using System.Web.Mvc;

namespace JavascriptPrecompiler
{
	public interface IPrecompiler
	{
		IPrecompiler Add(string templateName, string templateFilePath);
		MvcHtmlString Compile();
		IPrecompiler IncludeLibrary();
	}
}