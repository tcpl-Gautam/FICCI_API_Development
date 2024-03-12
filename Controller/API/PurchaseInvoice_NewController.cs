using Azure.Core;
using FICCI_API.DTO;
using FICCI_API.Models;
using FICCI_API.ModelsEF;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection.PortableExecutable;

namespace FICCI_API.Controller.API
{

    [Route("api/[controller]")]
    [ApiController]
    public class PurchaseInvoice_NewController : BaseController
    {
        private readonly FICCI_DB_APPLICATIONSContext _dbContext;
        private readonly IConfiguration _configuration;

        private readonly MySettings _mySettings;
        public PurchaseInvoice_NewController(FICCI_DB_APPLICATIONSContext dbContext, IConfiguration configuration, IOptions<MySettings> mySettings) : base(dbContext)
        {
            _dbContext = dbContext;
            _configuration = configuration;
            _mySettings = mySettings.Value;
        }
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> POST([FromForm] PurchaseInvoice_Request request)
        {
            PurchaseInvoice_New purchaseInvoice_New = new PurchaseInvoice_New();
            try
            {
                if (request != null)
                {
                    if (!request.isupdate)
                    {


                        FicciImpiHeader ficciImpiHeader = new FicciImpiHeader();
                        ficciImpiHeader.ImpiHeaderInvoiceType = request.ImpiHeaderInvoiceType;
                        ficciImpiHeader.ImpiHeaderPiNo = request.ImpiHeaderPiNo;
                        ficciImpiHeader.ImpiHeaderProjectCode = request.ImpiHeaderProjectCode;
                        ficciImpiHeader.ImpiHeaderProjectName = request.ImpiHeaderProjectName;
                        ficciImpiHeader.ImpiHeaderProjectDepartmentCode = request.ImpiHeaderProjectDepartmentCode;
                        ficciImpiHeader.ImpiHeaderProjectDepartmentName = request.ImpiHeaderProjectDepartmentName;
                        ficciImpiHeader.ImpiHeaderProjectDivisionCode = request.ImpiHeaderProjectDivisionCode;
                        ficciImpiHeader.ImpiHeaderProjectDivisionName = request.ImpiHeaderProjectDivisionName;
                        ficciImpiHeader.ImpiHeaderPanNo = request.ImpiHeaderPanNo;
                        ficciImpiHeader.ImpiHeaderGstNo = request.ImpiHeaderGstNo;
                        ficciImpiHeader.ImpiHeaderCustomerName = request.ImpiHeaderCustomerName;
                        ficciImpiHeader.ImpiHeaderCustomerCode = request.ImpiHeaderCustomerCode;
                        ficciImpiHeader.ImpiHeaderCustomerAddress = request.ImpiHeaderCustomerAddress;
                        ficciImpiHeader.ImpiHeaderCustomerCity = request.ImpiHeaderCustomerCity;
                        ficciImpiHeader.ImpiHeaderCustomerState = request.ImpiHeaderCustomerState;
                        ficciImpiHeader.ImpiHeaderCustomerPinCode = request.ImpiHeaderCustomerPinCode;
                        ficciImpiHeader.ImpiHeaderCustomerGstNo = request.ImpiHeaderCustomerGstNo;
                        ficciImpiHeader.ImpiHeaderCustomerContactPerson = request.ImpiHeaderCustomerContactPerson;
                        ficciImpiHeader.ImpiHeaderCustomerEmailId = request.ImpiHeaderCustomerEmailId;
                        ficciImpiHeader.ImpiHeaderCustomerPhoneNo = request.ImpiHeaderCustomerPhoneNo;
                        ficciImpiHeader.ImpiHeaderCreatedBy = request.LoginId;
                        ficciImpiHeader.ImpiHeaderCreatedOn = DateTime.Now;
                        ficciImpiHeader.ImpiHeaderActive = true;
                        ficciImpiHeader.ImpiHeaderTotalInvoiceAmount = request.ImpiHeaderTotalInvoiceAmount;
                       
                        ficciImpiHeader.ImpiHeaderPaymentTerms = request.ImpiHeaderPaymentTerms;
                        ficciImpiHeader.ImpiHeaderRemarks = request.ImpiHeaderRemarks;
                        ficciImpiHeader.ImpiHeaderStatus = request.IsDraft == true ? "Draft" : "Pending";
                        ficciImpiHeader.IsDraft = request.IsDraft;

                        ficciImpiHeader.ImpiHeaderSubmittedDate = DateTime.Now;
                        ficciImpiHeader.ImpiHeaderTlApprover = "amit.jha@teamcomputers.com";//request.ImpiHeaderTlApprover + "@ficci.com";
                        ficciImpiHeader.ImpiHeaderClusterApprover = "debananda.panda@teamcomputers.com";//request.ImpiHeaderClusterApprover + "@ficci.com";
                        ficciImpiHeader.ImpiHeaderFinanceApprover = "gautam.v@teamcomputers.com";//request.ImpiHeaderFinanceApprover + "@ficci.com";
                        ficciImpiHeader.ImpiHeaderTlApproverDate = null;
                        ficciImpiHeader.ImpiHeaderClusterApproverDate = null;
                        ficciImpiHeader.ImpiHeaderFinanceApproverDate = null;
                        ficciImpiHeader.AccountApproverDate = null;
                        ficciImpiHeader.ProjectStartDate = request.startDate;
                        ficciImpiHeader.ProjectEndDate = request.endDate;
                        if (request.ImpiHeaderSupportApprover != null)
                        {
                            ficciImpiHeader.ImpiHeaderSupportApprover = request.ImpiHeaderSupportApprover + "@ficci.com";
                        }
                        ficciImpiHeader.HeaderStatusId = request.IsDraft == true ? 1 : 2;

                        _dbContext.Add(ficciImpiHeader);
                        _dbContext.SaveChanges();
                        int returnid = ficciImpiHeader.ImpiHeaderId;
                        string documentNumber = "";
                        string folder = ficciImpiHeader.ImpiHeaderRecordNo;

                        if (request.ImpiHeaderAttachment != null)
                        {
                            ficciImpiHeader.ImpiHeaderAttachment = UploadFile(request.ImpiHeaderAttachment, request.LoginId, returnid, folder.Trim(), 2,"Invoice_Header","Invoice");
                        }

                        FicciImwd imwd = new FicciImwd();
                        imwd.ImwdScreenName = "Invoice Approver";
                        imwd.CustomerId = returnid;
                        imwd.ImwdCreatedOn = DateTime.Now;
                        imwd.ImwdCreatedBy = request.LoginId;
                        if(request.IsDraft == false)
                        {
                            imwd.ImwdPendingEmailAt = _dbContext.FicciImpiHeaders.Where(x => x.ImpiHeaderId == returnid && x.HeaderStatusId == 2).Select(x => x.ImpiHeaderTlApprover).FirstOrDefault().ToString();
                        }
                        imwd.ImwdStatus = request.IsDraft == true ? "1" : "2";
                        imwd.ImwdPendingAt = _dbContext.StatusMasters.Where(x => x.StatusId == ficciImpiHeader.HeaderStatusId).Select(a => a.StatusName).FirstOrDefault();
                        imwd.ImwdInitiatedBy = request.LoginId;
                        imwd.ImwdRemarks = request.InvoiceRemarks;
                        imwd.ImwdRole = request.RoleName;
                        imwd.ImwdType = 2;
                        _dbContext.Add(imwd);

                        _dbContext.SaveChanges();
                        if (returnid != 0 && request.lineItem_Requests.Count > 0)
                        {
                            foreach (var k in request.lineItem_Requests)
                            {
                                FicciImpiLine FicciImpiLine = new FicciImpiLine();
                                FicciImpiLine.DocumentType = k.DocumentType;
                                FicciImpiLine.ImpiDocumentNo = k.ImpiDocumentNo;
                                FicciImpiLine.ImpiGlNo = k.ImpiGlNo;
                                FicciImpiLine.ImpiGstBaseAmount = k.ImpiGstBaseAmount;
                                FicciImpiLine.ImpiLineAmount = k.ImpiLineAmount;
                                FicciImpiLine.ImpiLineActive = true;
                                FicciImpiLine.ImpiLineCreatedBy = request.LoginId;
                                FicciImpiLine.ImpiLineCreatedOn = DateTime.Now;
                                FicciImpiLine.PiHeaderId = returnid;
                                FicciImpiLine.IsDeleted = false;
                                FicciImpiLine.ImpiLinePiNo = DateTime.Now.ToString("yyyyMMddhhmmss");
                                FicciImpiLine.ImpiTotalGstAmount = k.ImpiTotalGstAmount;
                                FicciImpiLine.ImpiNetTotal = k.ImpiNetTotal;
                                FicciImpiLine.ImpiLocationCode = k.ImpiLocationCode;
                                FicciImpiLine.ImpiQuantity = k.ImpiQuantity;
                                FicciImpiLine.ImpiUnitPrice = k.ImpiUnitPrice;
                                FicciImpiLine.ImpiGstgroupCode = k.ImpiGstgroupCode;
                                FicciImpiLine.ImpiGstgroupType = k.ImpiGstgroupType;
                                FicciImpiLine.ImpiHsnsaccode = k.ImpiHsnsaccode;
                                FicciImpiLine.ImpiLineNo = k.ImpiLineNo;
                                FicciImpiLine.ImpiLinePiNo = k.ImpiLinePiNo;
                                FicciImpiLine.ImpiType = k.ImpiType;
                                _dbContext.Add(FicciImpiLine);
                                _dbContext.SaveChanges();


                               
                            }

                        }

                        if (request.IsDraft == false)
                        {
                            string htmlbody = InvoiceAssignhtmlBody(_mySettings.Website_link, ficciImpiHeader.ImpiHeaderCustomerName, ficciImpiHeader.ImpiHeaderCustomerCity, ficciImpiHeader.ImpiHeaderPanNo, ficciImpiHeader.ImpiHeaderCustomerGstNo, ficciImpiHeader.ImpiHeaderCustomerContactPerson, ficciImpiHeader.ImpiHeaderCustomerPhoneNo, ficciImpiHeader.ImpiHeaderProjectCode, ficciImpiHeader.ImpiHeaderProjectName, ficciImpiHeader.ImpiHeaderCustomerCode);
                            SendEmail(ficciImpiHeader.ImpiHeaderTlApprover, ficciImpiHeader.ImpiHeaderCustomerEmailId, $"New PI Assigned for Approval : {ficciImpiHeader.ImpiHeaderCustomerName}", htmlbody, _mySettings);

                        }



                        if (ficciImpiHeader.ImpiHeaderId != 0 && ficciImpiHeader.ImpiHeaderPiNo == null)
                        {
                            NavERPController navERPController = new NavERPController(_configuration, _dbContext);

                            PURCHASE_INVOICE_HEADER PostData = new PURCHASE_INVOICE_HEADER();

                            PostData.sellToCustomerNo = request.ImpiHeaderCustomerCode;
                            PostData.sellToCustomerName = request.ImpiHeaderCustomerName;
                            //  PostData.sellToCustomerName2 = request.cus;
                            PostData.sellToCity = request.ImpiHeaderCustomerCity;
                            PostData.sellToAddress = request.ImpiHeaderCustomerAddress;
                            PostData.sellToAddress2 = request.ImpiHeaderCustomerAddress;
                            PostData.sellToPostCode = request.ImpiHeaderCustomerPinCode;
                            //PostData.sellToCountryRegionCode = request.cpi;
                            PostData.GST_No = request.ImpiHeaderCustomerGstNo;
                            PostData.PAN_No = request.ImpiHeaderPanNo;

                            PostData.DepartmentCode = request.ImpiHeaderProjectDepartmentCode;
                            PostData.DepartmentName = request.ImpiHeaderProjectDepartmentName;
                            PostData.ProjectCode = request.ImpiHeaderProjectCode;

                            PostData.DivisionCode = request.ImpiHeaderProjectDivisionCode;
                            PostData.DivisionName = request.ImpiHeaderProjectDivisionName;
                            PostData.ApproverTL = request.ImpiHeaderTlApprover;
                            PostData.ApproverCH = request.ImpiHeaderClusterApprover;
                            PostData.ApproverSupport = request.ImpiHeaderSupportApprover;
                            PostData.FinanceApprover = request.ImpiHeaderFinanceApprover;
                            PostData.InvoicePortalOrder = false;
                            PostData.InvoicePortalSubmitted = false;
                            PostData.Cancelled = false;
                            PostData.CancelRemark = request.InvoiceRemarks;
                            PostData.CreatedByUser = "";

                            dynamic erpResponse = await navERPController.PostPIData(PostData);
                            var updatedNumber = erpResponse.Value.Data.no;
                            var resu = _dbContext.FicciImpiHeaders.Where(x => x.ImpiHeaderId == ficciImpiHeader.ImpiHeaderId).FirstOrDefault();
                            resu.ImpiHeaderPiNo = updatedNumber;
                            _dbContext.Update(resu);
                            _dbContext.SaveChanges();



                            if (returnid != 0 && resu.ImpiHeaderPiNo != null && request.lineItem_Requests.Count > 0)
                            {

                                //  PURCHASE_INVOICE_LINE[] PostLineDataList = new PURCHASE_INVOICE_LINE[request.lineItem_Requests.Count];

                                int index = 0;
                                long LineNo = 0;
                                foreach (var line in request.lineItem_Requests)
                                {
                                    NavERPController navERPControllerLine = new NavERPController(_configuration, _dbContext);

                                    PURCHASE_INVOICE_LINE PostLineData = new PURCHASE_INVOICE_LINE();

                                    if (LineNo <= 0)
                                    {
                                        LineNo = 10000;
                                    }
                                    else
                                    {
                                        LineNo = Convert.ToInt32(LineNo) + 10000;
                                    }

                                    PostLineData.documentNo = resu.ImpiHeaderPiNo;
                                    PostLineData.documentType = "Invoice";
                                    PostLineData.type = "G/L Account";
                                    PostLineData.lineNo = LineNo;    // Convert.ToInt32(line.ImpiLineNo);
                                    PostLineData.quantity = Convert.ToInt32(line.ImpiQuantity);
                                    PostLineData.unitPrice = line.ImpiUnitPrice;
                                    PostLineData.hSN_SAC_Code = line.ImpiHsnsaccode;
                                    PostLineData.no_ = line.ImpiGlNo;
                                    PostLineData.gSTGroupCode = line.ImpiGstgroupCode;
                                    PostLineData.LocationCode = line.ImpiLocationCode;
                                    PostLineData.lineAmount = line.ImpiLineAmount;
                                   // PostLineData.gST_Group_Type = "Goods";

                                    dynamic erpResponse1 = await navERPController.PostPILineData(PostLineData);

                                    ////   PostLineDataList[index] = PostLineData;
                                    //    index++;
                                    //  var updatedNumber = erpResponse1.Data.No;
                                    // var resu = _dbContext.FicciImpiHeaders.Where(x => x.ImpiHeaderId == returnId).FirstOrDefault();
                                    // resu.ImpiHeaderPiNo = updatedNumber;
                                    //   _dbContext.Update(resu);
                                    // _dbContext.SaveChanges();

                                }

                                dynamic erpResponse2 = await navERPController.UploadFileInERP(resu.ImpiHeaderPiNo,resu.ImpiHeaderRecordNo);

                            }

                        }
                        purchaseInvoice_New.Status = true;
                        purchaseInvoice_New.Message = "Purchase Invoice Submit Successfully";
                        return StatusCode(200, purchaseInvoice_New);
                    }
                    else
                    {
                        var data = await _dbContext.FicciImpiHeaders.Where(m => m.ImpiHeaderId == request.headerid).FirstOrDefaultAsync();
                        if (data != null)
                        {
                            //FicciImpiHeader ficciImpiHeader = new FicciImpiHeader();
                            data.ImpiHeaderInvoiceType = request.ImpiHeaderInvoiceType;
                            data.ImpiHeaderProjectCode = request.ImpiHeaderProjectCode;
                            data.ImpiHeaderProjectCode = request.ImpiHeaderProjectCode;
                            data.ImpiHeaderProjectName = request.ImpiHeaderProjectName;
                            data.ImpiHeaderProjectDepartmentCode = request.ImpiHeaderProjectDepartmentCode;
                            data.ImpiHeaderProjectDepartmentName = request.ImpiHeaderProjectDepartmentName;
                            data.ImpiHeaderProjectDivisionCode = request.ImpiHeaderProjectDivisionCode;
                            data.ImpiHeaderProjectDivisionName = request.ImpiHeaderProjectDivisionName;
                            data.ImpiHeaderPanNo = request.ImpiHeaderPanNo;
                            data.ImpiHeaderGstNo = request.ImpiHeaderGstNo;
                            data.ImpiHeaderCustomerName = request.ImpiHeaderCustomerName;
                            data.ImpiHeaderCustomerCode = request.ImpiHeaderCustomerCode;
                            data.ImpiHeaderCustomerAddress = request.ImpiHeaderCustomerAddress;
                            data.ImpiHeaderCustomerCity = request.ImpiHeaderCustomerCity;
                            data.ImpiHeaderCustomerState = request.ImpiHeaderCustomerState;
                            data.ImpiHeaderCustomerPinCode = request.ImpiHeaderCustomerPinCode;
                            data.ImpiHeaderCustomerGstNo = request.ImpiHeaderCustomerGstNo;
                            data.ImpiHeaderCustomerContactPerson = request.ImpiHeaderCustomerContactPerson;
                            data.ImpiHeaderCustomerEmailId = request.ImpiHeaderCustomerEmailId;
                            data.ImpiHeaderCustomerPhoneNo = request.ImpiHeaderCustomerPhoneNo;
                            data.ImpiHeaderModifiedBy = request.ImpiHeaderCreatedBy;
                            data.ImpiHeaderModifiedOn = DateTime.Now;
                            //data.ImpiHeaderActive = true;
                            data.ImpiHeaderTotalInvoiceAmount = request.ImpiHeaderTotalInvoiceAmount;
                            
                            data.ImpiHeaderPaymentTerms = request.ImpiHeaderPaymentTerms;
                            data.ImpiHeaderRemarks = request.ImpiHeaderRemarks;
                            data.ImpiHeaderStatus = request.IsDraft == true ? "Draft" : "Pending";
                            data.IsDraft = request.IsDraft;
                            data.HeaderStatusId = request.IsDraft == true ? 1 : 2;
                            data.ImpiHeaderTlApproverDate = null;
                            data.ImpiHeaderClusterApproverDate = null;
                            data.ImpiHeaderFinanceApproverDate = null;
                            data.AccountApproverDate = null;
                            data.ProjectStartDate = request.startDate;
                            data.ProjectEndDate = request.endDate;

                            //_dbContext.Add(data);
                            _dbContext.SaveChanges();
                            int returnid = data.ImpiHeaderId;
                            string folder = data.ImpiHeaderRecordNo;

                            if (request.ImpiHeaderAttachment != null)
                            {
                                data.ImpiHeaderAttachment = UploadFile(request.ImpiHeaderAttachment, request.LoginId, returnid, folder.Trim(), 2, "Invoice_Header", "Invoice");
                            }
                            FicciImwd imwd = new FicciImwd();
                            imwd.ImwdScreenName = "Invoice Approver";
                            imwd.CustomerId = returnid;
                            imwd.ImwdCreatedOn = DateTime.Now;
                            imwd.ImwdCreatedBy = request.LoginId;
                            if (request.IsDraft == false)
                            {
                                imwd.ImwdPendingEmailAt = _dbContext.FicciImpiHeaders.Where(x => x.ImpiHeaderId == returnid && x.HeaderStatusId == 2).Select(x => x.ImpiHeaderTlApprover).FirstOrDefault().ToString();
                            }
                            imwd.ImwdStatus = request.IsDraft == true ? "1" : "2";
                            imwd.ImwdPendingAt = _dbContext.StatusMasters.Where(x => x.StatusId == data.HeaderStatusId).Select(a => a.StatusName).FirstOrDefault();
                            imwd.ImwdInitiatedBy = request.LoginId;
                            imwd.ImwdRemarks = request.InvoiceRemarks;
                            imwd.ImwdRole = request.RoleName;
                            imwd.ImwdType = 2;
                            _dbContext.Add(imwd);

                            _dbContext.SaveChanges();
                            if (returnid != 0 && request.lineItem_Requests.Count > 0)
                            {
                                var dataline = _dbContext.FicciImpiLines.Where(x => x.PiHeaderId == data.ImpiHeaderId).ToList();
                                foreach (var l in dataline)
                                {
                                    l.IsDeleted = true;
                                    l.ImpiLineActive = false;

                                }
                                await _dbContext.SaveChangesAsync();
                                foreach (var k in request.lineItem_Requests)
                                {



                                    FicciImpiLine FicciImpiLine = new FicciImpiLine();
                                    FicciImpiLine.DocumentType = k.DocumentType;
                                    FicciImpiLine.ImpiDocumentNo = k.ImpiDocumentNo;
                                    FicciImpiLine.ImpiGlNo = k.ImpiGlNo;
                                    FicciImpiLine.ImpiGstBaseAmount = k.ImpiGstBaseAmount;
                                    FicciImpiLine.ImpiLineAmount = k.ImpiLineAmount;
                                    FicciImpiLine.ImpiLineActive = true;
                                    FicciImpiLine.ImpiLineCreatedBy = request.LoginId;
                                    FicciImpiLine.ImpiLineCreatedOn = DateTime.Now;
                                    FicciImpiLine.PiHeaderId = returnid;
                                    FicciImpiLine.IsDeleted = false;
                                    FicciImpiLine.ImpiLinePiNo = DateTime.Now.ToString("yyyyMMddhhmmss");
                                    FicciImpiLine.ImpiTotalGstAmount = k.ImpiTotalGstAmount;
                                    FicciImpiLine.ImpiNetTotal = k.ImpiNetTotal;
                                    FicciImpiLine.ImpiLocationCode = k.ImpiLocationCode;
                                    FicciImpiLine.ImpiQuantity = k.ImpiQuantity;
                                    FicciImpiLine.ImpiUnitPrice = k.ImpiUnitPrice;
                                    FicciImpiLine.ImpiGstgroupCode = k.ImpiGstgroupCode;
                                    FicciImpiLine.ImpiGstgroupType = k.ImpiGstgroupType;
                                    FicciImpiLine.ImpiHsnsaccode = k.ImpiHsnsaccode;
                                    FicciImpiLine.ImpiLineNo = k.ImpiLineNo;
                                    FicciImpiLine.ImpiLinePiNo = k.ImpiLinePiNo;
                                    FicciImpiLine.ImpiType = k.ImpiType;
                                    _dbContext.Add(FicciImpiLine);
                                    _dbContext.SaveChanges();


                                }

                            }

                            if (request.IsDraft == false)
                            {
                                string htmlbody = InvoiceAssignhtmlBody(_mySettings.Website_link, data.ImpiHeaderCustomerName, data.ImpiHeaderCustomerCity, data.ImpiHeaderPanNo, data.ImpiHeaderCustomerGstNo, data.ImpiHeaderCustomerContactPerson, data.ImpiHeaderCustomerPhoneNo, data.ImpiHeaderProjectCode, data.ImpiHeaderProjectName, data.ImpiHeaderCustomerCode);
                                SendEmail(data.ImpiHeaderTlApprover, data.ImpiHeaderCustomerEmailId, $"New PI Assigned for Approval : {data.ImpiHeaderCustomerName}", htmlbody, _mySettings);

                            }

                            if (data.ImpiHeaderId != 0 && data.ImpiHeaderPiNo != null)
                            {
                               
                                NavERPController navERPController = new NavERPController(_configuration, _dbContext);

                                PURCHASE_INVOICE_HEADER_UPDATE PostData = new PURCHASE_INVOICE_HEADER_UPDATE();

                                PostData.no = data.ImpiHeaderPiNo;
                                PostData.sellToCustomerNo = data.ImpiHeaderCustomerCode;
                                PostData.sellToCustomerName = data.ImpiHeaderCustomerName;
                                //  PostData.sellToCustomerName2 = request.cus;
                                PostData.sellToCity = data.ImpiHeaderCustomerCity;
                                PostData.sellToAddress = data.ImpiHeaderCustomerAddress;
                                PostData.sellToAddress2 = data.ImpiHeaderCustomerAddress;
                                PostData.sellToPostCode = data.ImpiHeaderCustomerPinCode;
                                //PostData.sellToCountryRegionCode = request.cpi;
                                PostData.GST_No = data.ImpiHeaderCustomerGstNo;
                                PostData.PAN_No = data.ImpiHeaderPanNo;

                                PostData.DepartmentCode = data.ImpiHeaderProjectDepartmentCode;
                                PostData.DepartmentName = data.ImpiHeaderProjectDepartmentName;
                                PostData.ProjectCode = data.ImpiHeaderProjectCode;

                                PostData.DivisionCode = data.ImpiHeaderProjectDivisionCode;
                                PostData.DivisionName = data.ImpiHeaderProjectDivisionName;
                                PostData.ApproverTL = data.ImpiHeaderTlApprover;
                                PostData.ApproverCH = data.ImpiHeaderClusterApprover;
                                PostData.ApproverSupport = data.ImpiHeaderSupportApprover;
                                PostData.FinanceApprover = data.ImpiHeaderFinanceApprover;
                                PostData.InvoicePortalOrder = false;
                                PostData.InvoicePortalSubmitted = false;                                
                                PostData.Cancelled = false;
                                PostData.CancelRemark = data.ImpiHeaderCancelRemarks;
                                PostData.CreatedByUser = "";

                                dynamic erpResponse = await navERPController.UpdatePIData(PostData);
                                //    var updatedNumber = erpResponse.Value.Data.no;
                                //  var resu = _dbContext.FicciImpiHeaders.Where(x => x.ImpiHeaderId == data.ImpiHeaderId).FirstOrDefault();
                                // resu.ImpiHeaderPiNo = updatedNumber;
                                // _dbContext.Update(resu);
                                // _dbContext.SaveChanges();

                                if (returnid != 0 && data.ImpiHeaderPiNo != null && request.lineItem_Requests.Count > 0)
                                {

                                    //  PURCHASE_INVOICE_LINE[] PostLineDataList = new PURCHASE_INVOICE_LINE[request.lineItem_Requests.Count];

                                    int index = 0;
                                    long LineNo = 0;
                                    foreach (var line in request.lineItem_Requests)
                                    {
                                        NavERPController navERPControllerLine = new NavERPController(_configuration, _dbContext);
                                        PURCHASE_INVOICE_LINE PostLineData = new PURCHASE_INVOICE_LINE();

                                        if (LineNo <= 0)
                                        {
                                            LineNo = 10000;
                                        }
                                        else
                                        {
                                            LineNo = Convert.ToInt32(LineNo) + 10000;
                                        }

                                        PostLineData.documentNo = data.ImpiHeaderPiNo;
                                        PostLineData.documentType = "Invoice";
                                        PostLineData.type = "G/L Account";
                                        PostLineData.lineNo = LineNo;    // Convert.ToInt32(line.ImpiLineNo);
                                        PostLineData.quantity = Convert.ToInt32(line.ImpiQuantity);
                                        PostLineData.unitPrice = line.ImpiUnitPrice;
                                        PostLineData.hSN_SAC_Code = line.ImpiHsnsaccode;
                                        PostLineData.no_ = line.ImpiGlNo;
                                        PostLineData.gSTGroupCode = line.ImpiGstgroupCode;
                                        PostLineData.LocationCode = line.ImpiLocationCode;
                                        PostLineData.lineAmount = line.ImpiLineAmount;
                                        //  PostLineData.gST_Group_Type = "Goods";

                                        dynamic erpResponseDelete = await navERPController.DeletePILineData(PostLineData);
                                        //dynamic erpResponse1 = await navERPController.UpdatePILineData(PostLineData);

                                        dynamic erpResponse1 = await navERPController.PostPILineData(PostLineData);

                                        ////   PostLineDataList[index] = PostLineData;
                                        //    index++;
                                        //  var updatedNumber = erpResponse1.Data.No;
                                        // var resu = _dbContext.FicciImpiHeaders.Where(x => x.ImpiHeaderId == returnId).FirstOrDefault();
                                        // resu.ImpiHeaderPiNo = updatedNumber;
                                        //   _dbContext.Update(resu);
                                        // _dbContext.SaveChanges();

                                    }

                                    dynamic erpResponse2 = await navERPController.UploadFileInERP(data.ImpiHeaderPiNo,data.ImpiHeaderRecordNo);

                                }

                            }


                            purchaseInvoice_New.Status = true;
                            purchaseInvoice_New.Message = "Purchase Invoice Update Successfully";
                            return StatusCode(200, purchaseInvoice_New);
                        }
                        else
                        {
                            purchaseInvoice_New.Status = false;
                            purchaseInvoice_New.Message = "Purchase Invoice record not found";
                            return StatusCode(200, purchaseInvoice_New);
                        }
                    }
                }
                else
                {
                    purchaseInvoice_New.Status = false;
                    purchaseInvoice_New.Message = "Invalid Data";
                    return StatusCode(404, purchaseInvoice_New);
                }
            }
            catch (Exception ex)
            {
                purchaseInvoice_New.Status = false;
                purchaseInvoice_New.Message = "Invalid Data";
                return StatusCode(500, purchaseInvoice_New);
            }
        }

