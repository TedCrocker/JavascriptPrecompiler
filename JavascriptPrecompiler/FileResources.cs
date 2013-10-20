using System.IO;
using System.Reflection;

namespace JavascriptPrecompiler
{
	public class FileResources
	{
		public static string GetFileContents(string filePath)
		{
			var contents = "";
			if (File.Exists(filePath))
			{
				contents = File.ReadAllText(filePath);
			}
			else
			{
				contents = TryGetEmbeddedResource(filePath, contents);
			}
			return contents;
		}

		private static string TryGetEmbeddedResource(string filePath, string contents)
		{
			var resourceName = "JavascriptPrecompiler." + filePath.Replace("/", ".").Replace("\\", ".");
			using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
			using (var reader = new StreamReader(stream))
			{
				contents = reader.ReadToEnd();
			}
			return contents;
		}
	}
}