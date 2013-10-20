using System.Web.Mvc;

namespace JavascriptPrecompiler
{
	public interface IPrecompiler
	{
		string GetLibraryRuntimeFileContents();
		string PrecompileTemplate(string templateName, string template);
		string LoadTemplateFunction { get; }
	}
}