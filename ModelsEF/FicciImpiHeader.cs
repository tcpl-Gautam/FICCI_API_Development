﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace FICCI_API.ModelsEF;

public partial class FicciImpiHeader
{
    public int ImpiHeaderId { get; set; }

    public string ImpiHeaderPiNo { get; set; }

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

    public bool ImpiHeaderActive { get; set; }

    public string ImpiHeaderCreatedBy { get; set; }

    public DateTime ImpiHeaderCreatedOn { get; set; }

    public string ImpiHeaderModifiedBy { get; set; }

    public DateTime? ImpiHeaderModifiedOn { get; set; }

    public string ImpiHeaderStatus { get; set; }

    public decimal? ImpiHeaderTotalInvoiceAmount { get; set; }

    public DateTime? ImpiHeaderSubmittedDate { get; set; }

    public string ImpiHeaderTlApprover { get; set; }

    public DateTime? ImpiHeaderTlApproverDate { get; set; }

    public string ImpiHeaderTlApproverRemarks { get; set; }

    public string ImpiHeaderClusterApprover { get; set; }

    public DateTime? ImpiHeaderClusterApproverDate { get; set; }

    public string ImpiHeaderClusterApproverRemarks { get; set; }

    public string ImpiHeaderFinanceApprover { get; set; }

    public DateTime? ImpiHeaderFinanceApproverDate { get; set; }

    public string ImpiHeaderFinanceRemarks { get; set; }

    public string ImpiHeaderSupportApprover { get; set; }

    public string ImpiHeaderRemarks { get; set; }

    public string ImpiHeaderCancelRemarks { get; set; }

    public string ImpiHeaderPaymentTerms { get; set; }

    public bool? IsDraft { get; set; }

    public string ImpiHeaderAttachment { get; set; }

    public int? HeaderStatusId { get; set; }

    public string AccountApprover { get; set; }

    public DateTime? AccountApproverDate { get; set; }

    public string AccountApproverRemarks { get; set; }

    public DateTime? ImpiCancelOn { get; set; }

    public string ImpiCancelBy { get; set; }

    public string ImpiHeaderRecordNo { get; set; }

    public bool? IsCancel { get; set; }

    public virtual ICollection<FicciImpiLine> FicciImpiLines { get; set; } = new List<FicciImpiLine>();
}