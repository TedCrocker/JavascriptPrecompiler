using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace JavascriptPrecompiler
{
	public class Precompiler
	{
		internal static IDictionary<string, string> OutputCache = new Dictionary<string, string>(); 

		public static Precompiler DustJS()
		{
			return new Precompiler(new DustJsPrecompiler());
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

		public MvcHtmlString Compile()
		{
			var builder = BuildOutput();
			var output = builder.ToString();

			var hash = "template" + new MD5Hasher().GetHash(output);
			OutputCache.Add(hash, output);

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
			foreach (var file in _filesToLoad)
			{
				var filePath = file.Value;
				if (HttpContext.Current != null)
				{
					filePath = HttpContext.Current.Server.MapPath(filePath);
				}
				var input = File.ReadAllText(filePath);
				builder.AppendFormat("\t{0}('{1}');\r\n", _precompiler.LoadTemplateFunction, _precompiler.PrecompileTemplate(file.Key, input));
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