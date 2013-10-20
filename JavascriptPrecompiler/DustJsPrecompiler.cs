using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Jurassic;

namespace JavascriptPrecompiler
{
	public class DustJsPrecompiler : IPrecompiler
	{
		private const string _javascriptLibraryPath = @"JavascriptPrecompilers\dust.js";
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
			return FileResources.GetFileContents(_javascriptLibraryPath);
		}

		public string GetJavascript(string templateName, string template)
		{
			return string.Format("\t{0}('{1}');\r\n", _loadTemplateFunction, _engine.CallGlobalFunction("precompile", template, templateName).ToString());
		}
	}
}