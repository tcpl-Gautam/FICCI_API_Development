using Newtonsoft.Json;

namespace FICCI_API.DTO
{
    public class GetLocation
    {
        [JsonProperty("code")]
        public string code { get; set; }

        [JsonProperty("name")]
        public string name { get; set; }
    }

    public class LoactionInfo
    {
        public string code { get; set; }

        public string name { get; set; }
    }
}
