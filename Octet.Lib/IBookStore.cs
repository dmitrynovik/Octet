using System;
using System.Collections.Generic;
using System.Linq;
using BookStore;

namespace Octet.Lib
{
    interface IBookStore
    {
        IQueryable<BookData> Search(Func<BookData, bool> filter = null, string sortBy = null, bool ascending = true);
        BookData GetById(int id, bool cached = true);
        void Add(BookData book);
        void Update(BookData book);
        IReadOnlyCollection<string> GetSortingFields();
    }
}
