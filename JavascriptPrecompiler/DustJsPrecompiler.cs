using System.Collections.Generic;
using System.IO;
using System.Text;
using Jurassic;

namespace JavascriptPrecompiler
{
	public class DustJsPrecompiler
	{
		private const string _javascriptLibraryPath = @"Scripts\dust.js";
		private const string _javascriptCompileFunction = @"dust.compile";
		private const string _loadTemplateFunction = @"dust.loadSource";
		private readonly IDictionary<string, string> _filesToLoad = new Dictionary<string, string>();
		private readonly ScriptEngine _engine;

		public string Output { get; private set; }

		public DustJsPrecompiler()
		{
			_engine = new ScriptEngine();
			_engine.ExecuteFile(_javascriptLibraryPath);
			_engine.Execute("var precompile = " + _javascriptCompileFunction);
		}

		public DustJsPrecompiler Add(string templateName, string templateFilePath)
		{
			_filesToLoad.Add(templateName, templateFilePath);
			return this;
		}

		public DustJsPrecompiler Compile()
		{
			var builder = new StringBuilder();
			builder.AppendLine("(function()\r\n{");
			foreach (var file in _filesToLoad)
			{
				var input = File.ReadAllText(file.Value);
				var precompileTemplate = _engine.CallGlobalFunction("precompile", input, file.Key);
				builder.AppendFormat("\t{0}('{1}');\r\n", _loadTemplateFunction, precompileTemplate);
			}
			builder.AppendLine("})();");

			Output = builder.ToString();
			return this;
		}

	}
}