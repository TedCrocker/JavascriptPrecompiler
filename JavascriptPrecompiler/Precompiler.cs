using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Mvc;
using JavascriptPrecompiler.Precompilers;
using JavascriptPrecompiler.Utilities;

namespace JavascriptPrecompiler
{
	public class Precompiler
	{
		internal static IDictionary<string, string> OutputCache = new Dictionary<string, string>(); 

		public static Precompiler DustJS()
		{
			return new Precompiler(new DustJsPrecompiler());
		}

		public static Precompiler Handlebars()
		{
			return new Precompiler(new HandlebarsPrecompiler());
		}

		private readonly IDictionary<string, string> _filesToLoad = new Dictionary<string, string>();
		private IPrecompiler _precompiler;
		private const string _controllerPath = "~/Precompiled/Js/{0}";
		private bool _includeRuntimeLibrary;
		private const string _scriptTag = @"<script type='text/javascript' src='{0}'></script>";


		public Precompiler(IPrecompiler precompiler)
		{
			_precompiler = precompiler;
		}

		public Precompiler Add(string templateName, string templateFilePath)
		{
			_filesToLoad.Add(templateName, templateFilePath);
			return this;
		}

		public Precompiler Add(string templateFileSearchPath)
		{
			foreach (var fileFilePath in FileResources.GetTemplateFilePaths(templateFileSearchPath))
			{
				_filesToLoad.Add(fileFilePath.Key, fileFilePath.Value);
			}
			return this;
		}

		public MvcHtmlString Compile()
		{
			var builder = BuildOutput();
			var output = builder.ToString();

			var hash = "template" + new MD5Hasher().GetHash(output);
			if (!OutputCache.ContainsKey(hash))
			{
				OutputCache.Add(hash, output);				
			}

			return new MvcHtmlString(string.Format(_scriptTag, GetPath(hash)));
		}

		public Precompiler IncludeLibrary()
		{
			_includeRuntimeLibrary = true;
			return this;
		}

		private StringBuilder BuildOutput()
		{
			var builder = new StringBuilder();

			if (_includeRuntimeLibrary)
			{
				builder.Append(_precompiler.GetLibraryRuntimeFileContents());
				builder.Append(";\r\n");
			}

			builder.AppendLine("(function()\r\n{");
			builder.AppendLine("\twindow.templates = {};");
			foreach (var file in _filesToLoad)
			{
				var filePath = file.Value;
				if (!File.Exists(filePath) && HttpContext.Current != null)
				{
					filePath = HttpContext.Current.Server.MapPath(filePath);
				}
				var input = File.ReadAllText(filePath);
				builder.AppendLine(_precompiler.GetJavascript(file.Key, input));

			}
			builder.AppendLine("})();");
			return builder;
		}

		private static string GetPath(string fileKey)
		{
			if (HttpContext.Current == null)
			{
				return string.Format(_controllerPath, fileKey);
			}

			return VirtualPathUtility.ToAbsolute(string.Format(_controllerPath, fileKey));
		}
	}
}