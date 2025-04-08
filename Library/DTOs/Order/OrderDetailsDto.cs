using Library.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.DTOs.Order
{
    public class OrderDetailsDto : Order
    {
        public CollectionSnippet<BookFormData> BookFormData { get; set; }
        public OrderDetailsDto() : base() { }
    }

    public class BookFormData
    {
        public Guid BookId {  get; set; }
        public string Title { get; set; }
        public BookFormData() => Title = string.Empty;
    }
}
