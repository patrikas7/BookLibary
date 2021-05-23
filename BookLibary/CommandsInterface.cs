using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookLibary
{
    interface CommandsInterface<T>
    {
        void AddNewBook(T book);
        void ListAllBooks();
        void DeleteBook(string bookName);
        void ReturnBook(string bookName, string userName);
    }
}
