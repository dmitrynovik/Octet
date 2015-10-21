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

        public ActionResult Edit(int id)
        {
            var book = _storeService.Search(x => x.BookId == id).FirstOrDefault();
            return View(book);
        }

        [HttpPost]
        public ActionResult Edit(BookData book)
        {
            if (book.BookId == 0)
            {
                _storeService.Add(book);
            }
            else
            {
                _storeService.Update(book);
            }
            return RedirectToAction("Index");
        }
    }
}