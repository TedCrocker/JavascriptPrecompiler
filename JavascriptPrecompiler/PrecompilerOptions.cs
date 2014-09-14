namespace JavascriptPrecompiler
{
	public static class PrecompilerOptions
	{
		private static string _templateNamespace = "templates";
		public static string TemplateNamespace
		{
			get { return _templateNamespace; }
			set { _templateNamespace = value; }
		}
	}
}