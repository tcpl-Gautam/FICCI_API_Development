using Newtonsoft.Json;

namespace FICCI_API.Models
{
    public class ShiptoCustomer
    {
        [JsonProperty("CustomerNo")]
        public string? CustomerNo { get; set; }

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
        public string? PostCode { get; set; }

        [JsonProperty("State")]
        public string? State { get; set; }

        [JsonProperty("CountryRegionCode")]
        public string? CountryRegionCode { get; set; }

        [JsonProperty("GSTRegistrationNo")]
        public string? GSTRegistrationNo { get; set; }

        [JsonProperty("GSTCustomerType")]
        public string? GSTCustomerType { get; set; }

        [JsonProperty("PAN_No")]
        public string? PAN_No { get; set; }

        [JsonProperty("emial")]
        public string? emial { get; set; }

        [JsonProperty("PrimaryContactNo")]
        public string? PrimaryContactNo { get; set; }

        [JsonProperty("Code")]
        public string? Code { get; set; }
    }
}
