using FICCI_API.DTO;
using Newtonsoft.Json;

namespace FICCI_API.Models
{
    public class CustomerModel
    {
        [JsonProperty("No")]
        public string? No { get; set; }

        [JsonProperty("Name")]
        public string? Name { get; set; }

        [JsonProperty("Name2")]
        public string? Name2 { get; set; }

        [JsonProperty("Address")]
        public string? Address { get; set; }

        [JsonProperty("Address2")]
        public string? Address2 { get; set; }

        [JsonProperty("City")]
        public string? City { get; set; }

        [JsonProperty("Contact")]
        public string? Contact { get; set; }

        [JsonProperty("PostCode")]
        public string? PinCode { get; set; }

        [JsonProperty("State_Code")]
        public string? State_Code { get; set; }

        [JsonProperty("Country_Region_Code")]
        public string? Country_Region_Code { get; set; }

        [JsonProperty("GSTRegistrationNo")]
        public string? GSTRegistrationNo { get; set; }

        [JsonProperty("GSTCustomerType")]
        public string? GSTCustomerType { get; set; }

        [JsonProperty("PAN_No")]
        public string? PAN_No { get; set; }

        [JsonProperty("EMail")]
        public string? EMail { get; set; }

        [JsonProperty("PrimaryContactNo")]
        public string? PrimaryContactNo { get; set; }
       
    }
}
