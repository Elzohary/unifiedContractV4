namespace UnifiedContract.API.Models
{
    public class ApiResponse<T>
    {
        public T? Data { get; set; }
        public string? Message { get; set; }
        public int Status { get; set; }
        public string? Timestamp { get; set; }
        public string? ErrorCode { get; set; }
        public Dictionary<string, string[]>? ValidationErrors { get; set; }
    }
} 