using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.DTOs.Book
{
    public class Author
    {
        public Guid AuthorId { get; set; }
        public string Name { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Biography { get; set; }
    }

}
