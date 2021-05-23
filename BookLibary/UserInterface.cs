using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookLibary
{
    /// <summary>
    /// Class that has all the console interface methods
    /// </summary>
    class UserInterface
    {
        /// <summary>
        /// Shows main libary interface with all the options
        /// </summary>
        public static void showInterface()
        {
            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("Welcome to my book libary! \nWhat would you like to do? (Type number of selected option)");
                Console.WriteLine("1. Add a new book");
                Console.WriteLine("2. Take a book");
                Console.WriteLine("3. Return a book");
                Console.WriteLine("4. List all the books");
                Console.WriteLine("5. Delete a book");
                Console.WriteLine("6. Exit");
                int option = 0;
                try
                {
                    option = int.Parse(Console.ReadLine());
                    if(option < 1 || option > 6)
                    {
                        throw new Exception();
                    }
                }
                catch
                {
                    Console.Clear();
                    Console.WriteLine("Selected choice does not exist :( \n");
                    continue;
                }

                try
                {
                    Commands commands = new Commands();
                    switch (option)
                    {
                        case 1:
                            {
                                Book book = createBook();
                                commands.AddNewBook(book);
                                break;
                            }

                        case 2:
                            {
                                takeBookInterface(commands);
                                break;
                            }
                        case 3:
                            {
                                ReturnBookInterface(commands);
                                break;
                            }
                        case 4:
                            {
                                commands.ListAllBooks();
                                break;
                            }
                        case 5:
                            {
                                Console.Write("Enter book name that you want to delete: ");
                                string bookName = Console.ReadLine();
                                commands.DeleteBook(bookName);
                                break;
                            }
                        default:
                            {
                                exit = true;
                                break;
                            }
                    }
                }
                catch(Exception e)
                {
                    Console.Clear();
                    Console.WriteLine(e.Message);
                } 
            }
        }

        /// <summary>
        /// Shows interface where user can select option how to filter books
        /// </summary>
        /// <returns>Number of selected option how user wants to sort books </returns>
        public static int filterInterface()
        {
            Console.WriteLine("\nSelect option how to filter books: (Type number of selected option)");
            Console.WriteLine("1. Filter by author");
            Console.WriteLine("2. Filter by category");
            Console.WriteLine("3. Filter by language");
            Console.WriteLine("4. Filter by ISBN");
            Console.WriteLine("5. Filter by name");
            Console.WriteLine("6. Filter taken or available books.");
            Console.WriteLine("7. Exit to main menu");
            int option = 0;
            try
            {
                option = int.Parse(Console.ReadLine());
                if (option < 1 || option > 7)
                {
                    throw new Exception();
                }
            }
            catch
            {
                Console.Clear();
                Console.WriteLine("Selected choice does not exist :( \n");
                
            }

            return option;
        }

        /// <summary>
        /// Interface for taking book
        /// </summary>
        /// <param name="commands">Commands class</param>
        public static void takeBookInterface(Commands commands)
        {
            Console.Clear();
            Console.Write("Enter your name: ");
            string UserName = Console.ReadLine();
            int avialabelBooksCount =  commands.GetUsersAvialableBooksCount(UserName);
            Console.WriteLine("You can take " + avialabelBooksCount + " more books \n");

            if (avialabelBooksCount > 0)
            {
                Dictionary<string, bool> usersTakenBooks = new Dictionary<string, bool>();
                Dictionary<string, string> usersTakeTime = new Dictionary<string, string>();

                while (avialabelBooksCount > 0)
                {
                    Console.Write("Enter book name: ");
                    string selectedBook = Console.ReadLine();

                    if (usersTakenBooks.ContainsKey(selectedBook))
                        throw new Exception("You can't take same book twice :( \n");

                    if (commands.BookExists(selectedBook))
                    {
                        string booksISBN = commands.GetBooksISBN(selectedBook);
                        usersTakenBooks.Add(booksISBN, true);
                        usersTakeTime.Add(booksISBN, DateTime.Now.ToString("yyyy/MM/dd"));
                    }
                    else
                        throw new Exception("Book does not exist or it is taken! \n");

                    string uesrAnswer = string.Empty;

                    if (avialabelBooksCount > 1)
                    {
                        Console.WriteLine("Do you want take more books? y/n: ");
                        uesrAnswer = Console.ReadLine().ToLower();
                        if (uesrAnswer == "n")
                            break;
                        else if (uesrAnswer != "y" && uesrAnswer != "n")
                            throw new Exception("Selected option does not exist :( \n");
                    }

                    avialabelBooksCount--;
                }

                Console.Write("Enter loan period in days (maximum 2 months): ");
                int period = int.Parse(Console.ReadLine());
                if (period < 0 || period > 61)
                    throw new Exception("There was an error in typing loan period! \n");

                LibaryUser user = new LibaryUser(UserName, usersTakenBooks, usersTakeTime);
                commands.ConvertUserToJson(user);
            }

        }

        /// <summary>
        /// Create book interface where user can type all new books parameters
        /// </summary>
        /// <returns>New book object</returns>
        public static Book createBook()
        {
       
            Console.WriteLine("New book");
            Console.Write("1. Name: ");
            string bookName = Console.ReadLine();
            Console.Write("2. Author: ");
            string bookAuthor = Console.ReadLine();
            Console.Write("3. Category: ");
            string bookCategory = Console.ReadLine();
            Console.Write("4. Language: ");
            string bookLanguage = Console.ReadLine();
            Console.Write("5. Publication date: ");
            string bookPublicationDate = Console.ReadLine();
            Console.Write("6. ISBN: ");
            string bookISBN = Console.ReadLine();

            if(bookName == string.Empty || bookAuthor == string.Empty || bookCategory == string.Empty || bookLanguage == string.Empty || bookISBN == string.Empty)
            {
                throw new Exception("All book description fields are required! \n");
            }

            else if (!DateTime.TryParse(bookPublicationDate, out DateTime temp))
            {
                throw new Exception("Date has to be in correct foramat! \n");
            }

            Book book = new Book(bookName, bookAuthor, bookCategory, bookLanguage, bookPublicationDate, bookISBN, false);

            return book;
        }

        /// <summary>
        /// Interface to return book
        /// </summary>
        /// <param name="commands">Commands class</param>
        public static void ReturnBookInterface(Commands commands)
        {
            Console.Clear();
            Console.Write("Enter your name: ");
            string UserName = Console.ReadLine();
            int usersBookCount = 3 - commands.GetUsersAvialableBooksCount(UserName);

            if (usersBookCount == 0)
                throw new Exception("You do not have any books to return \n");

            while (usersBookCount > 0)
            {
                Console.Write("Enter book name: ");
                string bookName = Console.ReadLine();
                commands.ReturnBook(bookName, UserName);

                if (usersBookCount > 1)
                {
                    Console.WriteLine("Do you want to return more books? y/n: ");
                    string uesrAnswer = Console.ReadLine().ToLower();
                    if (uesrAnswer == "n")
                        break;
                    else if (uesrAnswer != "y" && uesrAnswer != "n")
                        throw new Exception("Selected option does not exist :( \n");
                }

                usersBookCount--;
            }

            Console.Clear();
            Console.WriteLine("Books were returned! \n");

        }
    }
}
