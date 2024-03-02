using FICCI_API.DTO;
using FICCI_API.Models;
using FICCI_API.ModelsEF;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FICCI_API.Controller.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : BaseController
    {
        private readonly FICCI_DB_APPLICATIONSContext _dbContext;
        public AccountController(FICCI_DB_APPLICATIONSContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        [HttpGet]
        public async Task<IActionResult> Get(string loginid)
        {
            var result = new CustomerDTO();
            var resu = new List<CustomerList>();
            try
            {
                

                resu = await _dbContext.FicciErpCustomerDetails.Where(x => x.IsDelete != true && x.IsActive != false && x.CustomerStatus == 5 || x.ApprovedBy== loginid)
                        .Select(customer => new CustomerList
                        {
                            CustomerId = customer.CustomerId,
                            CustomerCode = customer.CusotmerNo,
                            CustomerName = customer.CustomerName,
                            CustomerLastName = customer.CustomerLastname,
                            Address = customer.CustoemrAddress,
                            Address2 = customer.CustoemrAddress2,
                            Contact = customer.CustomerContact,
                            Email = customer.CustomerEmailId,
                            PhoneNumber = customer.CustomerPhoneNo,
                            Pincode = customer.CustomerPinCode,
                            PAN = customer.CustomerPanNo,
                            GSTNumber = customer.CustomerGstNo,
                            IsActive = customer.IsActive,
                            CreatedBy = customer.Createdby,
                            CreatedOn = customer.CreatedOn,
                            LastUpdateBy = customer.LastUpdateBy,
                            ModifiedOn = Convert.ToDateTime(customer.CustomerUpdatedOn),
                            TLApprover = customer.CustomerTlApprover,
                            CLApprover = customer.CustomerClusterApprover,
                            CustomerStatus = _dbContext.StatusMasters.Where(x => x.StatusId == customer.CustomerStatus).Select(a => a.StatusName).FirstOrDefault(),
                            CustomerStatusId = customer.CustomerStatus,
                            ApprovedBy = customer.ApprovedBy,
                            ApprovedOn = customer.ApprovedOn,
                            GstType = customer.GstCustomerTypeNavigation == null ? null : new GSTCustomerTypeInfo
                            {
                                GstTypeId = customer.GstCustomerTypeNavigation.CustomerTypeId,
                                GstTypeName = customer.GstCustomerTypeNavigation.CustomerTypeName,
                            },
                            CityList = new CityInfo
                            {
                                cityCode = _dbContext.Cities.Where(x => x.CityCode == customer.CityCode && x.IsActive != false).Select(a => a.CityCode).FirstOrDefault(),
                                CityName = _dbContext.Cities.Where(x => x.CityCode == customer.CityCode && x.IsActive != false).Select(a => a.CityName).FirstOrDefault(),

                            },
                            StateList = new StateInfo
                            {
                                stateCode = _dbContext.States.Where(x => x.StateCode == customer.StateCode && x.IsActive != false).Select(a => a.StateCode).FirstOrDefault(),
                                StateName = _dbContext.States.Where(x => x.StateCode == customer.StateCode && x.IsActive != false).Select(a => a.StateName).FirstOrDefault(),
                            },
                            CountryList = new CountryInfo
                            {
                                countryCode = _dbContext.Countries.Where(x => x.CountryCode == customer.CountryCode && x.IsActive != false).Select(a => a.CountryCode).FirstOrDefault(),
                                CountryName = _dbContext.Countries.Where(x => x.CountryCode == customer.CountryCode && x.IsActive != false).Select(a => a.CountryName).FirstOrDefault()
                            },
                            
                        }).ToListAsync();

                if (resu.Count <= 0)
                {
                    var response = new
                    {
                        status = true,
                        message = "No customer list found",
                        data = resu
                    };
                    return Ok(response);
                }
                var respons = new
                {
                    status = true,
                    message = "Customer List fetch successfully",
                    data = resu
                };
                return Ok(respons);

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { status = false, message = "An error occurred while fetching the detail of Customers." });
            }
        }



        [HttpGet("GetInvoice")]
        public async Task<IActionResult> GetInvoice()
        {
            //var result = new PurchaseInvoice_Response();
            //var resu = new List<InvoiceList>();
            try
            {
                PurchaseInvoice_New purchaseInvoice_New = new PurchaseInvoice_New();
                var list = await _dbContext.FicciImpiHeaders.Where(x => x.ImpiHeaderActive != false && x.HeaderStatusId == 5).ToListAsync();

                if (list.Count > 0)
                {
                    List<PurchaseInvoice_Response> purchaseInvoice_responsel = new List<PurchaseInvoice_Response>();
                    foreach (var k in list)
                    {
                        PurchaseInvoice_Response purchaseInvoice_response = new PurchaseInvoice_Response();
                        purchaseInvoice_response.HeaderId = k.ImpiHeaderId;
                        purchaseInvoice_response.HeaderPiNo = k.ImpiHeaderPiNo;
                        purchaseInvoice_response.HeaderStatusId = k.HeaderStatusId;
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
                return StatusCode(500, new { status = false, message = "An error occurred while fetching the detail of Invoice." });
            }
        }

    }
}