        [HttpGet]

        public async Task<IActionResult> GET(string email)
        {
            PurchaseInvoice_New purchaseInvoice_New = new PurchaseInvoice_New();
            try
            {
                if (email == null)
                {
                    var response = new
                    {
                        status = true,
                        message = "Email is Mandatory field",
                    };
                    return Ok(response);
                }
                //var list = _dbContext.FicciImpiHeaders.Where(m => m.ImpiHeaderActive == true).ToList();
                //if (email != null)
                //{
                //    list = list.Where(m => m.ImpiHeaderCreatedBy == email).ToList();
                //}

                var emp_Role = await _dbContext.FicciImums.Where(x => x.ImumEmail == email).Select(a => a.RoleId).FirstOrDefaultAsync();

                var list = _dbContext.FicciImpiHeaders.Where(m => m.ImpiHeaderActive == true).ToList();
                if (emp_Role != 1)
                {
                    list = list.Where(m => m.ImpiHeaderCreatedBy == email).OrderByDescending(x => x.ImpiHeaderSubmittedDate).ToList();
                }

                if (list.Count > 0)
                {
                    List<PurchaseInvoice_Response> purchaseInvoice_responsel = new List<PurchaseInvoice_Response>();
                    foreach (var k in list)
                    {
                        PurchaseInvoice_Response purchaseInvoice_response = new PurchaseInvoice_Response();
                        purchaseInvoice_response.HeaderId = k.ImpiHeaderId;
                        purchaseInvoice_response.HeaderPiNo = k.ImpiHeaderPiNo;
                        purchaseInvoice_response.ImpiHeaderInvoiceType = k.ImpiHeaderInvoiceType;
                        purchaseInvoice_response.ImpiHeaderProjectCode = k.ImpiHeaderProjectCode;
                        purchaseInvoice_response.ImpiHeaderProjectName = k.ImpiHeaderProjectName;
                        purchaseInvoice_response.ImpiHeaderProjectDepartmentCode = k.ImpiHeaderProjectDepartmentCode;
                        purchaseInvoice_response.ImpiHeaderProjectDepartmentName = k.ImpiHeaderProjectDepartmentName;
                        purchaseInvoice_response.ImpiHeaderProjectDivisionCode = k.ImpiHeaderProjectDivisionCode;
                        purchaseInvoice_response.ImpiHeaderProjectDivisionName = k.ImpiHeaderProjectDivisionName;
                        purchaseInvoice_response.ImpiHeaderPanNo = k.ImpiHeaderPanNo;
                        purchaseInvoice_response.ImpiHeaderGstNo = k.ImpiHeaderGstNo;
                        purchaseInvoice_response.ImpiHeaderCustomerName = k.ImpiHeaderCustomerName;
                        purchaseInvoice_response.ImpiHeaderCustomerCode = k.ImpiHeaderCustomerCode;
                        purchaseInvoice_response.ImpiHeaderCustomerAddress = k.ImpiHeaderCustomerAddress;
                        purchaseInvoice_response.ImpiHeaderCustomerCity = k.ImpiHeaderCustomerCity;
                        purchaseInvoice_response.ImpiHeaderCustomerState = k.ImpiHeaderCustomerState;
                        purchaseInvoice_response.ImpiHeaderCustomerPinCode = k.ImpiHeaderCustomerPinCode;
                        purchaseInvoice_response.ImpiHeaderCustomerGstNo = k.ImpiHeaderCustomerGstNo;
                        purchaseInvoice_response.ImpiHeaderCustomerContactPerson = k.ImpiHeaderCustomerContactPerson;
                        purchaseInvoice_response.ImpiHeaderCustomerEmailId = k.ImpiHeaderCustomerEmailId;
                        purchaseInvoice_response.ImpiHeaderCustomerPhoneNo = k.ImpiHeaderCustomerPhoneNo;
                        purchaseInvoice_response.ImpiHeaderCreatedBy = k.ImpiHeaderCreatedBy;


                        purchaseInvoice_response.IsDraft = k.IsDraft;
                        purchaseInvoice_response.ImpiHeaderSubmittedDate = k.ImpiHeaderSubmittedDate;
                        purchaseInvoice_response.ImpiHeaderTotalInvoiceAmount = k.ImpiHeaderTotalInvoiceAmount;
                        purchaseInvoice_response.ImpiHeaderPaymentTerms = k.ImpiHeaderPaymentTerms;
                        purchaseInvoice_response.ImpiHeaderRemarks = k.ImpiHeaderRemarks;
                        purchaseInvoice_response.ImpiHeaderModifiedDate = k.ImpiHeaderModifiedOn;
                        purchaseInvoice_response.ImpiHeaderTlApprover = k.ImpiHeaderTlApprover;
                        purchaseInvoice_response.ImpiHeaderClusterApprover = k.ImpiHeaderClusterApprover;
                        purchaseInvoice_response.ImpiHeaderFinanceApprover = k.ImpiHeaderFinanceApprover;
                        purchaseInvoice_response.AccountApproverRemarks = k.AccountApproverRemarks;
                        purchaseInvoice_response.ImpiHeaderClusterApproverRemarks = k.ImpiHeaderClusterApproverRemarks;
                        purchaseInvoice_response.ImpiHeaderFinanceRemarks = k.ImpiHeaderFinanceRemarks;
                        purchaseInvoice_response.ImpiHeaderTlApproverRemarks = k.ImpiHeaderTlApproverRemarks;
                        purchaseInvoice_response.AccountApprover = k.AccountApprover;
                        purchaseInvoice_response.AccountApproveDate = k.AccountApproverDate;
                        purchaseInvoice_response.TlApproveDate = k.ImpiHeaderTlApproverDate;
                        purchaseInvoice_response.HeaderStatusId = k.HeaderStatusId;
                        purchaseInvoice_response.ClusterApproveDate = k.ImpiHeaderClusterApproverDate;
                        purchaseInvoice_response.FinanceApproveDate = k.ImpiHeaderFinanceApproverDate;
                        purchaseInvoice_response.CancelRemarks = k.ImpiHeaderCancelRemarks;
                        purchaseInvoice_response.CancelOn = k.ImpiCancelOn;
                        purchaseInvoice_response.CancelBy = k.ImpiCancelBy;
                        purchaseInvoice_response.IsCancel = k.IsCancel;
                        purchaseInvoice_response.startDate = k.ProjectStartDate;
                        purchaseInvoice_response.endDate = k.ProjectEndDate;
                        purchaseInvoice_response.ImpiHeaderAttachment = _dbContext.FicciImads.Where(x => x.ImadActive != false && x.ResourceId == k.ImpiHeaderId && x.ResourceTypeId == 2).ToList();
                        purchaseInvoice_response.HeaderStatus = _dbContext.StatusMasters.Where(x => x.StatusId == k.HeaderStatusId).Select(a => a.StatusName).FirstOrDefault();
                        purchaseInvoice_response.WorkFlowHistory = _dbContext.FicciImwds.Where(x => x.CustomerId == purchaseInvoice_response.HeaderId && x.ImwdType == 2).ToList(); ;
                        var lindata = _dbContext.FicciImpiLines.Where(m => m.ImpiLineActive == true && m.PiHeaderId == k.ImpiHeaderId).ToList();
                        if (lindata.Count > 0)
                        {
                            List<LineItem_request> lineItem_Requestl = new List<LineItem_request>();
                            foreach (var l in lindata)
                            {
                                LineItem_request lineItem_Request = new LineItem_request();
                                lineItem_Request.DocumentType = l.DocumentType;
                                lineItem_Request.ImpiDocumentNo = l.ImpiDocumentNo;
                                lineItem_Request.ImpiGlNo = l.ImpiGlNo;
                                lineItem_Request.ImpiGstBaseAmount = l.ImpiGstBaseAmount;
                                lineItem_Request.ImpiLineAmount = l.ImpiLineAmount;
                                lineItem_Request.ImpiLinePiNo = DateTime.Now.ToString("yyyyMMddhhmmss");
                                lineItem_Request.ImpiTotalGstAmount = l.ImpiTotalGstAmount;
                                lineItem_Request.ImpiNetTotal = l.ImpiNetTotal;
                                lineItem_Request.ImpiLocationCode = l.ImpiLocationCode;
                                lineItem_Request.ImpiQuantity = l.ImpiQuantity;
                                lineItem_Request.ImpiUnitPrice = l.ImpiUnitPrice;
                                lineItem_Request.ImpiGstgroupCode = l.ImpiGstgroupCode;
                                lineItem_Request.ImpiGstgroupType = l.ImpiGstgroupType;
                                lineItem_Request.ImpiHsnsaccode = l.ImpiHsnsaccode;
                                lineItem_Request.ImpiLineNo = l.ImpiLineNo;
                                lineItem_Request.ImpiLinePiNo = l.ImpiLinePiNo;

                                lineItem_Requestl.Add(lineItem_Request);
                            }

                            purchaseInvoice_response.lineItem_Requests = lineItem_Requestl;


                        }
                        purchaseInvoice_responsel.Add(purchaseInvoice_response);

                    }


                    purchaseInvoice_New.Status = true;
                    purchaseInvoice_New.Data = purchaseInvoice_responsel;
                    purchaseInvoice_New.Message = "Purchase Invoice list successfully";
                    return StatusCode(200, purchaseInvoice_New);
                }
                else
                {
                    purchaseInvoice_New.Status = false;
                    purchaseInvoice_New.Message = "No Data found";
                    return StatusCode(200, purchaseInvoice_New);
                }



            }
            catch (Exception ex)
            {
                purchaseInvoice_New.Status = false;
                purchaseInvoice_New.Message = "Invalid Data";
                return StatusCode(500, purchaseInvoice_New);
            }
        }


