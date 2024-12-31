using Library.DTOs.Book;
using System.ComponentModel.DataAnnotations;

namespace Client.Models.BookEntities.Book
{
    public class ManageBookViewModel
    {
        public Guid Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Title is required.")]
        [StringLength(255, ErrorMessage = "Title must be less than 255 characters.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Author ID is required.")]
        public Guid AuthorId { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        [Range(0, float.MaxValue, ErrorMessage = "Price must be a positive value.")]
        public float Price { get; set; }

        [Required(ErrorMessage = "Publisher ID is required.")]
        public Guid PublisherId { get; set; }

        [Required(ErrorMessage = "Language is required.")]
        public Language Language { get; set; }

        [Required(ErrorMessage = "Year is required.")]
        [DataType(DataType.Date, ErrorMessage = "Invalid date format.")]
        public DateTime Year { get; set; }

        [Required(ErrorMessage = "Category ID is required.")]
        public Guid CategoryId { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Description is required.")]
        [StringLength(1000, ErrorMessage = "Description must be less than 1000 characters.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Cover type is required.")]
        public CoverType Cover { get; set; }

        public bool IsAvailable { get; set; }

        public List<Guid> FeedbackIds { get; set; }

        public ManageBookViewModel()
        {
            Title = string.Empty;
            Description = string.Empty;
            FeedbackIds = new List<Guid>();
            IsAvailable = true;
            Year = DateTime.Now;
        }
    }

}
