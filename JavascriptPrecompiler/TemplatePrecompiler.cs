using Jurassic;


namespace JavascriptPrecompiler
{
	public static class TemplatePrecompiler
	{
		public static string PrecompileHandlebars(string name, string template)
		{
			var engine = new ScriptEngine();
			engine.ExecuteFile(@"Scripts\handlebars.js");
			engine.Execute(@"var precompile = Handlebars.precompile;");
			return string.Format("var {0} = Handlebars.template({1});", name, engine.CallGlobalFunction("precompile", template).ToString());
		}

		public static string PrecompileDustJS(string templateName, string template)
		{
			var engine = new ScriptEngine();
			engine.ExecuteFile(@"Scripts\dust.js");
			engine.Execute(@"var precompile = dust.compile;");
			return engine.CallGlobalFunction("precompile", template, templateName).ToString();
		}
	}
}
