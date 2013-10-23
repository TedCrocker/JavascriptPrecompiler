using JavascriptPrecompiler;
using JavascriptPrecompiler.Utilities;
using NUnit.Framework;

namespace PrecompilerTests
{
	[TestFixture]
	public class IFileHasherTests
	{
		[Test]
		public void MD5HasherTests()
		{
			var hasher = new MD5Hasher();
			var fileContents = @"(function()
{
	dust.loadSource('(function(){dust.register(""test"",body_0);function body_0(chk,ctx){return chk.write(""<div>Hello "").reference(ctx.get(""name""),ctx,""h"").write(""!</div>"");}return body_0;})();');
})();
";
			var hash = hasher.GetHash(fileContents);
			Assert.That(hash, Is.Not.Empty);
		}
	}
}