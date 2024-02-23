using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

public class FallbackController : Controller
{
     public ActionResult Index() => PhysicalFile(
            Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                "index.html"
            ),
            "text/HTML"
        );
}