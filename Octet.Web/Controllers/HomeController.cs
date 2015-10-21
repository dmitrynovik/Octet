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
            var book = BookStoreService.Instance.Search(x => x.BookId == id).FirstOrDefault();
            return View(book);
        }

        [HttpPost]
        public ActionResult Edit(BookData book)
        {
            if (book.BookId == 0)
            {
                BookStoreService.Instance.Add(book);
            }
            else
            {
                BookStoreService.Instance.Update(book);
            }
            return RedirectToAction("Index");
        }

        private List<BookData> SearchItems()
        {
            string filter = ComposePredicate(Request.QueryString["grid-filter"]);
            var sortColumn = Request.QueryString["grid-column"];
            var ascending = Request.QueryString["grid-dir"] == "1";

            return BookStoreService.Instance.Search(filter, sortColumn, @ascending).ToList();
        }

        private static string ComposePredicate(string gridFilterString)
        {
            if (gridFilterString == null)
                return null;

            var tokens = gridFilterString.Split(new[] { "__" }, StringSplitOptions.RemoveEmptyEntries);
            if (tokens.Length < 3)
                return null;

            string sProperty = tokens[0];
            string sOperator = tokens[1];
            string sOperand= tokens[2];
            switch (sOperator)
            {
                case "1":
                    return string.Format("{0}.ToString().Equals(\"{1}\")", sProperty, sOperand);
                case "2":
                    return string.Format("{0}.Contains(\"{1}\")", sProperty, sOperand);
                case "3":
                    return string.Format("{0}.StartsWith(\"{1}\")", sProperty, sOperand);
                case "4":
                    return string.Format("{0}.EndsWith(\"{1}\")", sProperty, sOperand);
                case "5":
                    return string.Format("{0} > {1}", sProperty, sOperand);
                case "6":
                    return string.Format("{0} < {1}", sProperty, sOperand);
                default:
                    return null;
            }
        }
    }
}