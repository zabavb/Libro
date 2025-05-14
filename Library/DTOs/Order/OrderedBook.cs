using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.DTOs.Order
{
    public class OrderedBook
    {
        Guid BookId { get; set; }
        int Quatity { get; set; }

        public OrderedBook(int quatity) { 

        }
    }
}
