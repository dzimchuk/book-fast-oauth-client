using System.Threading.Tasks;
using Microsoft.AspNet.Authentication.Cookies;
using Microsoft.AspNet.Mvc;

namespace BookFast.OAuth.Client.Controllers
{
    public class AuthController : Controller
    {
        public async Task<IActionResult> SignOut()
        {
            await HttpContext.Authentication.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}