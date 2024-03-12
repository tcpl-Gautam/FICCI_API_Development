using Newtonsoft.Json;

namespace FICCI_API.Models
{
    public class ProjectModel
    {
        [JsonProperty("Dimension_Code")]
        public string? Dimension_Code { get; set; }

        [JsonProperty("Code")]
        public string? Code { get; set; }

        [JsonProperty("Name")]
        public string? Name { get; set; }

        [JsonProperty("DepartmentCode")]
        public string? DepartmentCode { get; set; }

        [JsonProperty("DepartmentName")]
        public string? DepartmentName { get; set; }

        [JsonProperty("DivisionCode")]
        public string? DivisionCode { get; set; }

        [JsonProperty("DivisionName")]
        public string? DivisionName { get; set; }

        [JsonProperty("TLApprover")]
        public string? TLApprover { get; set; }

        [JsonProperty("CHApprover")]
        public string? CHApprover { get; set; }

        [JsonProperty("FinanceApprover")]
        public string? FinanceApprover { get; set; }

        [JsonProperty("SupportApprover")]
        public string? SupportApprover { get; set; }

        [JsonProperty("StartDate")]
        public string? StartDate { get; set; }

        [JsonProperty("Enddate")]
        public string? Enddate { get; set; }
    }
}
