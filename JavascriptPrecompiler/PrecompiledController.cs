using System.Web.Mvc;

namespace JavascriptPrecompiler
{
	public class PrecompiledController : Controller
	{
		public ActionResult Js(string id)
		{
			return Content(Precompiler.OutputCache[id], "application/javascript");
		}
	}
}