        //[HttpGet]

        //public async Task<IActionResult> GET(string email)
        //{
        //    PurchaseInvoice_New purchaseInvoice_New = new PurchaseInvoice_New();
        //    try
        //    {
        //        if (email == null)
        //        {
        //            var response = new
        //            {
        //                status = true,
        //                message = "Email is Mandatory field",
        //            };
        //            return Ok(response);
        //        }
        //        //var list = _dbContext.FicciImpiHeaders.Where(m => m.ImpiHeaderActive == true).ToList();
        //        //if (email != null)
        //        //{
        //        //    list = list.Where(m => m.ImpiHeaderCreatedBy == email).ToList();
        //        //}

        //        var emp_Role = await _dbContext.FicciImums.Where(x => x.ImumEmail == email).Select(a => a.RoleId).FirstOrDefaultAsync();

        //        var list = _dbContext.FicciImpiHeaders.Where(m => m.ImpiHeaderActive == true).ToList();
        //        if (emp_Role != 1)
        //        {
        //            list = list.Where(m => m.ImpiHeaderCreatedBy == email).OrderByDescending(x => x.ImpiHeaderSubmittedDate).ToList();
        //        }

        //        if (list.Count > 0)
        //        {
        //            List<PurchaseInvoice_Response> purchaseInvoice_responsel = new List<PurchaseInvoice_Response>();
        //            foreach (var k in list)
        //            {
        //                PurchaseInvoice_Response purchaseInvoice_response = new PurchaseInvoice_Response();
        //                purchaseInvoice_response.HeaderId = k.ImpiHeaderId;
        //                purchaseInvoice_response.HeaderPiNo = k.ImpiHeaderPiNo;
        //                purchaseInvoice_response.ImpiHeaderInvoiceType = k.ImpiHeaderInvoiceType;
        //                purchaseInvoice_response.ImpiHeaderProjectCode = k.ImpiHeaderProjectCode;
        //                purchaseInvoice_response.ImpiHeaderProjectName = k.ImpiHeaderProjectName;
        //                purchaseInvoice_response.ImpiHeaderProjectDepartmentCode = k.ImpiHeaderProjectDepartmentCode;
        //                purchaseInvoice_response.ImpiHeaderProjectDepartmentName = k.ImpiHeaderProjectDepartmentName;
        //                purchaseInvoice_response.ImpiHeaderProjectDivisionCode = k.ImpiHeaderProjectDivisionCode;
        //                purchaseInvoice_response.ImpiHeaderProjectDivisionName = k.ImpiHeaderProjectDivisionName;
        //                purchaseInvoice_response.ImpiHeaderPanNo = k.ImpiHeaderPanNo;
        //                purchaseInvoice_response.ImpiHeaderGstNo = k.ImpiHeaderGstNo;
        //                purchaseInvoice_response.ImpiHeaderCustomerName = k.ImpiHeaderCustomerName;
        //                purchaseInvoice_response.ImpiHeaderCustomerCode = k.ImpiHeaderCustomerCode;
        //                purchaseInvoice_response.ImpiHeaderCustomerAddress = k.ImpiHeaderCustomerAddress;
        //                purchaseInvoice_response.ImpiHeaderCustomerCity = k.ImpiHeaderCustomerCity;
        //                purchaseInvoice_response.ImpiHeaderCustomerState = k.ImpiHeaderCustomerState;
        //                purchaseInvoice_response.ImpiHeaderCustomerPinCode = k.ImpiHeaderCustomerPinCode;
        //                purchaseInvoice_response.ImpiHeaderCustomerGstNo = k.ImpiHeaderCustomerGstNo;
        //                purchaseInvoice_response.ImpiHeaderCustomerContactPerson = k.ImpiHeaderCustomerContactPerson;
        //                purchaseInvoice_response.ImpiHeaderCustomerEmailId = k.ImpiHeaderCustomerEmailId;
        //                purchaseInvoice_response.ImpiHeaderCustomerPhoneNo = k.ImpiHeaderCustomerPhoneNo;
        //                purchaseInvoice_response.ImpiHeaderCreatedBy = k.ImpiHeaderCreatedBy;

