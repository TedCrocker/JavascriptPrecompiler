using System.IO;
using System.Reflection;
using System.Web;

namespace JavascriptPrecompiler
{
	public class FileResources
	{
		public static string GetFileContents(string filePath)
		{
			var contents = "";
			if (File.Exists(GetFilePath(filePath)))
			{
				contents = File.ReadAllText(GetFilePath(filePath));
			}
			else
			{
				contents = TryGetEmbeddedResource(filePath, contents);
			}
			return contents;
		}

		private static string GetFilePath(string filePath)
		{
			if (HttpContext.Current != null)
			{
				filePath = HttpContext.Current.Server.MapPath("~/" + filePath);
				return filePath;
			}

			return filePath;
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