using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.DTOs.Book
{
    public class LibraryBook
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = "";
        public BookFormat Type { get; set; }
        public string? ImageUrl { get; set; }
        public string? AuthorFullName { get; set; }
    }
}
