using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.DTOs.Order
{
    public class BookForOrder
    {
        public string? BookId { get; set; }
        public string? Title { get; set; }
        public float Price { get; set; }
        public int Quantity { get; set; }

    }
}
