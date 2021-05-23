using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookLibary
{
    /// <summary>
    /// Libary user object
    /// </summary>
    public class LibaryUser
    {
        public string Name { get; set; }
        public Dictionary<string, bool> TakenBooks { get; set; }
        public Dictionary<string, string> TakenBookTime { get; set; }

        public LibaryUser(string name, Dictionary<string, bool> takenBooks, Dictionary<string, string> takenBookTime)
        {
            Name = name;
            TakenBooks = takenBooks;
            TakenBookTime = takenBookTime;
        }
    }
}
