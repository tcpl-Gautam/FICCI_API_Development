using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using FICCI_API.ModelsEF;
using FICCI_API.DTO;
using FICCI_API.Models;
using Microsoft.Extensions.Options;

namespace FICCI_API.Controller.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApproveCustomerController : BaseController
    {
        private readonly FICCI_DB_APPLICATIONSContext _dbContext;
        private readonly MySettings _mySettings;
        public ApproveCustomerController(FICCI_DB_APPLICATIONSContext dbContext, IOptions<MySettings> mySettings) : base(dbContext)
        {
            _dbContext = dbContext;
            _mySettings = mySettings.Value;
        }

  


        [HttpGet]
        public async Task<IActionResult> Get(string email)
        {
            try
            {
                Approval_Customer approverCustomer = new Approval_Customer();
                if (email == null)
                {
                    var response = new
                    {
                        status = true,
                        message = "Email is Mandatory field",
                    };
                    return Ok(response);
                }

                var list = await _dbContext.VwCustomerApprovalLists.Where(x => x.ApproverEmail == email).ToListAsync();
                if(list.Count > 0)
                {
                    List<Approval_CustomerValue> custom = new List<Approval_CustomerValue>();
                    foreach(var k in list)
                    {
                        Approval_CustomerValue cust = new Approval_CustomerValue();
                        cust.CustomerId = k.CustomerId;
                        cust.CustomerName = k.CustomerName;
                        cust.CustomerLastName = k.CustomerLastname;
                        cust.CustomerCode = k.CusotmerNo;
                        cust.Address = k.CustoemrAddress;
                        cust.Address2 = k.CustoemrAddress2;
                        cust.PhoneNumber = k.CustomerPhoneNo;
                        cust.Email = k.CustomerEmailId;
                        cust.Pincode = k.CustomerPinCode;
                        cust.Contact = k.CustomerContact;
                        cust.GSTNumber = k.CustomerGstNo;
                        cust.PAN = k.CustomerPanNo;
                        cust.IsActive = k.IsActive;
                        cust.CreatedBy = k.Createdby;
                        cust.CreatedOn = k.CreatedOn;
                        cust.TLApprover = k.CustomerTlApprover;
                        cust.CLApprover = k.CustomerClusterApprover;
                        cust.CustomerStatus = k.StatusName;
                        cust.CustomerStatusId = k.CustomerStatus;
                        cust.ModifiedOn = k.CustomerUpdatedOn;
                        cust.IsDraft = k.IsDraft;
                        cust.LastUpdateBy = k.LastUpdateBy;
                        cust.CityList = new CityInfo
                        {
                            cityCode = _dbContext.Cities.Where(x => x.CityCode == k.CityCode && x.IsActive != false).Select(a => a.CityCode).FirstOrDefault(),
                            CityName = _dbContext.Cities.Where(x => x.CityCode == k.CityCode && x.IsActive != false).Select(a => a.CityName).FirstOrDefault(),

                        };
                        cust.StateList = new StateInfo
                        {
                            stateCode = _dbContext.States.Where(x => x.StateCode == k.StateCode && x.IsActive != false).Select(a => a.StateCode).FirstOrDefault(),
                            StateName = _dbContext.States.Where(x => x.StateCode == k.StateCode && x.IsActive != false).Select(a => a.StateName).FirstOrDefault(),
                        };
                        cust.CountryList = new CountryInfo
                        {
                            countryCode = _dbContext.Countries.Where(x => x.CountryCode == k.CountryCode && x.IsActive != false).Select(a => a.CountryCode).FirstOrDefault(),
                            CountryName = _dbContext.Countries.Where(x => x.CountryCode == k.CountryCode && x.IsActive != false).Select(a => a.CountryName).FirstOrDefault()
                        };
                        cust.GstType = new GSTCustomerTypeInfo
                        {
                            GstTypeId = _dbContext.GstCustomerTypes.Where(x => x.CustomerTypeId == k.GstCustomerType && x.IsActive != false).Select(a => a.CustomerTypeId).FirstOrDefault(),
                            GstTypeName = _dbContext.GstCustomerTypes.Where(x => x.CustomerTypeId == k.GstCustomerType && x.IsActive != false).Select(a => a.CustomerTypeName).FirstOrDefault(),
                        };
                        custom.Add(cust);
                    }
                    approverCustomer.Status = true;
                    approverCustomer.Data = custom;
                    approverCustomer.Message = "Customer list successfully";
                    return StatusCode(200, approverCustomer);
                }
                else
                {
                    approverCustomer.Status = false;
                    approverCustomer.Message = "No Data found";
                    return StatusCode(200, approverCustomer);
                }
                
            }
            catch(Exception ex)
            {
                return StatusCode(500, new { status = false, message = "An error occurred while fetching the list of Approval." });

            }
        }
        [HttpPost]
        public async Task<IActionResult> Post(ApproveCustomer cust)
        {
            ApproverCustomer_Crud crud = new ApproverCustomer_Crud();

            try
            {
                string htmlbody = "";
                var res = await _dbContext.GetProcedures().prc_Approval_CustomerAsync(cust.CustomerId.ToString(), cust.IsApproved, cust.LoginId, cust.StatusId, cust.Remarks);
                if (res[0].returncode == 1)
                {
                    var result = await _dbContext.FicciErpCustomerDetails.Where(x => x.CustomerId == Convert.ToInt32(res[0].CustomerId)).FirstOrDefaultAsync();
                    string subject = "";
                    crud.status = res[0].returncode == 1 ? true : false;
                    crud.message = res[0].Message;
                    if (cust.IsApproved)
                    {
                        if (cust.StatusId == 2)
                        {
                            subject = "New Customer Approved by TL : " + result.CustomerName + "";
                            htmlbody = ApprovalhtmlBody(res[0].Status, _mySettings.Website_link, result.CustomerName, result.CityCode, result.CustomerPanNo, result.CustomerGstNo, result.CustomerContact,result.CustomerPhoneNo);
                            //  SendEmail(result.CustomerClusterApprover, result.CustomerEmailId, subject, htmlbody, _mySettings);
                            MailMethod(result.CustomerClusterApprover, result.CustomerEmailId, subject, htmlbody, "Customer", cust.LoginId, Convert.ToInt32(res[0].CustomerId));

                        }
                        else if (cust.StatusId == 3)
                        {
                            subject = "New Customer Approved by CH : " + result.CustomerName + "";
                            htmlbody = ApprovalhtmlBody(res[0].Status, _mySettings.Website_link, result.CustomerName, result.CityCode, result.CustomerPanNo, result.CustomerGstNo, result.CustomerContact, result.CustomerPhoneNo);
                            // SendEmail(res[0].InitiatedBy, result.CustomerEmailId, subject, htmlbody, _mySettings);
                            MailMethod(res[0].InitiatedBy, result.CustomerEmailId, subject, htmlbody, "Customer", cust.LoginId, Convert.ToInt32(res[0].CustomerId));

                        }
                        else if (cust.StatusId == 5)
                        {
                            subject = "New Customer Approved by Account : " + result.CustomerName + "";
                            htmlbody = ApprovalhtmlBody("approved by Accounts approver", _mySettings.Website_link, result.CustomerName, result.CityCode, result.CustomerPanNo, result.CustomerGstNo, result.CustomerContact, result.CustomerPhoneNo);
                           // SendEmail(result.CustomerEmailId, "" + result.CustomerTlApprover + "," + result.CustomerClusterApprover + "," + result.CustomerSgApprover + "", subject, htmlbody, _mySettings);
                            MailMethod(result.CustomerEmailId, "" + result.CustomerTlApprover + "," + result.CustomerClusterApprover + "," + result.CustomerSgApprover + "", subject, htmlbody, "Customer", cust.LoginId, Convert.ToInt32(res[0].CustomerId));


                            return StatusCode(200, crud);
                        }

                    }
                    if (!cust.IsApproved)
                    {
                        if (cust.StatusId == 2)
                        {
                            subject = "New Customer Rejected by TL : " + result.CustomerName + "";
                        }
                        else if (cust.StatusId == 3)
                        {
                            subject = "New Customer Rejected by CH : " + result.CustomerName + "";
                        }
                        else if (cust.StatusId == 5)
                        {
                            subject = "New Customer Rejected by Account : " + result.CustomerName + "";
                        }
                        htmlbody = ApprovalhtmlBody("rejected by the approver", _mySettings.Website_link, result.CustomerName, result.CityCode, result.CustomerPanNo, result.CustomerGstNo, result.CustomerContact, result.CustomerPhoneNo);
                       // SendEmail(result.CustomerEmailId, "" + result.CustomerTlApprover + "," + result.CustomerClusterApprover + "," + result.CustomerSgApprover + "", subject, htmlbody, _mySettings);
                        MailMethod(result.CustomerEmailId, "" + result.CustomerTlApprover + "," + result.CustomerClusterApprover + "," + result.CustomerSgApprover + "", subject, htmlbody, "Customer", cust.LoginId, Convert.ToInt32(res[0].CustomerId));
                        return StatusCode(200, crud);
                    }



                }
              
                return StatusCode(200, crud);

            }
            catch (Exception ex)
            {
                crud.status = false;
                crud.message = ex.InnerException.Message.ToString();
                return StatusCode(500, crud);
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
                bool isEmailSent = SendEmailData(mailTo, mailCC, mailSubject, mailBody, _mySettings,null);

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
    }
}
