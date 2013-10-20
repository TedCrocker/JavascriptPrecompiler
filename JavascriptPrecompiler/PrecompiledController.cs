using System.Web.Mvc;

namespace JavascriptPrecompiler
{
	public class PrecompiledController : Controller
	{
		public ActionResult Js(string key)
		{
			return Content("OK");
		}
	}
}