using Newtonsoft.Json;

namespace FICCI_API.DTO
{
    public class HSNSACMaster
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("GST_Group_Code")]
        public string GST_Group_Code { get; set; }
    }

    public class HSNSACMasterInfo
    {
        public string Code { get; set; }
        public string GST_Group_Code { get; set; }
    }
}
