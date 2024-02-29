namespace FICCI_API.DTO
{
    public class ApproveInvoice
    {
        public int HeaderId { get; set; }
        public bool IsApproved { get; set; }
        public string LoginId { get; set; }
        public int StatusId { get; set; }
        public string? Remarks { get; set; }
    }
    public class ApproverInvoice_Crud
    {
        public Boolean status { get; set; }
        public string message { get; set; }
        public List<ApproveInvoice> data { get; set; }
        public ApproverInvoice_Crud()
        {
            data = new List<ApproveInvoice>();
        }
    }
}
