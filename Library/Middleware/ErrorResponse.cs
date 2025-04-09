namespace Library.Common.Middleware
{
    public class ErrorResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public string? Details { get; set; }

        public ErrorResponse() => Message = string.Empty;
    }
}
