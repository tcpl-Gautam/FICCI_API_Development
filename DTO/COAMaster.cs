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

    public class COAInfo
    {
        public string No { get; set; }
        public string Name { get; set; }
    }
}
