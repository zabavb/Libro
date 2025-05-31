using Library.DTOs.Book;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.DTOs.Order
{
    public class OrderDetails
    {
        public Guid OrderId { get; set; }
        public float Price { get; set; }
        public DateTime Created {  get; set; }
        public OrderStatus Status { get; set; }
        public List<BookOrderDetails> OrderBooks { get; set; }
    }

    public class BookOrderDetails
    {
        public Guid BookId { get; set; }
        public string Title { get; set; }
        public string AuthorName { get; set; }
        public string ImageUrl {  get; set; }
        public float Price { get; set; }
        public int Amount { get; set; }

        //public CoverType CoverType { get; set; }
    }
}
