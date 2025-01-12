using System.ComponentModel.DataAnnotations;

namespace Client.Models.UserEntities.Subscription
{
    public class ManageSubscriptionViewModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        [StringLength(64, MinimumLength = 2, ErrorMessage = "Title must be between 2 and 64 characters.")]
        public string Title { get; set; }

        [StringLength(512, MinimumLength = 16, ErrorMessage = "Description must be between 16 and 512 characters.")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "End date is required.")]
        [DataType(DataType.DateTime, ErrorMessage = "Invalid date format.")]
        [FutureDate(ErrorMessage = "End date must be in the future.")]
        public DateTime EndDate { get; set; }

        public ManageSubscriptionViewModel()
        {
            Title = string.Empty;
            Description = null;
            EndDate = DateTime.Now.AddDays(1);
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
