using Azure.Core;
using FICCI_API.DTO;
using FICCI_API.Models;
using FICCI_API.ModelsEF;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace FICCI_API.Controller.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : BaseController
    {
        private readonly FICCI_DB_APPLICATIONSContext _dbContext;
        private readonly MySettings _mySettings;
        private readonly IConfiguration _configuration;

        public CustomerController(IConfiguration configuration,FICCI_DB_APPLICATIONSContext dbContext, IOptions<MySettings> mySettings) : base(dbContext)
        {
            _dbContext = dbContext;
            _mySettings = mySettings.Value;
            _configuration = configuration;

        }

     
     
        [HttpGet]
        //List of all customers
        public async Task<IActionResult> Get(string email)
        {
            var result = new CustomerDTO();
            var resu = new List<CustomerList>();
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
                var emp_Role = await _dbContext.FicciImums.Where(x => x.ImumEmail == email).Select(a => a.RoleId).FirstOrDefaultAsync();

                resu = await _dbContext.FicciErpCustomerDetails.Where(x => x.IsDelete != true && x.IsActive != false).OrderByDescending(x => x.CreatedOn)
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
                            IsDraft = customer.IsDraft,
                            CreatedBy = customer.Createdby,
                            CreatedOn = customer.CreatedOn,
                            LastUpdateBy = customer.LastUpdateBy,
                            ModifiedOn = Convert.ToDateTime(customer.CustomerUpdatedOn),
                            TLApprover = customer.CustomerTlApprover,
                            CLApprover = customer.CustomerClusterApprover,
                            ApprovedBy = customer.ApprovedBy,
                            ApprovedOn = customer.ApprovedOn,
                            CityList = new CityInfo
                            {
                                cityCode = _dbContext.Cities.Where(x => x.CityCode == customer.CityCode && x.IsActive != false).Select(a => a.CityCode).FirstOrDefault(),
                                CityName = _dbContext.Cities.Where(x => x.CityCode == customer.CityCode && x.IsActive != false).Select(a => a.CityName).FirstOrDefault(),
                            },
                            StateList = new StateInfo
                            {
                                stateCode = _dbContext.States.Where(x => x.StateCode == customer.StateCode && x.IsActive != false).Select(a => a.StateCode).FirstOrDefault() ,
                                StateName = _dbContext.States.Where(x => x.StateCode == customer.StateCode && x.IsActive != false).Select(a => a.StateName).FirstOrDefault(),
                            },
                            CountryList = new CountryInfo
                            {
                                countryCode = _dbContext.Countries.Where(x => x.CountryCode == customer.CountryCode && x.IsActive != false).Select(a => a.CountryCode).FirstOrDefault(),
                                CountryName = _dbContext.Countries.Where(x => x.CountryCode == customer.CountryCode && x.IsActive != false).Select(a => a.CountryName).FirstOrDefault()
                            },

                            // SGApprover = customer.CustomerSgApprover,
                            // CustomerStatus = customer.CustomerStatus == 1 ? "Draft":"Pending With TL Approver",

                            CustomerStatus = _dbContext.StatusMasters.Where(x => x.StatusId == customer.CustomerStatus).Select(a => a.StatusName).FirstOrDefault(),
                            CustomerStatusId = customer.CustomerStatus,
                            GstType = customer.GstCustomerTypeNavigation == null ? null : new GSTCustomerTypeInfo
                            {
                                GstTypeId = customer.GstCustomerTypeNavigation.CustomerTypeId,
                                GstTypeName = customer.GstCustomerTypeNavigation.CustomerTypeName,
                            },

                            WorkFlowHistory = _dbContext.FicciImwds.Where(x => x.CustomerId == customer.CustomerId && x.ImwdType == 1).ToList()

                        }).ToListAsync();
                if (emp_Role != 1)
                {
                    resu = resu.Where(m => m.CreatedBy == email).ToList();
                }
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

        [HttpDelete("Delete/{customerId:int}")]
        public async Task<IActionResult> Delete(int customerId)
        {
            try
            {
                if (customerId <= 0)
                {
                    return BadRequest(new { status = false, message = "Invalid Id" });
                   
                }
                var res = await _dbContext.FicciErpCustomerDetails.Where(x => x.CustomerId == customerId).FirstOrDefaultAsync();

                if(res == null)
                {
                    return NotFound(new { status = false, message = "Customer not found" });
                }
                res.IsActive = false;
                res.IsDelete = true;
                await _dbContext.SaveChangesAsync();

                var response = new
                {
                    status = true,
                    message = "Delete successfully",
                    data = new object[] { }
                };

                return StatusCode(200, response);
                
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { status = false, message = "An error occurred." });
            }
        }


        [HttpPost]
        //Submit customer not drafted
        public async Task<IActionResult> Post(CustomerRequest data)
        {
            new_Customer request = new new_Customer();
            using (IDbContextTransaction transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    if (data != null)
                    {
                        FicciErpCustomerDetail customer = new FicciErpCustomerDetail();
                        if (!data.isupdate)
                        {

                         
                            customer.CusotmerNo = data.CustomerCode;
                            customer.CustoemrAddress = data.Address;
                            customer.CustoemrAddress2 = data.Address2;
                            customer.CustomerContact = data.Contact;
                            customer.CustomerName = data.CustomerName;
                            customer.CustomerLastname = data.CustomerLastName;
                            customer.CustomerGstNo = data.GSTNumber;
                            customer.CustomerEmailId = data.Email;
                            customer.CustomerPinCode = data.PinCode;
                            customer.CustomerPanNo = data.PAN;
                            customer.CustomerUpdatedOn = DateTime.Now;
                            //customer.CustomerCity = data.Cityid;
                            customer.CustomerPhoneNo = data.Phone;
                            customer.GstCustomerType = data.GSTCustomerType;
                            customer.IsPending = true;
                            customer.IsDraft = data.IsDraft;
                            customer.Createdby = data.LoginId;
                            customer.CustomerStatus = data.IsDraft == true ? 1 : 5;
                            customer.CityCode = data.CityCode;
                            customer.CountryCode = data.CountryCode;
                            customer.StateCode = data.StateCode;
                            customer.CustomerRemarks = data.CustomerRemarks;
                            customer.CustomerTlApprover = _dbContext.FicciImems.Where(x => x.ImemEmail == data.LoginId).Select(x => x.ImemManagerEmail).FirstOrDefault().ToString() == null
                                ? null : _dbContext.FicciImems.Where(x => x.ImemEmail == data.LoginId).Select(x => x.ImemManagerEmail).FirstOrDefault().ToString();

                            customer.CustomerClusterApprover = _dbContext.FicciImems.Where(x => x.ImemEmail == data.LoginId).Select(x => x.ImemClusterEmail).FirstOrDefault().ToString() == null
                                 ? null : _dbContext.FicciImems.Where(x => x.ImemEmail == data.LoginId).Select(x => x.ImemClusterEmail).FirstOrDefault().ToString();
                            var k = _dbContext.FicciImems.Where(x => x.ImemEmail == data.LoginId).Select(x => x.ImemDepartmentHeadEmail).FirstOrDefault();
                            if(k != null)
                            {
                                customer.CustomerSgApprover = _dbContext.FicciImems.Where(x => x.ImemEmail == data.LoginId).Select(x => x.ImemDepartmentHeadEmail).FirstOrDefault();

                            }                            

                            _dbContext.Add(customer);
                            _dbContext.SaveChanges();
                            int returnId = customer.CustomerId;

                            
                                if (!data.IsDraft)
                                {

                                    NavERPController navERPController = new NavERPController(_configuration, _dbContext);

                                    CustomerPost PostData = new CustomerPost();

                                    PostData.Name = data.CustomerName;
                                    PostData.Name2 = data.CustomerLastName;
                                    PostData.Address = data.Address;
                                    PostData.Address2 = data.Address2;
                                    PostData.City = data.CityCode;
                                    PostData.State_Code = "";//data.StateCode;
                                    PostData.Country_Region_Code = "";// data.CountryCode;

                                    PostData.Contact = data.Contact;
                                    PostData.PostCode = data.PinCode;

                                    PostData.GSTCustomerType = ""; ;// data.GSTCustomerType.ToString();
                                    PostData.GSTRegistrationNo = "";//data.GSTNumber;
                                    PostData.PAN_No = "";//data.PAN;
                                    PostData.EMail = "";//data.Email;
                                    PostData.PrimaryContactNo = "";//data.Phone;
                                    //PostData.Name = "Ashish Raghav";
                                    //PostData.Name2 = "Devloper ok 3";
                                    //PostData.Address = "sec 137";
                                    //PostData.Address2 = "NOida";
                                    //PostData.City = "Noida";
                                    //PostData.State_Code = "";
                                    //PostData.Country_Region_Code = "";

                                    //PostData.Contact = "878789898";
                                    //PostData.PostCode = "201301";

                                    //PostData.GSTCustomerType = " ";
                                    //PostData.GSTRegistrationNo = "";
                                    //PostData.PAN_No = "";
                                    //PostData.EMail = "";
                                    //PostData.PrimaryContactNo = "";

                                    dynamic erpResponse = await navERPController.PostCustomer(PostData);
                                    var updatedNumber = erpResponse.Data.No;
                                    var resu = _dbContext.FicciErpCustomerDetails.Where(x => x.CustomerId == returnId).FirstOrDefault();
                                    resu.CusotmerNo = updatedNumber;
                                    _dbContext.Update(resu);
                                    _dbContext.SaveChanges();
                                    //PostData.Status = true;
                                    //PostData.Message = "Customer Update Successfully";
                                    // return StatusCode(200, PostData);
                                }


                            






                            int returnid = customer.CustomerId;

                            FicciImwd imwd = new FicciImwd();
                            imwd.ImwdScreenName = "Customer Approver";
                            imwd.CustomerId = returnid;
                            imwd.ImwdCreatedOn = DateTime.Now;
                            imwd.ImwdCreatedBy = data.LoginId;
                            imwd.ImwdStatus = data.IsDraft == true ? "1" : "5";
                            imwd.ImwdPendingAt = _dbContext.StatusMasters.Where(x => x.StatusId == customer.CustomerStatus).Select(a => a.StatusName).FirstOrDefault();
                            imwd.ImwdInitiatedBy = data.LoginId;
                            imwd.ImwdRemarks = data.CustomerRemarks;
                            imwd.ImwdRole = data.RoleName;
                            imwd.ImwdType = 1;
                            _dbContext.Add(imwd);

                            _dbContext.SaveChanges();
                            transaction.Commit();
                            if (data.IsDraft == false)
                            {
                                string htmlbody = AssignhtmlBody(_mySettings.Website_link, customer.CustomerName, customer.CityCode, customer.CustomerPanNo, customer.CustomerGstNo,customer.CustomerContact, customer.CustomerPhoneNo);
                                SendEmail(customer.CustomerTlApprover, customer.CustomerEmailId, $"New Customer Assigned for Approval : {customer.CustomerName}", htmlbody, _mySettings);
                            }
                            request.Status = true;
                            request.Message = "Customer Insert Successfully";
                            return StatusCode(200, request);
                        }
                        else
                        {
                            var result = await _dbContext.FicciErpCustomerDetails.Where(x => x.CustomerId == data.CustomerId).FirstOrDefaultAsync();
                            if (result != null)
                            {
                                result.CustoemrAddress = data.Address;
                                result.CustoemrAddress2 = data.Address2;
                                result.CusotmerNo = data.CustomerCode;
                                result.CustomerContact = data.Contact;
                                result.CustomerName = data.CustomerName;
                                result.CustomerLastname = data.CustomerLastName;
                                result.CustomerGstNo = data.GSTNumber;
                                result.CustomerEmailId = data.Email;
                                result.CustomerPinCode = data.PinCode;
                                result.CustomerPanNo = data.PAN;
                                result.CustomerRemarks = data.CustomerRemarks;
                                // result.CustomerCity = data.Cityid;
                                result.CustomerPhoneNo = data.Phone;
                                result.GstCustomerType = data.GSTCustomerType;
                                result.IsPending = true;
                                result.IsDraft = data.IsDraft;
                                result.LastUpdateBy = data.LoginId;
                                result.CustomerUpdatedOn = DateTime.Now;
                                result.CustomerStatus = data.IsDraft == true ? 1 : 5;
                                result.CityCode = data.CityCode;
                                result.CountryCode = data.CountryCode;
                                result.StateCode = data.StateCode;

                                _dbContext.SaveChanges();

                                FicciImwd imwd = new FicciImwd();
                                imwd.ImwdScreenName = "Customer Approver";
                                imwd.CustomerId = data.CustomerId;
                                imwd.ImwdCreatedOn = DateTime.Now;
                                imwd.ImwdCreatedBy = data.LoginId;
                                imwd.ImwdStatus = data.CustomerStatus.ToString();
                                imwd.ImwdPendingAt = _dbContext.StatusMasters.Where(x => x.StatusId == result.CustomerStatus).Select(a => a.StatusName).FirstOrDefault();
                                imwd.ImwdInitiatedBy = data.LoginId;
                                imwd.ImwdRemarks = data.CustomerRemarks;
                                imwd.ImwdRole = data.RoleName;
                                imwd.ImwdType = 1;
                                _dbContext.Add(imwd);

                                _dbContext.SaveChanges();

                             


                                transaction.Commit();
                                if (data.IsDraft == false)
                                {
                                    string htmlbody = AssignhtmlBody(_mySettings.Website_link, customer.CustomerName, customer.CityCode, customer.CustomerPanNo, customer.CustomerGstNo, customer.CustomerContact, customer.CustomerPhoneNo);
                                    SendEmail(customer.CustomerTlApprover, customer.CustomerEmailId, $"New Customer Assigned for Approval : {customer.CustomerName}", htmlbody, _mySettings);
                                }
                                request.Status = true;
                                request.Message = "Customer Update Successfully";
                                return StatusCode(200, request);
                            }
                            else
                            {
                                request.Status = true;
                                request.Message = "Customer not found";
                                return Ok(request);
                            }
                        }
                    }
                    else
                    {
                        request.Status = false;
                        request.Message = "Invalid data";
                        return StatusCode(404, request);
                    }

                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return StatusCode(500, new { status = false, message = "An error occurred while fetching the detail of Customers." });
                }
            }
        }


        //public async Task<IActionResult> PostDataInNav(CustomerRequest data)
        //{
        //    try
        //    {
        //        using (IDbContextTransaction transaction = _dbContext.Database.BeginTransaction())
        //        {
        //            CustomerPost PostData = new CustomerPost();

        //            PostData.Name = data.CustomerName;
        //            PostData.Name2 = data.CustomerLastName;
        //            PostData.Address = data.Address;
        //            PostData.Address2 = data.Address2;
        //            PostData.City = data.CityCode;
        //            PostData.State_Code = data.StateCode;
        //            PostData.Country_Region_Code = data.CountryCode;

        //            PostData.Contact = data.Contact;
        //            PostData.PostCode = data.PinCode;

        //            PostData.GSTCustomerType = data.GSTCustomerType.ToString();
        //            PostData.GSTRegistrationNo = data.GSTNumber;
        //            PostData.PAN_No = data.PAN;
        //            PostData.EMail = data.Email;
        //            PostData.PrimaryContactNo = data.Phone;

        //            _NavERPController.PostCustomer(PostData);
        //            transaction.Commit();

        //            PostData.Status = true;
        //            PostData.Message = "Customer Update Successfully";
        //            return StatusCode(200, PostData);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //transaction.Rollback();
        //        return StatusCode(500, new { status = false, message = "An error occurred while fetching the detail of Customers." });
        //    }

        //}
    }



}

