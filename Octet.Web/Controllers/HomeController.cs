using System;
using System.Collections.Generic;
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
            return View(SearchItems());
        }

        public ActionResult Export()
        {
            var items = SearchItems();
            var format = Request.QueryString["format"];
            return ExportFactory.GetFile(format, items);
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

        private List<BookData> SearchItems()
        {
            Func<BookData, bool> filter = (book) => true;
            var sortColumn = Request.QueryString["grid-column"];
            var ascending = Request.QueryString["grid-dir"] == "1";

            return _storeService.Search(filter, sortColumn, @ascending).ToList();
        }

    }
}