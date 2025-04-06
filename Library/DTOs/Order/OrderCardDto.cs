using Library.Interfaces;

namespace Library.DTOs.Order
{
    public class OrderCardDto
    {
        public Guid OrderId { get; set; }
        public string Region { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public float FullPrice { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime DeliveryDate { get; set; }
        public OrderStatus Status { get; set; }
        public CollectionSnippet<BookCardSnippet> Books { get; set; }
        public SingleSnippet<DeliveryCardSnippet> Delivery { get; set; }

        public OrderCardDto()
        {
            Region = string.Empty;
            Address = string.Empty;
            City = string.Empty;
        }
    }

    public class BookCardSnippet
    {
        public Guid BookId { get; set; }
        public string Title { get; set; }
        public BookCardSnippet() => Title = string.Empty;
    }

    public class DeliveryCardSnippet
    {
        public Guid DeliveryId { get; set; }
        public string ServiceName { get; set; }
        public DeliveryCardSnippet() => ServiceName = string.Empty;
    }
}
