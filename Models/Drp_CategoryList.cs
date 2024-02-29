namespace FICCI_API.Models
{
    public class Drp_CategoryList
    {
        public Boolean status {  get; set; }
        public string message { get; set; }
        public List<Drp_CategoryListResponse> Data { get; set; }
        public Drp_CategoryList()
        {
            Data = new List<Drp_CategoryListResponse>();
        }
    }
    public class Drp_CategoryListResponse
    {
        public int Id { get; set; }
        public string Category_Name { get; set; }
    }
}
