using Library.Validators;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.DTOs.Order
{
    public class Order
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public Dictionary<Guid,int> Books { get; set; }

        public string Region { get; set; }

        public string City { get; set; }

        public string Address { get; set; }

        public float Price { get; set; }

        public Guid DeliveryTypeId { get; set; }

        public float DeliveryPrice { get; set; }

        public DateTime OrderDate { get; set; }

        public DateTime DeliveryDate { get; set; }

        public OrderStatus Status { get; set; }

        public Order()
        {
            Books = new Dictionary<Guid, int>();
            Region = string.Empty;
            City = string.Empty;
            Address = string.Empty;
            OrderDate = DateTime.Now;
            Status = OrderStatus.PENDING;
        }
    }
}
