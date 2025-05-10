using Library.Sorts;
using OrderApi.Models;

namespace OrderAPI
{
    public class OrderSort : SortBase<Order>
    {
        public Bool OrderDate { get; set; }
        public Bool OrderPrice { get; set; }
        public Bool DeliveryDate { get; set; }
        //public Bool BooksAmount { get; set; } // Count of BookIds list
        //public Bool DeliveryPrice { get; set; }
        //public Bool StatusSort { get; set; } 

        public override IQueryable<Order> Apply(IQueryable<Order> query)
        {
            query = ApplySorting(query, OrderDate, o => o.OrderDate);
            query = ApplySorting(query, OrderPrice, o => o.Price);
            query = ApplySorting(query, DeliveryDate, o => o.DeliveryDate);
            //query = ApplySorting(query, BooksAmount, o => o.Books.Count);
            //query = ApplySorting(query, DeliveryPrice, o => o.DeliveryPrice);
            //query = ApplySorting(query, StatusSort, o => o.Status);

            return query;
        }
    }
}
