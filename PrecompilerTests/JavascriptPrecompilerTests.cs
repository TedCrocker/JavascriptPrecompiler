using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using JavascriptPrecompiler;
using JavascriptPrecompiler.Precompilers;
using JavascriptPrecompiler.Utilities;
using Jurassic;
using NUnit.Framework;

namespace PrecompilerTests
{
	[TestFixture]
	public class JavascriptPrecompilerTests
	{
		[Test]
		public void HandlebarsPrecompilerTest()
		{
			var engine = new ScriptEngine();
			engine.Execute(FileResources.GetFileContents(@"JavascriptPrecompilers\handlebars.runtime.js"));
			engine.Execute("var templates = {};");

			var template = "<div>{{hello}} world!</div>";
			var compiler = new HandlebarsPrecompiler();
			var output = compiler.GetJavascript("Yar", template);

			Assert.That(output, Is.Not.Null);
			Assert.That(output, Is.Not.Empty);

			var result = engine.Evaluate(output);
			Assert.That(result, Is.Not.Null);
		}

		[Test]
		public void DustPrecompilerTest()
		{
			var engine = new ScriptEngine();
			engine.Execute(FileResources.GetFileContents(@"JavascriptPrecompilers\dust-core.js"));
			
			var compiler = new DustJsPrecompiler();
			var template = @"<div class=""yar"">Hello {name}!</div>";
			var output = compiler.GetJavascript("Yar", template);

			Assert.That(output, Is.Not.Null);
			Assert.That(output, Is.Not.Empty);
			
			var result = engine.Evaluate(output);
			Assert.That(result, Is.Not.Null);
		}
	}
}
