using FICCI_API.ModelsEF;

namespace FICCI_API.Models
{
    public class PurchaseInvoice_New
    {
        public Boolean Status { get; set; }
        public string Message { get; set; }
        public List<PurchaseInvoice_Response> Data { get; set; }
        //public PurchaseInvoice_New()
        //{
        //    Data = new List<PurchaseInvoice_Response>();
        //}

    }
    public class PurchaseInvoice_Response
    {
        public int? HeaderId { get; set; }
        public string HeaderPiNo { get; set; }
        public string ImpiHeaderInvoiceType { get; set; }
       public string ImpiHeaderProjectCode { get; set; }

        public string ImpiHeaderProjectName { get; set; }

        public string ImpiHeaderProjectDepartmentCode { get; set; }

        public string ImpiHeaderProjectDepartmentName { get; set; }

        public string ImpiHeaderProjectDivisionCode { get; set; }

        public string ImpiHeaderProjectDivisionName { get; set; }

        public string ImpiHeaderPanNo { get; set; }

        public string ImpiHeaderGstNo { get; set; }
        public string ImpiHeaderCustomerName { get; set; }
        public string ImpiHeaderCustomerCode { get; set; }
        public string ImpiHeaderCustomerAddress { get; set; }
        public string ImpiHeaderCustomerCity { get; set; }
        public string ImpiHeaderCustomerState { get; set; }
        public string ImpiHeaderCustomerPinCode { get; set; }
        public string ImpiHeaderCustomerGstNo { get; set; }
        public string ImpiHeaderCustomerContactPerson { get; set; }
        public string ImpiHeaderCustomerEmailId { get; set; }
        public string ImpiHeaderCustomerPhoneNo { get; set; }
        public string ImpiHeaderCreatedBy { get; set; }
        public decimal? ImpiHeaderTotalInvoiceAmount { get; set; }
        public List<FicciImad> ImpiHeaderAttachment { get; set; }
        public string ImpiHeaderPaymentTerms { get; set; }
        public string ImpiHeaderRemarks { get; set; }
        public DateTime? ImpiHeaderSubmittedDate { get; set; }
        public DateTime? ImpiHeaderModifiedDate { get; set; }
        public string? HeaderStatus { get; set; }
        public bool? IsDraft { get; set; }
        public string ImpiHeaderTlApprover { get; set; }
        public string ImpiHeaderClusterApprover { get; set; }
        public string ImpiHeaderFinanceApprover { get; set; }
        public string? ImpiHeaderSupportApprover { get; set; }
        public string AccountApprover { get; set; }
        public string AccountApproverRemarks { get; set; }
        public string ImpiHeaderClusterApproverRemarks { get; set; }
        public string ImpiHeaderFinanceRemarks { get; set; }
        public string ImpiHeaderTlApproverRemarks { get; set; }
        public DateTime? AccountApproveDate { get; set; }
        public DateTime? TlApproveDate { get; set; }
        public DateTime? ClusterApproveDate { get; set; }
        public DateTime? FinanceApproveDate { get; set; }

        public int? HeaderStatusId { get; set; }
        public List<FicciImad> Attachments { get; set; }
        public List<FicciImwd>? WorkFlowHistory { get; set; }
        public List<LineItem_request> lineItem_Requests { get; set; }
    }
    

    public class PurchaseInvoice_Request
    {

        public bool isupdate { get; set; }
        public int? headerid { get; set; }
        public string ImpiHeaderInvoiceType { get; set; }
        public string ImpiHeaderProjectCode { get; set; }

        public string ImpiHeaderProjectName { get; set; }

        public string ImpiHeaderProjectDepartmentCode { get; set; }

        public string ImpiHeaderProjectDepartmentName { get; set; }

        public string ImpiHeaderProjectDivisionCode { get; set; }

        public string ImpiHeaderProjectDivisionName { get; set; }

        public string ImpiHeaderPanNo { get; set; }

