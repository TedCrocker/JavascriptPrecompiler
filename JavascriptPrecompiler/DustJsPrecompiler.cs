using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using Jurassic;

namespace JavascriptPrecompiler
{
	public class DustJsPrecompiler : IPrecompiler
	{
		private const string _scriptTag = @"<script type='text/javascript' src='{0}'></script>";
		private const string _controllerPath = "~/Precompiler/Js/{0}";
		private const string _javascriptLibraryPath = @"Scripts\dust.js";
		private const string _javascriptCompileFunction = @"dust.compile";
		private const string _loadTemplateFunction = @"dust.loadSource";
		private readonly IDictionary<string, string> _filesToLoad = new Dictionary<string, string>();
		private readonly ScriptEngine _engine;

		public DustJsPrecompiler()
		{
			_engine = new ScriptEngine();
			_engine.ExecuteFile(_javascriptLibraryPath);
			_engine.Execute("var precompile = " + _javascriptCompileFunction);
		}

		public IPrecompiler Add(string templateName, string templateFilePath)
		{
			_filesToLoad.Add(templateName, templateFilePath);
			return this;
		}

		private static string GetPath(string fileKey)
		{
			if (HttpContext.Current == null)
			{
				return string.Format(_controllerPath, fileKey);
			}

			return VirtualPathUtility.ToAbsolute(string.Format(_controllerPath, fileKey));
		}

		public string Compile()
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
			var output = builder.ToString();

			var hash = new MD5Hasher().GetHash(output);
			Precompiler.OutputCache.Add(hash, output);

			return string.Format(_scriptTag, GetPath(hash));
		}

	}
}