
namespace OrderApi.Models
{
    // One-to-many relation
    // One DeliveryType can be used in multiple Orders
    public class DeliveryType
    {
        public Guid DeliveryId { get; set; } // Primary key

        public string ServiceName { get; set; }

        public ICollection<Order> Orders { get; set; } = new List<Order>();

        public DeliveryType()
        {
            ServiceName = string.Empty;
        }
    }
}
