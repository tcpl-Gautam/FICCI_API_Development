namespace FICCI_API.Models
{
    public class Employee_Master
    {
        public int IMEM_ID { get; set; }
        public string IMEM_EmpId { get; set; }
        public string IMEM_Name { get; set; }
        public string IMEM_Email { get; set; }
        public string IMEM_Username { get; set; }
        public bool? IsActive { get; set; }
        public string RoleName { get; set; }


    }
    public class GetEmployee_MasterResponse
    {
        public bool status { get; set; }
        public string message { get; set; }
        public List<Employee_Master> data { get; set; }
        public GetEmployee_MasterResponse()
        {
            data = new List<Employee_Master>();
        }
    }

    public class PostUserMaster
    {
        public bool? IsUpdate { get; set; }
        public int IMEM_ID { get; set; }
        public string IMEM_EmpId { get; set; }
        public string IMEM_Username { get; set; }
        public string IMEM_Name { get; set; }
        public string IMEM_Email { get; set; }
        public int RoleId { get; set; }
        public bool IsActive { get; set; }
        public string?  RoleName { get; set; }
    }

}
