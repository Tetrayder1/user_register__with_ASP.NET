using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace user_register.Controllers
{
    public class StockController : Controller
    {
        [Authorize(Policy ="StockPolicy")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
