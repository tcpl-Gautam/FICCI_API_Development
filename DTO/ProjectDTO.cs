namespace FICCI_API.DTO
{
    public class ProjectDTO
    {
        // public int projectId { get; set; }
        public string dimension_Code { get; set; }

        public string code { get; set; }

        public string name { get; set; }

        public string? departmentCode { get; set; }

        public string? departmentName { get; set; }

        public string? divisionCode { get; set; }

        public string? divisionName { get; set; }

        public string? tlApprover { get; set; }

        public string? chApprover { get; set; }

        public string? financeApprover { get; set; }

        public string? supportApprover { get; set; }

        public string? startDate { get; set; }
        public string? endDate { get; set; }
    }
    public class AllProjectList
    {
        public int ProjectId { get; set; }
        public string ProjectCode { get; set; }
        public string ProjectName { get; set; }
        public string? Divison { get; set; }
        public string? Department { get; set; }
    }
}
