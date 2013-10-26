using System.Collections.Generic;
using System.IO;
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
			_precompiler = new Precompiler(new FakeJSPrecompiler());
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

		private IDictionary<string, string> Cache
		{
			get
			{
				return _precompiler.GetType().GetField("OutputCache", BindingFlags.NonPublic | BindingFlags.Static).GetValue(_precompiler)
					as IDictionary<string, string>;
			}
		}

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