namespace UserAPI.Models.Filters
{
    public class BySubscription
    {
        public Guid Id { get; set; }
        public string Title { get; set; }

        public BySubscription() => Title = string.Empty;
    }
}