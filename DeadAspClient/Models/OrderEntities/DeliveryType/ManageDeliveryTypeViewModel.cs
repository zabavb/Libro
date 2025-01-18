using System.ComponentModel.DataAnnotations;

namespace DeadAspClient.Models.OrderEntities.DeliveryType
{
    public class ManageDeliveryTypeViewModel
    {
        public Guid Id { get; set; }

        [Required(AllowEmptyStrings = false,ErrorMessage = "Service name is required.")]
        [StringLength(50, ErrorMessage = "Service name should be less than 50 characters.")]
        public string ServiceName { get; set; }

        public ManageDeliveryTypeViewModel()
        {
            ServiceName = string.Empty;
        }
    }
}
