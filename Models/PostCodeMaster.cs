using Newtonsoft.Json;

namespace FICCI_API.Models
{
    public class PostCodeMaster
    {
        [JsonProperty("PostCode")]
        public string PostCode { get; set; }

        [JsonProperty("City")]
        public string City { get; set; }
    }
}