        public string ImpiHeaderGstNo { get; set; }
        public string ImpiHeaderCustomerName { get; set; }
        public string ImpiHeaderCustomerCode { get; set; }
        public string ImpiHeaderCustomerAddress { get; set; }
        public string ImpiHeaderCustomerCity { get; set; }
        public string ImpiHeaderCustomerState { get; set; }
        public string ImpiHeaderCustomerPinCode { get; set; }
        public string ImpiHeaderCustomerGstNo { get; set; }
        public string ImpiHeaderCustomerContactPerson { get; set; }
        public string ImpiHeaderCustomerEmailId { get; set; }
        public string ImpiHeaderCustomerPhoneNo { get; set; }
        public string ImpiHeaderCreatedBy { get; set; }
        public decimal? ImpiHeaderTotalInvoiceAmount { get; set; }
        public List<IFormFile>? ImpiHeaderAttachment { get; set; }
        public string ImpiHeaderPaymentTerms { get; set; }
        public string ImpiHeaderRemarks { get; set; }
        public bool? IsDraft { get; set; }
        public string? ImpiHeaderPiNo { get; set; }
        public string LoginId { get; set; }
        public string? HeaderStatus { get; set; }
        public string ImpiHeaderTlApprover { get; set; }
        public string ImpiHeaderClusterApprover { get; set; }
        public string ImpiHeaderFinanceApprover { get; set; }
        public string? ImpiHeaderSupportApprover { get; set; }
        public string RoleName { get; set; }
        public string? InvoiceRemarks { get; set; }
        public List<LineItem_request> lineItem_Requests { get; set; }


    }
    public class LineItem_request
    {

        public string? ImpiLinePiNo { get; set; }

        public string? ImpiDocumentNo { get; set; }

        public string? ImpiLineNo { get; set; }

        public string? DocumentType { get; set; }

        public string? ImpiGlNo { get; set; }

        public string? ImpiLocationCode { get; set; }

        public string? ImpiQuantity { get; set; }

        public decimal? ImpiUnitPrice { get; set; }

        public decimal? ImpiLineAmount { get; set; }

        public string? ImpiGstgroupCode { get; set; }

        public string? ImpiGstgroupType { get; set; }

        public string? ImpiHsnsaccode { get; set; }

        public string? ImpiType { get; set; }

        public decimal? ImpiGstBaseAmount { get; set; }

        public decimal? ImpiNetTotal { get; set; }

        public decimal? ImpiTotalGstAmount { get; set; }


    }


    public class InvoiceList
    {
        public int? HeaderId { get; set; }

        public string HeaderPiNo { get; set; }
        public string ImpiHeaderInvoiceType { get; set; }
        public string ImpiHeaderProjectCode { get; set; }

        public string ImpiHeaderProjectName { get; set; }

        public string ImpiHeaderProjectDepartmentCode { get; set; }

        public string ImpiHeaderProjectDepartmentName { get; set; }

        public string ImpiHeaderProjectDivisionCode { get; set; }

        public string ImpiHeaderProjectDivisionName { get; set; }

        public string ImpiHeaderPanNo { get; set; }

        public string ImpiHeaderGstNo { get; set; }
        public string ImpiHeaderCustomerName { get; set; }
        public string ImpiHeaderCustomerCode { get; set; }
        public string ImpiHeaderCustomerAddress { get; set; }
        public string ImpiHeaderCustomerCity { get; set; }
        public string ImpiHeaderCustomerState { get; set; }
        public string ImpiHeaderCustomerPinCode { get; set; }
        public string ImpiHeaderCustomerGstNo { get; set; }
        public string ImpiHeaderCustomerContactPerson { get; set; }
        public string ImpiHeaderCustomerEmailId { get; set; }
        public string ImpiHeaderCustomerPhoneNo { get; set; }
        public string ImpiHeaderCreatedBy { get; set; }
     //   public decimal? ImpiHeaderTotalInvoiceAmount { get; set; }
      //  public string ImpiHeaderAttachment { get; set; }
       // public string ImpiHeaderPaymentTerms { get; set; }
       // public string ImpiHeaderRemarks { get; set; }
        //public DateTime? ImpiHeaderSubmittedDate { get; set; }
      //  public DateTime? ImpiHeaderModifiedDate { get; set; }
        public string? HeaderStatus { get; set; }
        public bool? IsDraft { get; set; }
        public string ImpiHeaderTlApprover { get; set; }
        public string ImpiHeaderClusterApprover { get; set; }
        public string ImpiHeaderFinanceApprover { get; set; }
        public string? ImpiHeaderSupportApprover { get; set; }

    }
}
