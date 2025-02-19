using Library.OrderEntities;
using Library.Validators;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace Client.Models.OrderEntities.Order
{
    public class ManageOrderViewModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "User id is required.")]
        public Guid UserId { get; set; }
        [Required(ErrorMessage = "Book ids are required.")]
        public List<Guid> BookIds { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Region is required.")]
        [StringLength(100, ErrorMessage = "Region must be less than 100 characters.")]
        public string Region { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "City is required.")]
        [StringLength(100, ErrorMessage = "City must be less than 100 characters.")]
        public string City { get; set; }
        [Required(AllowEmptyStrings = false,ErrorMessage = "Address is required.")]
        [StringLength(255, ErrorMessage = "Address must be less than 255 characters.")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        [Range(0, float.MaxValue)]
        public float Price { get; set; }

        [Required(ErrorMessage = "Delivery type is required.")]
        public Guid DeliveryTypeId { get; set; }

        [Required(ErrorMessage = "Delivery price is required.")]
        public float DeliveryPrice { get; set; }

        [Required(ErrorMessage = "Order date is required.")]
        [DataType(DataType.Date, ErrorMessage = "Invalid date format.")]
        public DateTime OrderDate { get; set; }

        [Required(ErrorMessage = "Delivery date is required")]
        [DataType(DataType.Date, ErrorMessage = "Invalid date format.")]
        [FutureDate(ErrorMessage = "Delivery date must be in the future.")]
        public DateTime DeliveryDate { get; set; }

        [Required(ErrorMessage = "Status is required.")]
        public OrderStatus Status { get; set; }

        public ManageOrderViewModel()
        {
            BookIds = new List<Guid>();
            Region = string.Empty;
            City = string.Empty;
            Address = string.Empty;
            OrderDate = DateTime.Now;
            Status = OrderStatus.PENDING;
        }

        public class FutureDateAttribute : ValidationAttribute
        {
            protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
            {
                if (value is DateTime dateTime && dateTime <= DateTime.Now)
                    return new ValidationResult(ErrorMessage ?? "Date must be in the future.");
                return ValidationResult.Success;
            }
        }
    }
}
