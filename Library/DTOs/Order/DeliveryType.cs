using System.ComponentModel.DataAnnotations;

namespace Library.DTOs.Order
{
    public class DeliveryType
    {
        [Required]
        [Key]
        public Guid Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(50, ErrorMessage = "Service name should be less than 50 characters.")]
        public string ServiceName { get; set; }

        public DeliveryType()
        {
            ServiceName = string.Empty;
        }
    }
}