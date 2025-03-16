namespace Library.DTOs.Book
{
    public class Discount
    {
        public Guid DiscountId { get; set; }

        public Guid BookId { get; set; }

        public float DiscountRate { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
