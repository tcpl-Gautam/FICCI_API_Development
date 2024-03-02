using Azure.Core;
using FICCI_API.Models;
using FICCI_API.ModelsEF;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Drawing;
using System.IO;
using System.Linq;

namespace FICCI_API.Controller.API
{

    [Route("api/[controller]")]
    [ApiController]
    public class PurchaseInvoice_NewController : BaseController
    {
        private readonly FICCI_DB_APPLICATIONSContext _dbContext;
        public PurchaseInvoice_NewController(FICCI_DB_APPLICATIONSContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
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
                        if (request.ImpiHeaderAttachment != null)
                        {
                            ficciImpiHeader.ImpiHeaderAttachment = UploadFile(request.ImpiHeaderAttachment, request.LoginId);
                        }
                        ficciImpiHeader.ImpiHeaderPaymentTerms = request.ImpiHeaderPaymentTerms;
                        ficciImpiHeader.ImpiHeaderRemarks = request.ImpiHeaderRemarks;
                        ficciImpiHeader.ImpiHeaderStatus = request.IsDraft == true ? "Draft" : "Pending";
                        ficciImpiHeader.IsDraft = request.IsDraft;

                        ficciImpiHeader.ImpiHeaderSubmittedDate = DateTime.Now;
                        ficciImpiHeader.ImpiHeaderTlApprover = "amit.jha@teamcomputers.com";//request.ImpiHeaderTlApprover + "@ficci.com";
                        ficciImpiHeader.ImpiHeaderClusterApprover = "debananda.panda@teamcomputers.com";//request.ImpiHeaderClusterApprover + "@ficci.com";
                        ficciImpiHeader.ImpiHeaderFinanceApprover = "gautam.v@teamcomputers.com";//request.ImpiHeaderFinanceApprover + "@ficci.com";
                        if (request.ImpiHeaderSupportApprover != null)
                        {
                            ficciImpiHeader.ImpiHeaderSupportApprover = request.ImpiHeaderSupportApprover + "@ficci.com";
                        }
                        ficciImpiHeader.HeaderStatusId = request.IsDraft == true ? 1 : 2;

                        _dbContext.Add(ficciImpiHeader);
                        _dbContext.SaveChanges();
                        int returnid = ficciImpiHeader.ImpiHeaderId;
                        FicciImwd imwd = new FicciImwd();
                        imwd.ImwdScreenName = "Invoice Approver";
                        imwd.CustomerId = returnid;
                        imwd.ImwdCreatedOn = DateTime.Now;
                        imwd.ImwdCreatedBy = request.LoginId;
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
                            if (request.ImpiHeaderAttachment != null)
                            {
                                data.ImpiHeaderAttachment = UploadFile(request.ImpiHeaderAttachment, request.LoginId);
                            }

                            data.ImpiHeaderPaymentTerms = request.ImpiHeaderPaymentTerms;
                            data.ImpiHeaderRemarks = request.ImpiHeaderRemarks;
                            data.ImpiHeaderStatus = request.IsDraft == true ? "Draft" : "Pending";
                            data.IsDraft = request.IsDraft;
                            data.HeaderStatusId = request.IsDraft == true ? 1 : 2;


                            //_dbContext.Add(data);
                            _dbContext.SaveChanges();
                            int returnid = data.ImpiHeaderId;
                            FicciImwd imwd = new FicciImwd();
                            imwd.ImwdScreenName = "Invoice Approver";
                            imwd.CustomerId = returnid;
                            imwd.ImwdCreatedOn = DateTime.Now;
                            imwd.ImwdCreatedBy = request.LoginId;
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
                                var dataline = _dbContext.FicciImpiLines.ToList();
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
                    list = list.Where(m => m.ImpiHeaderCreatedBy == email).ToList();
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

                        //error in auto generated model ImpiHeaderAttachment is not null
                        if (k.ImpiHeaderAttachment != null)
                        {
                            string[]? valuesArray = k.ImpiHeaderAttachment.Split(',');

                            // Display the result
                            List<FicciImad> listing = new List<FicciImad>();

                            foreach (string value in valuesArray)
                            {

                                var path = await _dbContext.FicciImads.Where(x => x.ImadId == Convert.ToInt32(value) && x.ImadActive !=false).FirstOrDefaultAsync();
                                if (path != null)
                                {

                                    listing.Add(path);
                                }
                            }

                            purchaseInvoice_response.ImpiHeaderAttachment = listing;
                        }
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


        [NonAction]
        public string UploadFile(List<IFormFile>? file1, string loginId)
        {
            string timestamp = DateTime.Now.ToString("yyyyMMddhhmmss");
            string fileids = "";
            if (file1 == null || !file1.Any())
            {
                return null; // Handle invalid file
            }

            foreach (var file in file1)
            {
                // Generate a unique filename to avoid conflicts
                string uniqueFileName = timestamp;
                var fileExtension = Path.GetExtension(file.FileName);
                string folderpath = Path.Combine("wwwroot", "PurchaseInvoice");
                // Combine the path where you want to store the file with the unique filename
                string filePath = Path.Combine("wwwroot", "PurchaseInvoice", uniqueFileName + fileExtension);
                string savefilePath = Path.Combine("PurchaseInvoice", uniqueFileName + fileExtension);
                if (!Directory.Exists(folderpath))
                {
                    // The folder does not exist, so create it
                    Directory.CreateDirectory(folderpath);

                }
                // Save the file to the specified path
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }
                FileInfoModel fileInfoModel = new FileInfoModel();



                fileInfoModel.FileName = file.FileName;
                fileInfoModel.Size = file.Length;
                fileInfoModel.ContentType = file.ContentType;
                int reurnId = FileMethod(fileInfoModel.FileName, fileInfoModel.Size, fileInfoModel.ContentType, savefilePath, loginId);
                // Return the file path
                fileids += reurnId + ",";
            }
            return fileids.TrimEnd(',');
        }
        [NonAction]
        public int FileMethod(string fileName, long length, string contentType, string path, string loginId)
        {
            using (IDbContextTransaction transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    FicciImad imad = new FicciImad();
                     imad.ImadCreatedBy = loginId;
                    imad.ImadCreatedOn = DateTime.Now;
                    imad.ImadActive = true;
                    imad.ImadFileName = fileName;
                    imad.ImadFileSize = length.ToString();
                    imad.ImadFileType = contentType;
                    imad.ImadFileUrl = path;
                    imad.ImadScreenName = "Invoice";
                    _dbContext.Add(imad);
                    _dbContext.SaveChanges();
                    int returnId = imad.ImadId;
                    transaction.Commit();
                    return returnId;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw;
                }
            }

        }

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
