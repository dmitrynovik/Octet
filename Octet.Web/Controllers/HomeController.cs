using System;
using System.Data.Entity.Core.Common.EntitySql;
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
            Func<BookData, bool> filter = (book) => true;
            var column = Request.QueryString["grid-column"];
            var ascending = Request.QueryString["grid-dir"] == "1";

            var items = _storeService.Search(filter, column, ascending).ToList();
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