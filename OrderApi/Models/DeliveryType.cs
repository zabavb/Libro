using System.ComponentModel.DataAnnotations;

namespace OrderApi.Models
{
    public class DeliveryType
    {
        [Required]
        [Key]
        public Guid DeliveryId { get; set; }
        [Required(AllowEmptyStrings = false)]
        [StringLength(50, ErrorMessage = "Service name should be less than 50 characters.")]
        public string ServiceName { get; set; }

        public DeliveryType()
        {
            ServiceName = string.Empty;
        }
    }
}
