using Microsoft.AspNetCore.Mvc;

namespace ShoppingList.Controllers
{
    public class IdentityController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(string emailAddress, string password)
        {
            return View();
        }
    }
}
