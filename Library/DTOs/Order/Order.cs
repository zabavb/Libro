using Library.Validators;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.DTOs.Order
{
    public class Order
    {
        [Required]
        [Key]
        public Guid Id { get; set; }
        [Required]
        public Guid UserId { get; set; }
        [Required]
        public List<Guid> BookIds { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(100, ErrorMessage = "Region must be less than 100 characters.")]
        public string Region { get; set; }
        [Required(AllowEmptyStrings = false)]
        [StringLength(100, ErrorMessage = "City must be less than 100 characters.")]
        public string City { get; set; }
        [Required(AllowEmptyStrings = false)]
        [StringLength(255, ErrorMessage = "Address must be less than 255 characters.")]
        public string Address { get; set; }

        [Required]
        [Range(0, float.MaxValue)]
        public float Price { get; set; }

        [Required]
        [ForeignKey(nameof(DeliveryType))]
        public Guid DeliveryTypeId { get; set; }

        [Required]
        [Range(0, float.MaxValue)]
        public float DeliveryPrice { get; set; }

        public DateTime OrderDate { get; set; }
        [Required]
        public DateTime DeliveryDate { get; set; }

        [EnumRange(typeof(OrderStatus))]
        public OrderStatus Status { get; set; }

        public Order()
        {
            BookIds = new List<Guid>();
            Region = string.Empty;
            City = string.Empty;
            Address = string.Empty;
            OrderDate = DateTime.Now;
            Status = OrderStatus.PENDING;
        }
    }
}
