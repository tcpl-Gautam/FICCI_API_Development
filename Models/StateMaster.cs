using Newtonsoft.Json;

namespace FICCI_API.Models
{
    public class StateMaster
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("description")]
        public string Name { get; set; }
    }

}
