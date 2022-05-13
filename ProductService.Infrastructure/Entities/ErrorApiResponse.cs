using Newtonsoft.Json;

namespace ProductService.Infrastructure.Entities
{
    public class ErrorApiResponse
    {
        [JsonProperty("message")]
        public string Message { get; set; }
    }
}
