namespace BookAPI.Models
{
    public class Category
    {
        public Guid Id { get; set; } 
        public string Name { get; set; } = null!;

        public List<SubCategory> Subcategories { get; set; } = new();
        public Category()
        {
            Name = string.Empty;
            Subcategories = new List<SubCategory>();
        }
    }

}
