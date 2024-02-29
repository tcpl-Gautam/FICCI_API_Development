namespace FICCI_API.Models
{
    public class Configuration_CRUD
    {
        public Boolean status { get; set; }
        public string message { get; set; }
        public List<Configuration_List> Data { get; set; }
        public Configuration_CRUD()
        {
            Data = new List<Configuration_List>();
        }
    }
    public class Configuration_List
    {
        public int C_ID { get; set; }
        public string C_Code { get; set; }
        public string C_Value { get; set; }
        public string Category_Name { get; set; }
        public bool IsActive { get; set; }
    }

    public class Configuration_Post_Request
    {
        
        public bool? IsUpdate { get; set; }
        public int? C_ID { get; set; }
        public string C_Code { get; set; }
        public string C_Value { get; set; }
        public int? CategoryID { get; set; }
        public string User { get; set; }
        public bool? Isactive { get; set; }
    }
}

