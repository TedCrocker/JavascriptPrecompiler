using JavascriptPrecompiler.Utilities;
using Jurassic;

namespace JavascriptPrecompiler.Precompilers
{
	public class DustJsPrecompiler : IPrecompiler
	{
		private const string _javascriptLibraryPath = @"JavascriptPrecompilers\dust-full.js";
		private const string _javascriptRuntimeLibraryPath = @"JavascriptPrecompilers\dust-core.js";
		private const string _javascriptCompileFunction = @"dust.compile";
		private const string _loadTemplateFunction = @"dust.loadSource";
		private readonly ScriptEngine _engine;

		public DustJsPrecompiler()
		{
			_engine = new ScriptEngine();
			_engine.Execute(FileResources.GetFileContents(_javascriptLibraryPath));
			_engine.Execute("var precompile = " + _javascriptCompileFunction);
		}

		public string LoadTemplateFunction { get { return _loadTemplateFunction; } }

		public string GetLibraryRuntimeFileContents()
		{
			return FileResources.GetFileContents(_javascriptRuntimeLibraryPath);
		}

		public string GetJavascript(string templateName, string template)
		{
			var escapedTemplate = _engine.CallGlobalFunction("precompile", template.Replace("\"", "\\\""), templateName).ToString();
			return string.Format("\t{0}('{1}');\r\n", _loadTemplateFunction, escapedTemplate);
		}
	}
}