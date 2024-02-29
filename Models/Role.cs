namespace FICCI_API.Models
{
    public class Role
    {
        public int Role_id { get; set; }
        public string? RoleName { get; set; }
        public string IsActive { get; set; }

    }
    public class GetRoleResponse
    {
        public bool status { get; set; }
        public string message { get; set; }

        public List<Role> data { get; set;}

    }
}
