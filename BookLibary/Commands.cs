using System;
using Newtonsoft.Json;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace BookLibary
{
    /// <summary>
    /// Class that executes all the commands that are avialable in book libary
    /// </summary>
     public class Commands : CommandsInterface<Book>
     {
        private static readonly string booksFilePath = @"books.json";
        private static readonly string usersFilePath = @"users.json";

        /// <summary>
        /// Adds new book object to json file
        /// </summary>
        /// <param name="book">New book object that has to be added</param>
        public void AddNewBook(Book book)
        {
            var bookList = DeserializeJson();
            bookList.Add(book);
            SerelizeJson(bookList);
            if (!Console.IsOutputRedirected)
            {
                Console.Clear();
                Console.WriteLine("Thank you for adding new book to libary :) \n");
            }
           // Console.Clear();
            
        }

        /// <summary>
        /// Deletes book object from json file
        /// </summary>
        /// <param name="bookName">Book name that has to be deleted</param>
        public void DeleteBook(string bookName)
        {
            var bookList = DeserializeJson();
            var flag = bookList.Any(x => x.Name == bookName);
            if (!flag)
            {
                throw new Exception("Book does not exist in our libary :( \n");
            }

            bookList.RemoveAll(x => x.Name == bookName);
            SerelizeJson(bookList);
            if (!Console.IsOutputRedirected)
            {
                Console.Clear();
                Console.WriteLine("Book was successfully removed! \n");
            }

        }

        /// <summary>
        /// Lists all the book objects that are in json file
        /// </summary>
        public void ListAllBooks()
        {
            var bookList = DeserializeJson();
            Console.Clear();
            Console.WriteLine("Our libary book list: ");
            foreach(var book in bookList)
            {
                Console.WriteLine(book.ToString());
            }

            int selectedOption = UserInterface.filterInterface();
            FilterList(bookList, selectedOption);
            Console.Clear();

        }

        /// <summary>
        /// Gets libary users avialable books count by its name
        /// </summary>
        /// <param name="name">Libary users name</param>
        /// <returns>Number of books that user can take </returns>
        public int GetUsersAvialableBooksCount(string name)
        {
            var jsonData = File.ReadAllText(usersFilePath);
            List<LibaryUser> users = JsonConvert.DeserializeObject<List<LibaryUser>>(jsonData) ?? new List<LibaryUser>();
            bool isOldUser = users.Any(x => x.Name == name);
            int avaliableBooksForUser = 3;

            if (!isOldUser)
                return avaliableBooksForUser;
            else
            {
                LibaryUser user = users.First(x => x.Name == name);
                int takenBooksCount = user.TakenBooks.Count(x => x.Value);
                return avaliableBooksForUser - takenBooksCount;
            }

        }

        /// <summary>
        /// Returns book object to libary
        /// </summary>
        /// <param name="bookName">Book name</param>
        /// <param name="user">Users name</param>
        public void ReturnBook(string bookName, string user)
        {
            
            var books = DeserializeJson();
            var users = DeserializeJsonLibaryUser();

            foreach (var libaryUser in users)
            {
                if(libaryUser.Name == user)
                {
                    libaryUser.TakenBooks.Remove(GetBooksISBN(bookName));
                    libaryUser.TakenBookTime.Remove(GetBooksISBN(bookName));
                }
            }

            foreach(var book in books)
            {
                if(book.Name == bookName)
                {
                    book.IsTaken = false;
                }
            }

            SerelizeJson(books);
            SerelizeJsonLibaryUser(users);
        }

        /// <summary>
        /// Gets books ISBN number by its name
        /// </summary>
        /// <param name="bookName">Books name</param>
        /// <returns>Books ISBN number</returns>
        public string GetBooksISBN(string bookName)
        {
            List<Book> books = DeserializeJson();
            Book book = books.First(x => x.Name == bookName);
            return book.ISBN;
        }

        /// <summary>
        /// Finds if books exists in libary and it is avialable
        /// </summary>
        /// <param name="bookName">Book name</param>
        /// <returns>True if book exists and it is avialabe, false otherwise</returns>
        public bool BookExists(string bookName)
        {
            List<Book> books = DeserializeJson();
            return books.Any(x => x.Name == bookName && !x.IsTaken);
        }


        /// <summary>
        /// List of book objects is being filtered by selected option and displayed to the screen
        /// </summary>
        /// <param name="books"> List of book objects </param>
        /// <param name="option"> Users selected option </param>
        private static void FilterList(List<Book> books, int option)
        {
            if (option == 1)
                books = books.OrderBy(x => x.Author).ToList();
            else if (option == 2)
                books = books.OrderBy(x => x.Category).ToList();
            else if (option == 3)
                books = books.OrderBy(x => x.Language).ToList();
            else if (option == 4)
                books = books.OrderBy(x => x.ISBN).ToList();
            else if (option == 5)
                books = books.OrderBy(x => x.Name).ToList();
            else if (option == 6)
                books = books.OrderBy(x => x.IsTaken).ToList();

            foreach (var book in books)
            {
                Console.WriteLine(book.ToString());
            }
        }

        /// <summary>
        /// Converts libary user object to json file
        /// </summary>
        /// <param name="user">Libary user object</param>
        public void ConvertUserToJson(LibaryUser user)
        {
            try
            {
                var users = DeserializeJsonLibaryUser();
                var isOldUser = users.Any(x => x.Name == user.Name);
                if (isOldUser)
                {
                    foreach(var libaryUser in users)
                    {
                        if(libaryUser.Name == user.Name)
                        {
                            foreach(var item in user.TakenBooks)
                            {
                                if (!libaryUser.TakenBooks.ContainsKey(item.Key))
                                    libaryUser.TakenBooks.Add(item.Key, item.Value);
                                else
                                    throw new Exception("You already have this book! :(\n");
                            }

                            foreach (var item in user.TakenBookTime)
                            {
                                libaryUser.TakenBookTime.Add(item.Key, item.Value);
                            }

                            break;
                        }
                    }
                }
                else
                {
                    users.Add(user);
                }

                SerelizeJsonLibaryUser(users);
                Console.Clear();
                Console.WriteLine("Your loan was confirmed :) \n");
            }

            catch
            {
                Console.Clear();
                Console.WriteLine("There was an error when taking book! :( \n");
            }


        }

        /// <summary>
        /// Deserializes json with libary user objects into list of objects
        /// </summary>
        /// <returns>List of deserialzed libary user objects</returns>
        private List<LibaryUser> DeserializeJsonLibaryUser()
        {
            var jsonData = File.ReadAllText(usersFilePath);
            var booksList = JsonConvert.DeserializeObject<List<LibaryUser>>(jsonData) ?? new List<LibaryUser>();

            return booksList;
        }

        /// <summary>
        /// Serelize list of libary user objects to json file
        /// </summary>
        /// <param name="users">List of libary user objects</param>
        private static void SerelizeJsonLibaryUser(List<LibaryUser> users)
        {
            var jsonData = JsonConvert.SerializeObject(users, Formatting.Indented);
            File.WriteAllText(usersFilePath, jsonData);
        }

        /// <summary>
        /// Json file is being deserialized into list of book objects
        /// </summary>
        /// <returns> List of deserialized book objects </returns>
        private List<Book> DeserializeJson()
        {
            var jsonData = File.ReadAllText(booksFilePath);
            var booksList = JsonConvert.DeserializeObject<List<Book>>(jsonData) ?? new List<Book>();

            return booksList;
        }

        /// <summary>
        /// List of book objects is being serelized into Json file
        /// </summary>
        /// <param name="books"> List of book objects </param>
        private static void SerelizeJson(List<Book> books)
        {
            var jsonData = JsonConvert.SerializeObject(books, Formatting.Indented);
            if (File.Exists(booksFilePath))
                File.WriteAllText(booksFilePath, jsonData);
            else
            {
                File.Create(booksFilePath);
                File.WriteAllText(booksFilePath, jsonData);
            }
        }

    }
}
