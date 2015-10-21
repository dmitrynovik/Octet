using System.Linq;
using System.Web.Mvc;
using BookStore;
using Octet.Lib;

namespace Octet.Web.Controllers
{
    public class HomeController : Controller
    {
        readonly BookStoreService _storeService = new BookStoreService(new BookStoreSource());

        public ActionResult Index()
        {
            var items = _storeService.Search().ToList();
            return View(items);
        }
    }
}