        //                ////error in auto generated model ImpiHeaderAttachment is not null
        //                //if (k.ImpiHeaderAttachment != null)
        //                //{
        //                //    string[]? valuesArray = k.ImpiHeaderAttachment.Split(',');

        //                //    // Display the result
        //                //    List<FicciImad> listing = new List<FicciImad>();

        //                //    foreach (string value in valuesArray)
        //                //    {

        //                //        var path = await _dbContext.FicciImads.Where(x => x.ImadId == Convert.ToInt32(value) && x.ImadActive !=false).FirstOrDefaultAsync();
        //                //        if (path != null)
        //                //        {

        //                //            listing.Add(path);
        //                //        }
        //                //    }

        //                //    purchaseInvoice_response.ImpiHeaderAttachment = listing;
        //                //}
        //                purchaseInvoice_response.IsDraft = k.IsDraft;
        //                purchaseInvoice_response.ImpiHeaderSubmittedDate = k.ImpiHeaderSubmittedDate;
        //                purchaseInvoice_response.ImpiHeaderTotalInvoiceAmount = k.ImpiHeaderTotalInvoiceAmount;
        //                purchaseInvoice_response.ImpiHeaderPaymentTerms = k.ImpiHeaderPaymentTerms;
        //                purchaseInvoice_response.ImpiHeaderRemarks = k.ImpiHeaderRemarks;
        //                purchaseInvoice_response.ImpiHeaderModifiedDate = k.ImpiHeaderModifiedOn;
        //                purchaseInvoice_response.ImpiHeaderTlApprover = k.ImpiHeaderTlApprover;
        //                purchaseInvoice_response.ImpiHeaderClusterApprover = k.ImpiHeaderClusterApprover;
        //                purchaseInvoice_response.ImpiHeaderFinanceApprover = k.ImpiHeaderFinanceApprover;
        //                purchaseInvoice_response.AccountApproverRemarks = k.AccountApproverRemarks;
        //                purchaseInvoice_response.ImpiHeaderClusterApproverRemarks = k.ImpiHeaderClusterApproverRemarks;
        //                purchaseInvoice_response.ImpiHeaderFinanceRemarks = k.ImpiHeaderFinanceRemarks;
        //                purchaseInvoice_response.ImpiHeaderTlApproverRemarks = k.ImpiHeaderTlApproverRemarks;
        //                purchaseInvoice_response.AccountApprover = k.AccountApprover;
        //                purchaseInvoice_response.AccountApproveDate = k.AccountApproverDate;
        //                purchaseInvoice_response.TlApproveDate = k.ImpiHeaderTlApproverDate;
        //                purchaseInvoice_response.HeaderStatusId = k.HeaderStatusId;
        //                purchaseInvoice_response.ClusterApproveDate = k.ImpiHeaderClusterApproverDate;
        //                purchaseInvoice_response.FinanceApproveDate = k.ImpiHeaderFinanceApproverDate;
        //                purchaseInvoice_response.ImpiHeaderAttachment = _dbContext.FicciImads.Where(x => x.ImadActive != false && x.ResourceId == k.ImpiHeaderId && x.ResourceTypeId == 2).ToList();
        //                purchaseInvoice_response.HeaderStatus = _dbContext.StatusMasters.Where(x => x.StatusId == k.HeaderStatusId).Select(a => a.StatusName).FirstOrDefault();
        //                purchaseInvoice_response.WorkFlowHistory = _dbContext.FicciImwds.Where(x => x.CustomerId == purchaseInvoice_response.HeaderId && x.ImwdType == 2).ToList(); ;
        //                var lindata = _dbContext.FicciImpiLines.Where(m => m.ImpiLineActive == true && m.PiHeaderId == k.ImpiHeaderId).ToList();
        //                if (lindata.Count > 0)
        //                {
        //                    List<LineItem_request> lineItem_Requestl = new List<LineItem_request>();
        //                    foreach (var l in lindata)
        //                    {
        //                        LineItem_request lineItem_Request = new LineItem_request();
        //                        lineItem_Request.DocumentType = l.DocumentType;
        //                        lineItem_Request.ImpiDocumentNo = l.ImpiDocumentNo;
        //                        lineItem_Request.ImpiGlNo = l.ImpiGlNo;
        //                        lineItem_Request.ImpiGstBaseAmount = l.ImpiGstBaseAmount;
        //                        lineItem_Request.ImpiLineAmount = l.ImpiLineAmount;
        //                        lineItem_Request.ImpiLinePiNo = DateTime.Now.ToString("yyyyMMddhhmmss");
        //                        lineItem_Request.ImpiTotalGstAmount = l.ImpiTotalGstAmount;
        //                        lineItem_Request.ImpiNetTotal = l.ImpiNetTotal;
        //                        lineItem_Request.ImpiLocationCode = l.ImpiLocationCode;
        //                        lineItem_Request.ImpiQuantity = l.ImpiQuantity;
        //                        lineItem_Request.ImpiUnitPrice = l.ImpiUnitPrice;
        //                        lineItem_Request.ImpiGstgroupCode = l.ImpiGstgroupCode;
        //                        lineItem_Request.ImpiGstgroupType = l.ImpiGstgroupType;
        //                        lineItem_Request.ImpiHsnsaccode = l.ImpiHsnsaccode;
        //                        lineItem_Request.ImpiLineNo = l.ImpiLineNo;
        //                        lineItem_Request.ImpiLinePiNo = l.ImpiLinePiNo;

