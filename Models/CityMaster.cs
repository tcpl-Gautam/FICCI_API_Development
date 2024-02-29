using FICCI_API.ModelsEF;
using Newtonsoft.Json;

namespace FICCI_API.Models
{
    public class CityMaster
    {

        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("city")]
        public string Name { get; set; }


        [JsonProperty("countryRegionCode")]
        public string CountyCode { get; set; }

        [JsonProperty("county")]
        public string County { get; set; }


    }
}
