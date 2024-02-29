using Newtonsoft.Json;

namespace FICCI_API.DTO
{
    public class COAMaster
    {
        [JsonProperty("no")]
        public string No { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