        //                        lineItem_Requestl.Add(lineItem_Request);
        //                    }

        //                    purchaseInvoice_response.lineItem_Requests = lineItem_Requestl;


        //                }
        //                purchaseInvoice_responsel.Add(purchaseInvoice_response);

        //            }


        //            purchaseInvoice_New.Status = true;
        //            purchaseInvoice_New.Data = purchaseInvoice_responsel;
        //            purchaseInvoice_New.Message = "Purchase Invoice list successfully";
        //            return StatusCode(200, purchaseInvoice_New);
        //        }
        //        else
        //        {
        //            purchaseInvoice_New.Status = false;
        //            purchaseInvoice_New.Message = "No Data found";
        //            return StatusCode(200, purchaseInvoice_New);
        //        }



        //    }
        //    catch (Exception ex)
        //    {
        //        purchaseInvoice_New.Status = false;
        //        purchaseInvoice_New.Message = "Invalid Data";
        //        return StatusCode(500, purchaseInvoice_New);
        //    }
        //}

        [HttpDelete("{headerid}")]

        public async Task<IActionResult> DELETE(int headerid)
        {
            PO_delete pO_Delete = new PO_delete();


            try
            {

                var list = await _dbContext.FicciImpiHeaders.Where(m => m.ImpiHeaderId == headerid).FirstOrDefaultAsync();

                list.ImpiHeaderActive = false;
                await _dbContext.SaveChangesAsync();

                pO_Delete.status = true;
                pO_Delete.message = "Delete Successfully";
                return StatusCode(200, pO_Delete);

            }
            catch (Exception ex)
            {
                pO_Delete.status = false;
                pO_Delete.message = "Invalid Data";
                return StatusCode(500, pO_Delete);
            }
        }

