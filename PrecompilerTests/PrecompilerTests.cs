using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JavascriptPrecompiler;
using JavascriptPrecompiler.Precompilers;
using JavascriptPrecompiler.Utilities;
using NUnit.Framework;

namespace PrecompilerTests
{
	[TestFixture]
	public class PrecompilerTests
	{
		private Precompiler _precompiler;

		[SetUp]
		public void Setup()
		{
			_precompiler = new Precompiler(new FakeJSPrecompiler(), new FakeDebugStatusChecker());
			_precompiler.GetType().GetField("OutputCache", BindingFlags.NonPublic | BindingFlags.Static).SetValue(null, new Dictionary<string, string>());
		}

		[Test]
		public void CanAddSingleFile()
		{
			_precompiler.Add("testTemplate", "testFiles/helloWorld.dust");

			var result = _precompiler.Compile();
			Assert.That(result, Is.Not.Null);
			Assert.That(Cache.Count, Is.EqualTo(1));
			Assert.That(Cache.First().Value, Is.StringContaining("testTemplate"));
		}

		[Test]
		public void CanAddMultipleFiles()
		{
			_precompiler.Add("testFiles/*.dust");
			var results = _precompiler.Compile();

			Assert.That(results, Is.Not.Null);
			Assert.That(Cache.Count, Is.EqualTo(1));

			Assert.That(Cache.First().Value, Is.StringContaining("var helloWorld = "));
			Assert.That(Cache.First().Value, Is.StringContaining("var helloWorld2 = "));
		}

		[Test]
		public void CanIncludeRuntimeLibrary()
		{
			_precompiler.IncludeLibrary();
			var result = _precompiler.Compile();

			Assert.That(result, Is.Not.Null);
			Assert.That(Cache.Count, Is.EqualTo(1));
			Assert.That(Cache.First().Value, Is.StringContaining("var testLibrary = \"this is a test file!\";"));
		}

		[Test]
		public void CanGetCompiledResult()
		{
			_precompiler.IncludeLibrary();
			var results = _precompiler.Compile().ToString();
			var fileName = results.Replace("<script type='text/javascript' src='~/Precompiled/Js/", "").Replace("'></script>", "");
			Assert.That(Cache[fileName], Is.Not.Null);
		}

		[Test]
		public void CanOverrideDefaultNamespace()
		{
			var results = _precompiler.Compile("override").ToString();
			var fileName = results.Replace("<script type='text/javascript' src='~/Precompiled/Js/", "").Replace("'></script>", "");
			Assert.That(Cache[fileName], Is.StringContaining("window.override"));
		}

		private IDictionary<string, string> Cache
		{
			get
			{
				return _precompiler.GetType().GetField("OutputCache", BindingFlags.NonPublic | BindingFlags.Static).GetValue(_precompiler)
					as IDictionary<string, string>;
			}
		}

	}

	public class FakeDebugStatusChecker : IDebugStatusChecker
	{
		public bool InDebugMode { get { return true; } }
	}

	public class FakeJSPrecompiler : IPrecompiler
	{
		public string GetLibraryRuntimeFileContents()
		{
			return FileResources.GetFileContents(@"testFiles\testLibrary.js");
		}

		public string GetJavascript(string templateName, string template)
		{
			return "var " + templateName + " = '" + template + "';";
		}
	}
}