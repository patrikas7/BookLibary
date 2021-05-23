using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using System.Linq;

namespace BookLibary.Tests
{
    [TestClass()]
    public class CommandsTests
    {
        

        [TestMethod()]
        public void BookExists_False()
        {
            Commands commands = new Commands();
            Assert.IsTrue(commands.BookExists("testas"));
        }

        [TestMethod()]
        public void BookExists_True()
        {
            Commands commands = new Commands();
            Assert.IsTrue(commands.BookExists("ff"));
        }

        [TestMethod()]
        public void GetUsersAvialableBooksCount_True()
        {
            Commands commands = new Commands();
            Assert.AreEqual(commands.GetUsersAvialableBooksCount("testas"), 3);
        }

        [TestMethod()]
        public void GetUsersAvialableBooksCount_False()
        {
            Commands commands = new Commands();
            Assert.AreNotEqual(commands.GetUsersAvialableBooksCount("testas1"), 0);
        }

        [TestMethod()]
        public void GetBooksISBN_True()
        {
            Commands commands = new Commands();
            Assert.AreEqual(commands.GetBooksISBN("w"), "g-6");
        }

        [TestMethod()]
        public void GetBooksISBN_False()
        {
            Commands commands = new Commands();
            Assert.AreNotEqual(commands.GetBooksISBN("ff"), "g-6");
        }

        [TestMethod()]
        public void AddBook_True()
        {
            Book book = new Book("testas", "testoAutorius", "Siaubas","lietuviu" ,"1999-12-07", "667-G", false);
            Commands commands = new Commands();
            commands.AddNewBook(book);
            var jsonData = File.ReadAllText(@"books.json");
            
            var booksList = JsonConvert.DeserializeObject<List<Book>>(jsonData) ?? new List<Book>();
            Assert.IsTrue(booksList.Any(x => x.Name == book.Name));
        }

        [TestMethod()]
        public void RemoveBook_Test()
        {
            Book book = new Book("testas", "testoAutorius", "Siaubas", "lietuviu", "1999-12-07", "667-G", false);
            Commands commands = new Commands();
            commands.DeleteBook(book.Name);

            var jsonData = File.ReadAllText(@"books.json");
            var booksList = JsonConvert.DeserializeObject<List<Book>>(jsonData) ?? new List<Book>();
            Assert.IsFalse(booksList.Any(x => x.Name == book.Name));
        }

    }
}