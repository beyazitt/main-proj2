using Microsoft.AspNetCore.Mvc;

namespace OnionBase.Presentation.Controllers
{
    public class PricingController : Controller
    {

        [HttpPost]
        public IActionResult Discount()
        {
            return View();
        }
    }
}
