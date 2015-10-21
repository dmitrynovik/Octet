using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Reflection;
using System.Runtime.Caching;
using System.Threading;
using BookStore;

namespace Octet.Lib
{
    public class BookStoreService : IBookStore
    {
        private const int MaxTake = 1000;
        private readonly static DateTimeOffset CacheExpiryTime = DateTimeOffset.UtcNow.AddHours(24);
        private readonly static TimeSpan AcquireLockTimeout = TimeSpan.FromSeconds(10);
        private readonly static IReadOnlyCollection<string> Fields; 

        private readonly BookStoreSource _store;
        private readonly ObjectCache _cache = new MemoryCache("book-store");
        private readonly ReaderWriterLock _lock = new ReaderWriterLock();

        static BookStoreService()
        {
            Fields = typeof (BookData).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Select(p => p.Name)
                .OrderBy(x => x)
                .ToArray();
        }

        public BookStoreService(BookStoreSource store)
        {
            if (store == null) 
                throw new ArgumentNullException(nameof(store));

            _store = store;
            try
            {
                AcquireWriterLock();
                // fill in-memory cache:
                _store.GetAll().ForEach(UpdateCache);
            }
            finally
            {
                ReleaseWriterLock();
            }
        }

        public IQueryable<BookData> Search(Func<BookData, bool> filter = null, string sortBy = null, bool @ascending = true)
        {
            try
            {
                AcquireReaderLock();
                var query = _cache.AsQueryable();

                if (filter != null)
                {
                    query = query.Where(kvp => filter((BookData) kvp.Value));
                }

                if (!string.IsNullOrWhiteSpace(sortBy))
                {
                    // Using Linq.Dynamic:
                    query = @ascending ? query.OrderBy(sortBy) : query.OrderBy(sortBy).Reverse();
                }

                return query.Select(kvp => kvp.Value).Take(MaxTake).Cast<BookData>();
            }
            finally
            {
                ReleaseReaderLock();
            }
        }

        public BookData GetById(int id, bool cached = true)
        {
            try
            {
                AcquireReaderLock();
                if (cached)
                    return (BookData)_cache.Get(id.ToString()) ?? _store.GetById(id);

                return _store.GetById(id);
            }
            finally
            {
                ReleaseReaderLock();
            }
        }

        public void Add(BookData book)
        {
            UpdateBook(x => _store.Add(x), book);
        }

        public void Update(BookData book)
        {
            UpdateBook(x => _store.Update(x), book);
        }

        public IReadOnlyCollection<string> GetSortingFields()
        {
            return Fields;
        }

        private void UpdateBook(Action<BookData> updateAction, BookData book)
        {
            if (updateAction == null)
                return;
            if (book == null)
                throw new ArgumentNullException(nameof(book));

            try
            {
                AcquireWriterLock();
                updateAction(book);
                UpdateCache(book);
            }
            finally
            {
                ReleaseWriterLock();
            }
        }

        private void UpdateCache(BookData book)
        {
            _cache.Set(book.BookId.ToString(), book, CacheExpiryTime);
        }

        private void AcquireReaderLock()
        {
            _lock.AcquireReaderLock(AcquireLockTimeout);
        }

        private void ReleaseReaderLock()
        {
            _lock.ReleaseReaderLock();
        }

        private void AcquireWriterLock()
        {
            _lock.AcquireWriterLock(AcquireLockTimeout);
        }

        private void ReleaseWriterLock()
        {
            _lock.ReleaseWriterLock();
        }
    }
}
