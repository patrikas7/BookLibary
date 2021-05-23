using Newtonsoft.Json;
using System;

namespace BookLibary
{
    /// <summary>
    /// Book object
    /// </summary>
    public class Book
    {
        public string Name { get; set; }
        public string Author { get; set; }
        public string Category { get; set; }
        public string Language { get; set; }
        public string PublicationDate { get; set; }
        public string ISBN { get; set; }

        [JsonIgnore]
        public bool IsTaken { get; set; }

        public Book(string name, string author, string category, string language, string publicationDate, string iSBN, bool isTaken)
        {
            this.Name = name;
            this.Author = author;
            this.Category = category;
            this.Language = language;
            this.PublicationDate = publicationDate;
            this.ISBN = iSBN;
            this.IsTaken = isTaken;
        }

        public override string ToString()
        {
            return "Name: " + this.Name + "\nAuthor: " + this.Author + "\nCategory: " + this.Category + "\nLanguage: " +
                    this.Language + "\nPublication date: " + this.PublicationDate + "\nISBN: " + this.ISBN + "\n";
        }
    }
}
