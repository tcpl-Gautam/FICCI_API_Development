namespace FICCI_API.DTO
{
    public class ApproveCustomer
    {
        public int CustomerId { get; set; }
        public bool IsApproved { get; set; }
        public string LoginId { get; set; }
        public int StatusId { get; set; }
        public string? Remarks { get; set; }
    }
    public class ApproverCustomer_Crud
    {
        public Boolean status { get; set; }
        public string message { get; set; }
        public List<ApproveCustomer> data { get; set; }
        public ApproverCustomer_Crud()
        {
            data = new List<ApproveCustomer>();
        }
    }
}
