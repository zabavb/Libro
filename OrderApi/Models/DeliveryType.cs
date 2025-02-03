//using System.ComponentModel.DataAnnotations;

using System.ComponentModel.DataAnnotations;

namespace OrderApi.Models
{
    // One-to-many relation
    // One DeliveryType can be used in multiple Orders
    public class DeliveryType
    {
        [Key]
        public Guid DeliveryId { get; set; } // Primary key
        //[Required(AllowEmptyStrings = false)]
        //[StringLength(50, ErrorMessage = "Service name should be less than 50 characters.")]
        public string ServiceName { get; set; }

        public ICollection<Order> Orders { get; set; } = new List<Order>();

        public DeliveryType()
        {
            ServiceName = string.Empty;
        }
    }
}
