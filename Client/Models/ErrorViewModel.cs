namespace Client.Models
{
    public class ErrorViewModel
    {
        public string? Message { get; set; }
        public string? Details { get; set; }
        public bool HasMessage => !string.IsNullOrEmpty(Message);
    }
}