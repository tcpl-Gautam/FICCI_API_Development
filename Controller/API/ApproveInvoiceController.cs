using FICCI_API.DTO;
using FICCI_API.Models;
using FICCI_API.ModelsEF;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace FICCI_API.Controller.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApproveInvoiceController : BaseController
    {
        private readonly FICCI_DB_APPLICATIONSContext _dbContext;
        private readonly MySettings _mySettings;
        public ApproveInvoiceController(FICCI_DB_APPLICATIONSContext dbContext, IOptions<MySettings> mySettings) : base(dbContext)
        {
            _dbContext = dbContext;
            _mySettings = mySettings.Value;
        }


        [HttpGet]
        public async Task<IActionResult> Get(string email)
        {
            try
            {
                PurchaseInvoice_New purchaseInvoice_New = new PurchaseInvoice_New();
                if (email == null)
                {
                    var response = new
                    {
                        status = true,
                        message = "Email is Mandatory field",
                    };
                    return Ok(response);
                }
                // BASIC COMMENT
                var list = await _dbContext.VwInvoiceApprovalLists.Where(x => x.ApproverEmail == email).ToListAsync();
                if (list.Count > 0)
                {
                    List<PurchaseInvoice_Response> purchaseInvoice_responsel = new List<PurchaseInvoice_Response>();
                    foreach (var k in list)
                    {
                        PurchaseInvoice_Response purchaseInvoice_response = new PurchaseInvoice_Response();
                        purchaseInvoice_response.HeaderStatusId = k.HeaderStatusId;
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
                        if (k.ImpiHeaderAttachment != null)
                        {
                            string[] valuesArray = k.ImpiHeaderAttachment.Split(',');

                            // Display the result
                            List<FicciImad> listing = new List<FicciImad>();

                            foreach (string value in valuesArray)
                            {

                                var path = await _dbContext.FicciImads.Where(x => x.ImadId == Convert.ToInt32(value)).FirstOrDefaultAsync();
                                listing.Add(path);

                            }
                            purchaseInvoice_response.ImpiHeaderAttachment = listing;
                        }

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
                        purchaseInvoice_response.HeaderStatus = _dbContext.StatusMasters.Where(x => x.StatusId == k.HeaderStatusId).Select(a => a.StatusName).FirstOrDefault();
                       // purchaseInvoice_response.WorkFlowHistory = _dbContext.FicciImwds.Where(x => x.CustomerId == purchaseInvoice_response.HeaderId && x.ImwdType == 2).ToList(); ;
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
                                lineItem_Request.ImpiLineAmount = l.ImpiLineAmount;
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
                return StatusCode(500, new { status = false, message = "An error occurred while fetching the list of Approval." });

            }
        }
        [HttpPost]
        public async Task<IActionResult> Post(ApproveInvoice cust)
        {
            ApproverInvoice_Crud crud = new ApproverInvoice_Crud();

            try
            {
                string htmlbody = "";
                var res = await _dbContext.GetProcedures().prc_Approval_InvoiceAsync(cust.HeaderId.ToString(), cust.IsApproved, cust.LoginId, cust.StatusId, cust.Remarks);
                if (res[0].returncode == 1)
                {
                   // var result = await _dbContext.FicciErpCustomerDetails.Where(x => x.CustomerId == Convert.ToInt32(res[0].CustomerId)).FirstOrDefaultAsync();
                  //  string subject = "";
                    //if (cust.IsApproved)
                    //{
                    //    if (cust.StatusId == 2)
                    //    {
                    //        subject = "New Customer Approved by TL : " + result.CustomerName + "";
                    //        htmlbody = ApprovalhtmlBody(res[0].Status, _mySettings.Website_link, result.CusotmerNo, result.CustomerName, result.CityCode, result.CustomerPanNo, result.CustomerGstNo);
                    //        SendEmail(result.CustomerClusterApprover, result.CustomerEmailId, subject, htmlbody, _mySettings);
                    //    }
                    //    else if (cust.StatusId == 3)
                    //    {
                    //        subject = "New Customer Approved by CH : " + result.CustomerName + "";
                    //        htmlbody = ApprovalhtmlBody(res[0].Status, _mySettings.Website_link, result.CusotmerNo, result.CustomerName, result.CityCode, result.CustomerPanNo, result.CustomerGstNo);
                    //        SendEmail(res[0].InitiatedBy, result.CustomerEmailId, subject, htmlbody, _mySettings);
                    //    }
                    //    else if (cust.StatusId == 11)
                    //    {
                    //        subject = "New Customer Approved by Finance : " + result.CustomerName + "";
                    //        htmlbody = ApprovalhtmlBody("approved by Finance approver", _mySettings.Website_link, result.CusotmerNo, result.CustomerName, result.CityCode, result.CustomerPanNo, result.CustomerGstNo);
                    //        SendEmail(result.CustomerEmailId, "" + result.CustomerTlApprover + "," + result.CustomerClusterApprover + "," + result.CustomerSgApprover + "", subject, htmlbody, _mySettings);
                    //        return StatusCode(200, crud);
                    //    }
                    //    else if (cust.StatusId == 5)
                    //    {
                    //        subject = "New Customer Approved by Account : " + result.CustomerName + "";
                    //        htmlbody = ApprovalhtmlBody("approved by Accounts approver", _mySettings.Website_link, result.CusotmerNo, result.CustomerName, result.CityCode, result.CustomerPanNo, result.CustomerGstNo);
                    //        SendEmail(result.CustomerEmailId, "" + result.CustomerTlApprover + "," + result.CustomerClusterApprover + "," + result.CustomerSgApprover + "", subject, htmlbody, _mySettings);
                    //        return StatusCode(200, crud);
                    //    }

                    //}
                    //if (!cust.IsApproved)
                    //{
                    //    if (cust.StatusId == 2)
                    //    {
                    //        subject = "New Customer Rejected by TL : " + result.CustomerName + "";
                    //    }
                    //    else if (cust.StatusId == 3)
                    //    {
                    //        subject = "New Customer Rejected by CH : " + result.CustomerName + "";
                    //    }
                    //    else if (cust.StatusId == 5)
                    //    {
                    //        subject = "New Customer Rejected by Account : " + result.CustomerName + "";
                    //    }
                    //    htmlbody = ApprovalhtmlBody("rejected by the approver", _mySettings.Website_link, result.CusotmerNo, result.CustomerName, result.CityCode, result.CustomerPanNo, result.CustomerGstNo);
                    //    SendEmail(result.CustomerEmailId, "" + result.CustomerTlApprover + "," + result.CustomerClusterApprover + "," + result.CustomerSgApprover + "", subject, htmlbody, _mySettings);
                    //    return StatusCode(200, crud);
                    //}



                }
                crud.status = res[0].returncode == 1 ? true : false;
                crud.message = res[0].Message;
                return StatusCode(200, crud);

            }
            catch (Exception ex)
            {
                crud.status = false;
                crud.message = ex.InnerException.Message.ToString();
                return StatusCode(500, crud);
            }
        }
    }
}
