namespace FICCI_API.DTO
{
    public class PurchaseInvoiceDTO
    {
        //public int HeaderId { get; set; }
        public string? HeaderPiNo { get; set; }
        public string InvoiceType { get; set; }
        public string ProjectCode { get; set; }
        public string Department { get; set; }
        public string Division { get; set; }
        public string GST { get; set; }
        public string PAN { get; set; }
        public string CustomerName { get; set; }
        public string CustomerCode { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Pincode { get; set; }
        public string CustomerGST { get; set; }
        public string ContactPerson { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string PaymentTerms { get; set; }
        public string Remarks { get; set; }
       // public IFormFile Attachment { get; set; }
        public List<LineItem> LineItems { get; set; }
    }
    public class LineItem
    {
        //public int Lineid { get; set; }
        public string? LinePiNo { get; set; }
        public string Description { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Discount { get; set; }
        public decimal Amount { get; set; }

    }
}
