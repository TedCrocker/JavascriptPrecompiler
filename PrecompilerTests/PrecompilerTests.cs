﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JavascriptPrecompiler;
using NUnit.Framework;

namespace PrecompilerTests
{
	[TestFixture]
	public class PrecompilerTests
	{
		[Test]
		public void HandlebarsTest()
		{
			var template = "<div>{{hello}} world!</div>";
			var precompile = TemplatePrecompiler.PrecompileHandlebars("Test", template);
			Assert.That(precompile, Is.Not.Null);
			Assert.That(precompile, Is.Not.Empty);
		}

		[Test]
		public void DustJSTest()
		{
			var template = "<div>{hello} world!</div>";
			var precompile = TemplatePrecompiler.PrecompileDustJS("Test", template);
			Assert.That(precompile, Is.Not.Null);
			Assert.That(precompile, Is.Not.Empty);
		}

		[Test]
		public void DustPrecompilerTest()
		{
			var compiler = new DustJsPrecompiler();
			compiler.Add("test", @"testFiles\\helloWorld.dust");
			compiler.Compile();

			Assert.That(compiler.Output, Is.Not.Null);
			Assert.That(compiler.Output, Is.Not.Empty);
			File.WriteAllText("test.js", compiler.Output);
		}
	}
}