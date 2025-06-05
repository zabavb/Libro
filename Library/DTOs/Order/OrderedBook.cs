using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.DTOs.Order
{
    public class OrderedBook
    {
        public Guid BookId { get; set; }
        public int Quantity { get; set; }

        public OrderedBook(Guid id, int quantity) {
            BookId = id;
            Quantity = quantity;
        }
    }
}
