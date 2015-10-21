using System.Linq;
using BookStore;
using NUnit.Framework;

namespace Octet.Lib.UnitTests
{
    [TestFixture]
    public class BookStoreServiceTest
    {
        [Test]
        public void Initializing_Service_Does_Not_Throw_Exception()
        {
            CreateService();
        }

        [Test]
        public void Bookstore_Has_Books()
        {
            Assert.IsTrue(CreateService().Search().Any());
        }

        [Test]
        public void Update_Book_Updates_Book_In_Store()
        {
            const string author = "Dmitry Novik";

            var service = CreateService();
            var book = service.Search().First();
            book.Author = author;
            service.Update(book);

            Assert.AreEqual(author, service.GetById(book.BookId, false).Author);
        }

        [Test]
        public void Can_Insert_Book()
        {
            var service = CreateService();
            var book = new BookData()
            {
                Author = "Dmitry Novik",
                Genre = "Fiction",
                Rating = -1,
                Title = "A Nightmare on Elm Street"
            };

            service.Add(book);
            var book2 = service.Search(b => b.Title == "A Nightmare on Elm Street").First();

            Assert.AreEqual("Dmitry Novik", book2.Author);
            Assert.AreEqual("A Nightmare on Elm Street", book2.Title);
            Assert.AreEqual(-1, book2.Rating);
            Assert.AreEqual("Fiction", book2.Genre);
        }

        private static BookStoreService CreateService()
        {
            return new BookStoreService(new BookStoreSource());
        }
    }
}
