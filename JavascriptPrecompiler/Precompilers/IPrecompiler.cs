namespace JavascriptPrecompiler.Precompilers
{
	public interface IPrecompiler
	{
		string GetLibraryRuntimeFileContents();
		string GetJavascript(string templateName, string template);
	}
}