using System.Threading.Tasks;
using BookFast.OAuth.Client.Proxy;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;

namespace BookFast.OAuth.Client.Controllers
{
    public class HomeController : Controller
    {
        private readonly BookFastApiProxy proxy;

        public HomeController(BookFastApiProxy proxy)
        {
            this.proxy = proxy;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> Bookings()
        {
            ViewBag.Title = "Hello, stranger!";
            var bookings = await proxy.LoadBookingsAsync(User.FindFirst("access_token").Value);

            return View(bookings);
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
