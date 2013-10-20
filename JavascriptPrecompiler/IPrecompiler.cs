namespace JavascriptPrecompiler
{
	public interface IPrecompiler
	{
		IPrecompiler Add(string templateName, string templateFilePath);
		string Compile();
	}
}