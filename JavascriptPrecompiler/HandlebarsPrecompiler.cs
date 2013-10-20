using Jurassic;

namespace JavascriptPrecompiler
{
	public class HandlebarsPrecompiler : IPrecompiler
	{
		private const string _javascriptLibraryPath = @"JavascriptPrecompilers\handlebars.js";
		private const string _javascriptCompileFunction = @"Handlebars.precompile";
		private const string _javascriptRuntimeLibrary = @"JavascriptPrecompilers\handlebars.runtime.js";
		private readonly ScriptEngine _engine;

		public HandlebarsPrecompiler()
		{
			_engine = new ScriptEngine();
			_engine.Execute(FileResources.GetFileContents(_javascriptLibraryPath));
			_engine.Execute("var precompile = " + _javascriptCompileFunction);
		}

		public string GetLibraryRuntimeFileContents()
		{
			return FileResources.GetFileContents(_javascriptRuntimeLibrary);
		}

		public string GetJavascript(string templateName, string template)
		{
			return string.Format("templates.{0} = {1};", templateName, _engine.CallGlobalFunction("precompile", template));
		}
	}
}
