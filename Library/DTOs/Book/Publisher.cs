using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.DTOs.Book
{
    public class Publisher
    {
        public Guid PublisherId { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
    }

}
