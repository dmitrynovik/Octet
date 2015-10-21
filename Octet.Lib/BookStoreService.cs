using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Runtime.Caching;
using System.Threading;
using BookStore;

namespace Octet.Lib
{
    /// <remarks>
    /// NOTE: This simple implementation is using memory cache and therefore is not suitable for horizontal scaling.
    /// </remarks>>
    public class BookStoreService : IBookStore, IDisposable
    {
        private const int MaxTake = 1000;
        private const int RefreshCacheIntervalHours = 24;
        private readonly static DateTimeOffset CacheExpiryTime = DateTimeOffset.UtcNow.AddHours(RefreshCacheIntervalHours);
        private readonly static TimeSpan AcquireLockTimeout = TimeSpan.FromSeconds(10);

        private readonly BookStoreSource _store;
        private readonly ObjectCache _cache = new MemoryCache("book-store");
        private readonly ReaderWriterLock _lock = new ReaderWriterLock();
        private readonly Timer _cacheRefreshTimer;

        public BookStoreService(BookStoreSource store)
        {
            if (store == null) 
                throw new ArgumentNullException(nameof(store));

            _store = store;
            FillCache();

            // Start timer to refresh cache every X hours:
            _cacheRefreshTimer = new Timer(_ => FillCache(), 
                null, 
                TimeSpan.FromHours(RefreshCacheIntervalHours), 
                TimeSpan.FromHours(RefreshCacheIntervalHours));
        }

        private void FillCache()
        {
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
                var query = _cache.Select(kvp => kvp.Value).Cast<BookData>().AsQueryable();

                if (filter != null)
                {
                    query = query.Where(x => filter(x));
                }

                if (!string.IsNullOrWhiteSpace(sortBy))
                {
                    // Using Linq.Dynamic:
                    query = @ascending ? query.OrderBy(sortBy) : query.OrderBy(sortBy).Reverse();
                }

                return query.Take(MaxTake);
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
            WriteBookToStore(x => _store.Add(x), book);
        }

        public void Update(BookData book)
        {
            WriteBookToStore(x => _store.Update(x), book);
        }

        private void WriteBookToStore(Action<BookData> writeAction, BookData book)
        {
            if (writeAction == null)
                return;
            if (book == null)
                throw new ArgumentNullException(nameof(book));

            try
            {
                AcquireWriterLock();
                writeAction(book);
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

        public void Dispose()
        {
            _cacheRefreshTimer.Dispose();
        }
    }
}
