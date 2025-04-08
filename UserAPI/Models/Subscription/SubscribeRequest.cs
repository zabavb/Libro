using System.ComponentModel.DataAnnotations;

namespace UserAPI.Models.Subscription
{
    public class SubscribeRequest
    {
        [Required(ErrorMessage = "UserId is required.")]
        [NotEmptyGuid(ErrorMessage = "UserId must not be empty.")]
        public Guid UserId { get; set; }

        [Required(ErrorMessage = "SubscriptionId is required.")]
        [NotEmptyGuid(ErrorMessage = "SubscriptionId must not be empty.")]
        public Guid SubscriptionId { get; set; }
    }

    internal class NotEmptyGuidAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value) =>
            value is Guid guid && guid != Guid.Empty;
    }
}