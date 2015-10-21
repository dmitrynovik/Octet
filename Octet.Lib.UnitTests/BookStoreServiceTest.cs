using System;
using System.Linq;
using System.Linq.Dynamic;
using BookStore;
using NUnit.Framework;

namespace Octet.Lib.UnitTests
{
    [TestFixture]
    public class BookStoreServiceTest
    {
        [Test]
        public void Bookstore_Has_Books()
        {
            Assert.IsTrue(BookStoreService.Instance.Search().Any());
        }

        [Test]
        public void Update_Book_Updates_Book_In_Store()
        {
            const string author = "Dmitry Novik";

            var book = BookStoreService.Instance.Search().First();
            book.Author = author;
            BookStoreService.Instance.Update(book);

            Assert.AreEqual(author, BookStoreService.Instance.GetById(book.BookId, false).Author);
        }

        [Test]
        public void Can_Insert_Book()
        {
            var book = new BookData()
            {
                Author = "Dmitry Novik",
                Genre = "Fiction",
                Rating = -1,
                Title = "A Nightmare on Elm Street"
            };

            BookStoreService.Instance.Add(book);
            var book2 = BookStoreService.Instance.Search(b => b.Title == "A Nightmare on Elm Street").First();

            Assert.AreEqual("Dmitry Novik", book2.Author);
            Assert.AreEqual("A Nightmare on Elm Street", book2.Title);
            Assert.AreEqual(-1, book2.Rating);
            Assert.AreEqual("Fiction", book2.Genre);
        }

        [Test]
        public void There_are_2_Books_Of_Children_Genre()
        {
            var books = BookStoreService.Instance.Search(x => x.Genre.Contains("Children"));
            Assert.AreEqual(2, books.Count());
        }

        [Test]
        public void DynamicQuery()
        {
            BookStoreService.Instance.Search().Where("Title.ToString().Contains(\"a\")").ToList().ForEach(b => Console.WriteLine(b.Title));
        }
    }
}
