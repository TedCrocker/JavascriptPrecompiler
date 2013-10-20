using System;
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
		public void HandlebarsPrecompilerTest()
		{
			var template = "<div>{{hello}} world!</div>";
			var compiler = new HandlebarsPrecompiler();
			var output = compiler.GetJavascript("Yar", template);

			Assert.That(output, Is.Not.Null);
			Assert.That(output, Is.Not.Empty);
		}

		[Test]
		public void DustPrecompilerTest()
		{
			var compiler = new DustJsPrecompiler();
			var template = "<div>Hello {name}!</div>";
			var output = compiler.GetJavascript("Yar", template);
			//compiler.Add("test", @"testFiles\\helloWorld.dust");

			Assert.That(output, Is.Not.Null);
			Assert.That(output, Is.Not.Empty);
		}
	}
}
