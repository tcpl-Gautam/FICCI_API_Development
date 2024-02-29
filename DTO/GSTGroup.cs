using Newtonsoft.Json;

namespace FICCI_API.DTO
{
    public class GSTGroup
    {

        [JsonProperty("Code")]
        public string Code { get; set; }
    }

    public class GSTGroupInfo
    {
        public string Code { get; set; }

    }
}
