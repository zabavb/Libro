using Amazon.S3.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.DTOs.Book
{
    public class BookInput
    {
        public string BookId { get; set; } = default!;
        public int Quantity { get; set; }
        public float Price { get; set; }
    }
}
