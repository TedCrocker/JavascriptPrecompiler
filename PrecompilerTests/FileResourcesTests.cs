using JavascriptPrecompiler.Utilities;
using NUnit.Framework;

namespace PrecompilerTests
{
	[TestFixture]
	public class FileResourcesTests
	{
		[Test]
		public void CanGetFileThatExistsOnFileSystem()
		{
			var contents = FileResources.GetFileContents("testFiles/helloWorld.dust");

			Assert.That(contents, Is.Not.Empty);
			Assert.That(contents, Is.StringContaining("Hello {name}!"));
		}

		[Test]
		public void CanGetEmbeddedResourceWhenFileDoesNotExist()
		{
			var contents = FileResources.GetFileContents("JavascriptPrecompilers/handlebars.js");
		}
	}
}