using Newtonsoft.Json;

namespace FICCI_API.Models
{
    public class EmployeeModel
    {
        [JsonProperty("No")]
        public string? No { get; set; }

        [JsonProperty("FirstName")]
        public string? FirstName { get; set; }

        [JsonProperty("LastName")]
        public string? LastName { get; set; }


        [JsonProperty("Gender")]
        public string? Gender { get; set; }


        [JsonProperty("BirthDate")]
        public string? BirthDate { get; set; }
    }
}