        [HttpPost("CancelEmployee")]
        public async Task<IActionResult> CancelEmployee(CancelEmployee employee)
        {
            try
            {
                if (employee.LoginId != null && employee.Remarks != null)
                {
                    string subject = "";
                    string htmlbody = "";
                    var result = await _dbContext.FicciImpiHeaders.Where(x => x.ImpiHeaderId == employee.HeaderId).FirstOrDefaultAsync();
                    if (result == null)
                    {
                        return NotFound("No inovice found");
                    }
                    result.HeaderStatusId = 13;
                    result.ImpiCancelBy = employee.LoginId;
                    result.ImpiHeaderCancelRemarks = employee.Remarks;
                    result.ImpiCancelOn = DateTime.Now;
                    result.IsCancel = true;
                    await _dbContext.SaveChangesAsync();
                    var status = _dbContext.StatusMasters.Where(x => x.StatusId == result.HeaderStatusId).Select(a => a.StatusName).FirstOrDefault();
                    subject = "PI is Cancel by : " + result.ImpiHeaderCustomerName + "";
                    htmlbody = InvoiceApprovalhtmlBody(status, _mySettings.Website_link, result.ImpiHeaderCustomerCode, result.ImpiHeaderCustomerName, result.ImpiHeaderCustomerCity, result.ImpiHeaderPanNo, result.ImpiHeaderCustomerGstNo, result.ImpiHeaderProjectName, result.ImpiHeaderProjectCode);
                    MailMethod(result.ImpiHeaderFinanceApprover, result.ImpiHeaderCustomerEmailId, subject, htmlbody, "Cancel", employee.LoginId, result.ImpiHeaderId);

                    if (result != null)
                    {
                        NavERPController navERPController = new NavERPController(_configuration, _dbContext);

                        PURCHASE_INVOICE_HEADER_UPDATE PostData = new PURCHASE_INVOICE_HEADER_UPDATE();

                        PostData.no = result.ImpiHeaderPiNo;
                        PostData.sellToCustomerNo = result.ImpiHeaderCustomerCode;
                        PostData.sellToCustomerName = result.ImpiHeaderCustomerName;
                        //  PostData.sellToCustomerName2 = request.cus;
                        PostData.sellToCity = result.ImpiHeaderCustomerCity;
                        PostData.sellToAddress = result.ImpiHeaderCustomerAddress;
                        PostData.sellToAddress2 = result.ImpiHeaderCustomerAddress;
                        PostData.sellToPostCode = result.ImpiHeaderCustomerPinCode;
                        //PostData.sellToCountryRegionCode = request.cpi;
                        PostData.GST_No = result.ImpiHeaderCustomerGstNo;
                        PostData.PAN_No = result.ImpiHeaderPanNo;

                        PostData.DepartmentCode = result.ImpiHeaderProjectDepartmentCode;
                        PostData.DepartmentName = result.ImpiHeaderProjectDepartmentName;
                        PostData.ProjectCode = result.ImpiHeaderProjectCode;

                        PostData.DivisionCode = result.ImpiHeaderProjectDivisionCode;
                        PostData.DivisionName = result.ImpiHeaderProjectDivisionName;
                        PostData.ApproverTL = result.ImpiHeaderTlApprover;
                        PostData.ApproverCH = result.ImpiHeaderClusterApprover;
                        PostData.ApproverSupport = result.ImpiHeaderSupportApprover;
                        PostData.FinanceApprover = result.ImpiHeaderFinanceApprover;
                        PostData.InvoicePortalOrder = false;
                        PostData.InvoicePortalSubmitted = false;
                        PostData.Cancelled = true;
                        PostData.CancelRemark = employee.Remarks;
                        PostData.CreatedByUser = "";

                        dynamic erpResponse = await navERPController.UpdatePIData(PostData);

                    }


                    FicciImwd imwd = new FicciImwd();
                    imwd.ImwdScreenName = "Invoice Approver";
                    imwd.CustomerId = employee.HeaderId;
                    imwd.ImwdCreatedOn = DateTime.Now;
                    imwd.ImwdCreatedBy = employee.LoginId;
                    imwd.ImwdStatus = result.HeaderStatusId.ToString();
                    imwd.ImwdPendingAt = _dbContext.StatusMasters.Where(x => x.StatusId == result.HeaderStatusId).Select(a => a.StatusName).FirstOrDefault();
                    imwd.ImwdInitiatedBy = employee.LoginId;
                    imwd.ImwdRemarks = employee.Remarks;
                    imwd.ImwdRole = _dbContext.FicciImums.Where(x => x.ImumEmail == employee.LoginId).Select(x => x.Role.RoleName).FirstOrDefault();
                    imwd.ImwdType = 2;
                    _dbContext.Add(imwd);

                    _dbContext.SaveChanges();
                    var responseObject = new
                    {
                        status = true,
                        message = "Invoice has been Cancel",
                        data = result
                    };
                    return StatusCode(200, responseObject);
                }
                else
                {
                    var responseObject = new
                    {
                        status = false,
                        message = "Some error occured"
                    };
                    return StatusCode(200, responseObject);
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }
        }

        [NonAction]
        public bool MailMethod(string mailTo, string mailCC, string mailSubject, string mailBody, string? ResourceType, string LoginId, int ResourceId)
        {
            try
            {
                if (mailTo == null || mailCC == null || mailSubject == null || mailBody == null)
                {
                    return false;
                }
                bool isEmailSent = SendEmailData(mailTo, mailCC, mailSubject, mailBody, _mySettings, null);

                FicciImmd immd = new FicciImmd();
                immd.ImmdMailTo = mailTo;
                immd.ImmdMailCc = mailCC;
                immd.ImmdMailSubject = mailSubject;
                immd.ImmdMailBody = mailBody;
                immd.ImmdActive = true;
                immd.ResourceType = ResourceType;
                immd.ImmdCreatedBy = LoginId;
                immd.IsSent = isEmailSent;
                immd.ResourceId = ResourceId;
                immd.ImmdCreatedOn = DateTime.Now;
                immd.ImmdMailSentOn = DateTime.Now;
                _dbContext.Add(immd);
                _dbContext.SaveChanges();

                return true;

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //[HttpPost("CancelEmployee")]
        //public async Task<IActionResult> CancelEmployee(CancelEmployee employee)
        //{
        //    try
        //    {
        //        if(employee.LoginId != null && employee.Remarks != null)
        //        {
        //            var result = await _dbContext.FicciImpiHeaders.Where(x => x.ImpiHeaderId == employee.HeaderId).FirstOrDefaultAsync();
        //            if(result == null)
        //            {
        //                return NotFound("No inovice found");
        //            }
        //            result.HeaderStatusId = 13;
        //            result.ImpiCancelBy = employee.LoginId;
        //            result.ImpiHeaderCancelRemarks = employee.Remarks;
        //            result.ImpiCancelOn = DateTime.Now;
        //            await _dbContext.SaveChangesAsync();

        //            FicciImwd imwd = new FicciImwd();
        //            imwd.ImwdScreenName = "Invoice Approver";
        //            imwd.CustomerId = employee.HeaderId;
        //            imwd.ImwdCreatedOn = DateTime.Now;
        //            imwd.ImwdCreatedBy = employee.LoginId;
        //            imwd.ImwdStatus = result.HeaderStatusId.ToString();
        //            imwd.ImwdPendingAt = _dbContext.StatusMasters.Where(x => x.StatusId == result.HeaderStatusId).Select(a => a.StatusName).FirstOrDefault();
        //            imwd.ImwdInitiatedBy = employee.LoginId;
        //            imwd.ImwdRemarks = employee.Remarks;
        //            imwd.ImwdRole = _dbContext.FicciImums.Where(x => x.ImumEmail == employee.LoginId).Select(x => x.Role.RoleName).FirstOrDefault();
        //            imwd.ImwdType = 2;
        //            _dbContext.Add(imwd);

        //            _dbContext.SaveChanges();
        //            var responseObject = new
        //            {
        //                status = true,
        //                message = "Invoice has been Cancel",
        //                data = result
        //            };
        //            return StatusCode(200, responseObject);
        //        }
        //        else
        //        {
        //            var responseObject = new
        //            {
        //                status = false,
        //                message = "Some error occured"
        //            };
        //            return StatusCode(200, responseObject);
        //        }

        //    }
        //    catch(Exception ex)
        //    {
        //        return BadRequest(ex.Message);

        //    }
        //}

    }
    public class PO_delete
    {
        public Boolean status { get; set; }
        public string message { get; set; }
    }



    public class FileInfoModel
    {
        public string FileName { get; set; }
        public long Size { get; set; }
        public string ContentType { get; set; }
    }
}
