using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using BookStore;

namespace Octet.Lib
{
    interface IBookStore
    {
        IEnumerable<BookData> GetAll(Expression<Func<BookData, bool>> filter );
        BookData GetById(int id);
        void Add(BookData book);
        void Update(BookData book);
    }
}
