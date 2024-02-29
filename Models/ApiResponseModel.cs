using System.Net;

namespace FICCI_API.Models
{
    public class ApiResponseModel
    {
        public string? Message { get; set; }
        public bool Status { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public object? Data { get; set; }
    }
}
