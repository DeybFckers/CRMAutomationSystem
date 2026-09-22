namespace CRMSystem.Models.Responses
{
    public class ErrorResponse
    {
        public bool Success { get; set; } = false;
        public string Message { get; set; } = null!;
        public object? Data { get; set; }
    }
}
