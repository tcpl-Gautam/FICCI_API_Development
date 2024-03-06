using System.Text.Json.Serialization;

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

    public partial class PURCHASE_INVOICE_HEADER
    {

        //  public PURCHASE_INVOICE_LINE[] purchase_LineModels;      

        //[JsonPropertyName("Document_Type")]
        //public string Document_Type { get; set; }

        //  [JsonPropertyName("no")]
        //   public string no { get; set; }

        [JsonPropertyName("sellToCustomerName")]
        public string sellToCustomerName { get; set; }

        [JsonPropertyName("sellToCustomerName2")]
        public string sellToCustomerName2 { get; set; }

        [JsonPropertyName("sellToCustomerNo")]
        public string sellToCustomerNo { get; set; }

        [JsonPropertyName("ProjectCode")]
        public string ProjectCode { get; set; }
        [JsonPropertyName("DepartmentName")]
        public string DepartmentName { get; set; }

        [JsonPropertyName("DepartmentCode")]
        public string DepartmentCode { get; set; }

        [JsonPropertyName("DivisionCode")]
        public string DivisionCode { get; set; }

        [JsonPropertyName("DivisionName")]
        public string DivisionName { get; set; }

        [JsonPropertyName("ApproverTL")]
        public string ApproverTL { get; set; }

        [JsonPropertyName("ApproverCH")]
        public string ApproverCH { get; set; }

        [JsonPropertyName("ApproverSupport")]
        public string ApproverSupport { get; set; }

        [JsonPropertyName("FinanceApprover")]
        public string FinanceApprover { get; set; }

        [JsonPropertyName("InvoicePortalOrder")]
        public bool InvoicePortalOrder { get; set; }

        [JsonPropertyName("InvoicePortalSubmitted")]
        public bool InvoicePortalSubmitted { get; set; }

        [JsonPropertyName("sellToCity")]
        public string sellToCity { get; set; }

        [JsonPropertyName("sellToAddress")]

        public string sellToAddress { get; set; }

        [JsonPropertyName("sellToAddress2")]
        public string sellToAddress2 { get; set; }

        [JsonPropertyName("sellToPostCode")]
        public string sellToPostCode { get; set; }

        [JsonPropertyName("sellToCountryRegionCode")]
        public string sellToCountryRegionCode { get; set; }

        [JsonPropertyName("GST_No")]
        public string GST_No { get; set; }

        [JsonPropertyName("PAN_No")]
        public string PAN_No { get; set; }

    }


    public partial class PURCHASE_INVOICE_HEADER_RESPONSE
    {

        // public VMS_PURCHASE_LINE_RESPONSE[] purchaseinvoiceLineAPI;



        public string Document_Type { get; set; }

        public string no { get; set; }


        public string sellToCustomerName { get; set; }


        public string sellToCustomerName2 { get; set; }


        public string sellToCustomerNo { get; set; }


        public string ProjectCode { get; set; }

        public string DepartmentName { get; set; }

        public string DepartmentCode { get; set; }

        public string DivisionCode { get; set; }

        public string DivisionName { get; set; }

        public string ApproverTL { get; set; }

        public string ApproverCH { get; set; }

        public string ApproverSupport { get; set; }

        public string FinanceApprover { get; set; }


        public bool InvoicePortalOrder { get; set; }

        public bool InvoicePortalSubmitted { get; set; }

        public string sellToCity { get; set; }

        public string sellToAddress { get; set; }

        public string sellToAddress2 { get; set; }

        public string sellToPostCode { get; set; }

        public string sellToCountryRegionCode { get; set; }

        public string GST_No { get; set; }

        public string PAN_No { get; set; }


    }
    public partial class PURCHASE_INVOICE_LINE
    {

        [JsonPropertyName("documentType")]
        public string documentType { get; set; }

        [JsonPropertyName("documentNo")]
        public string documentNo { get; set; }

        [JsonPropertyName("type")]
        public string type { get; set; }

        [JsonPropertyName("no_")]
        public string no_ { get; set; }

        [JsonPropertyName("LocationCode")]
        public string LocationCode { get; set; }


        [JsonPropertyName("quantity")]
        public string quantity { get; set; }
        [JsonPropertyName("unitPrice")]

        public Nullable<decimal> unitPrice { get; set; }

        [JsonPropertyName("lineAmount")]
        public Nullable<decimal> lineAmount { get; set; }

        [JsonPropertyName("gSTGroupCode")]
        public string gSTGroupCode { get; set; }

        [JsonPropertyName("gST_Group_Type")]
        public string gST_Group_Type { get; set; }

        [JsonPropertyName("hSN_SAC_Code")]
        public string hSN_SAC_Code { get; set; }

        [JsonPropertyName("lineNo")]
        public long lineNo { get; set; }

        [JsonPropertyName("gSTCredit")]
        public decimal gSTCredit { get; set; }


    }

    public partial class VMS_PURCHASE_LINE_RESPONSE
    {

        public string documentNo { get; set; }
        public string documentType { get; set; }
        public string type { get; set; }
        public string no_ { get; set; }
        //public long lineNo { get; set; }
        public string LocationCode { get; set; }
        public int quantity { get; set; }
        public Nullable<decimal> unitPrice { get; set; }
        public Nullable<decimal> lineAmount { get; set; }
        //  public decimal gSTCredit { get; set; }
        public string gSTGroupCode { get; set; }
        public string gST_Group_Type { get; set; }
        public string hSN_SAC_Code { get; set; }
        public long lineNo { get; set; }
    }



    public class CancelEmployee
    {
        public string LoginId { get; set; }
        public string Remarks { get; set; }
        public int HeaderId { get; set; }
    }
}

