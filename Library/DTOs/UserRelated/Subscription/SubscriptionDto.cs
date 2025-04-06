using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Library.DTOs.UserRelated.Subscription
{
    public class SubscriptionDto
    {
        [Required(ErrorMessage = "Id is required.")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        [StringLength(50, ErrorMessage = "Title cannot exceed 50 characters.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Expiration date are required.")]
        public int ExpirationDays { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        public float Price { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
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