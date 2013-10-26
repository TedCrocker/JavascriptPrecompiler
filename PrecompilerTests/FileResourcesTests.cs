using System.IO;
using JavascriptPrecompiler.Utilities;
using NUnit.Framework;
using System.Linq;

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

			Assert.That(contents, Is.Not.Empty);
			Assert.That(contents, Is.StringContaining("handlebars"));
		}

		[Test]
		public void CanGetMultipleFilesAndThereContents()
		{
			var filesAndContents = FileResources.GetTemplateFilePaths("testFiles/*.dust");

			Assert.That(filesAndContents.Count(), Is.EqualTo(2));
			Assert.That(filesAndContents.ContainsKey("helloWorld"));
			Assert.That(filesAndContents["helloWorld"], Is.StringContaining("helloWorld.dust"));
			Assert.That(filesAndContents.ContainsKey("helloWorld2"));
			Assert.That(filesAndContents["helloWorld2"], Is.StringContaining("helloWorld2.dust"));
		}
	}
}