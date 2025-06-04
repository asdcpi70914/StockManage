using Microsoft.AspNetCore.Mvc;

namespace SRC.Backend.Controllers.Delivery
{
    public class DeliveryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
