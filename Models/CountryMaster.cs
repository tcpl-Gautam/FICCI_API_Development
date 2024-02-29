using Newtonsoft.Json;

namespace FICCI_API.Models
{
    public class CountryMaster
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

    }
}
