using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Library.DTOs.UserRelated.Subscription
{
    public class SubscriptionDto
    {
        [Required(ErrorMessage = "Id is required.")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Title cannot be less then 2 and exceed 30 characters.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Expiration days are required.")]
        public int ExpirationDays { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        [Range(0, 5000, ErrorMessage = "Price cannot be negative or exceed 5000.")]
        public float Price { get; set; }

        [StringLength(40, ErrorMessage = "Subdescription cannot exceed 50 characters.")]
        public string? Subdescription { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot exceed 50 characters.")]
        public string? Description { get; set; }

        public string? ImageUrl { get; set; }
        public IFormFile? Image { get; set; }

        public SubscriptionDto()
        {
            Id = Guid.NewGuid();
            Title = string.Empty;
            ImageUrl = string.Empty;
        }
    }
}