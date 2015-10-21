﻿using System.Linq;
using BookStore;
using NUnit.Framework;

namespace Octet.Lib.UnitTests
{
    [TestFixture]
    public class BookStoreServiceTest
    {
        public BookStoreServiceTest()
        {
            Service = new BookStoreService(new BookStoreSource());
        }

        [Test]
        public void Bookstore_Has_Books()
        {
            Assert.IsTrue(Service.Search().Any());
        }

        [Test]
        public void Update_Book_Updates_Book_In_Store()
        {
            const string author = "Dmitry Novik";

            var book = Service.Search().First();
            book.Author = author;
            Service.Update(book);

            Assert.AreEqual(author, Service.GetById(book.BookId, false).Author);
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

            Service.Add(book);
            var book2 = Service.Search(b => b.Title == "A Nightmare on Elm Street").First();

            Assert.AreEqual("Dmitry Novik", book2.Author);
            Assert.AreEqual("A Nightmare on Elm Street", book2.Title);
            Assert.AreEqual(-1, book2.Rating);
            Assert.AreEqual("Fiction", book2.Genre);
        }

        private BookStoreService Service { get; }
    }
}
