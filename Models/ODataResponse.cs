namespace FICCI_API.Models
{
    public class ODataResponse<T>
    {
        public List<T> Value { get; set; }
    }
}
