using FICCI_API.DTO;
using FICCI_API.Interface;
using FICCI_API.Models;
using FICCI_API.Models.Services;
using FICCI_API.ModelsEF;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml.Linq;
using System.Net.Http;
using System.Globalization;
using System.Linq;
using System.Text;
using Azure.Core;
using System.Reflection.Metadata;

namespace FICCI_API.Controller.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class NavERPController : BaseController
    {
        private readonly FICCI_DB_APPLICATIONSContext _dbContext;
        private readonly IConfiguration _configuration;
        private readonly string _erpServer;
        private readonly string _navURL;
        private readonly string _navServiceURL;
        private readonly string _endURL;
        ApiResponseModel _responseModel;
        public NavERPController(IConfiguration configuration, FICCI_DB_APPLICATIONSContext dbContext) : base(dbContext)
        {
            _configuration = configuration;
            _navURL = _configuration["ERP:URL"] ?? "https://api.businesscentral.dynamics.com/v2.0/d3a55687-ec5c-433b-9eaa-9d952c913e94";
            _erpServer = _configuration["ERP:Server"] ?? "FICCI_CRM";
            _endURL = _configuration["ERP:EndURL"] ?? "ODataV4/Company('FICCI')";
            _navServiceURL = $"{_navURL}/{_erpServer}/{_endURL}";
            _responseModel = new ApiResponseModel();
            _dbContext = dbContext;
        }

        [HttpGet("GetCountry")]
        public async Task<IActionResult> GetCountry()
        {
            try
            {
                #region Old Code
                //string result = await GetSecurityToken();
                //var httpClient = new HttpClient();
                //httpClient.DefaultRequestHeaders.Add("Authorization", result);
                //string serviceURL = $"{_navServiceURL}/{_configuration["ERP:Services:Country"]}";
                //var response = await httpClient.GetAsync(serviceURL);
                //response.EnsureSuccessStatusCode();
                //var responseContent = response.Content.ReadAsStringAsync().Result;
                //List<CountryMaster> countryList = JsonConvert.DeserializeObject<ODataResponse<CountryMaster>>(responseContent).Value.ToList();
                #endregion


                List<CountryMaster> countryList = await GetList<CountryMaster>("Country");
                var apiResponse = new
                {
                    data = countryList,
                    status = true,
                    message = $"{countryList.Count} records found.",
                };

                //   var country = await _dbContext.Countries.Where(x => x.IsDelete != true && x.IsActive != false).OrderBy(x => x.CountryName).ToListAsync();
                if (countryList.Count > 0)
                {
                    var countryResponse = countryList.Select(c => new CountryInfo
                    {
                        CountryId = c.Code,
                        CountryName = c.Name,
                    }).ToList();
                    _dbContext.Database.ExecuteSqlRaw($"TRUNCATE TABLE Country");
                    foreach (var k in countryResponse)
                    {
                        Country country = new Country();
                        country.CountryCode = k.CountryId;
                        country.CountryName = k.CountryName;
                        _dbContext.Add(country);
                        _dbContext.SaveChanges();
                    }


                    var response = new
                    {
                        status = true,
                        message = "Country List fetch successfully",
                        data = countryResponse
                    };
                    return Ok(response);

                }
                else
                {
                    var response = new
                    {
                        status = true,
                        message = "No Country list found",
                        data = countryList
                    };
                    return NotFound(response);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }


            //List<CountryMaster> countryList = await GetList<CountryMaster>("Country");
            //    var apiResponse = new
            //    {
            //        data = countryList,
            //        status = true,
            //        message = $"{countryList.Count} records found.",
            //    };
            //    return Ok(apiResponse);
            //}
            //catch (Exception ex)
            //{
            //    return StatusCode(500, new { status = false, message = ex.Message });
            //}
        }

        [HttpGet("GetState")]
        public async Task<IActionResult> GetState()
        {
            try
            {
                List<StateMaster> stateList = await GetList<StateMaster>("State");

                var apiResponse = new
                {
                    data = stateList,
                    status = true,
                    message = $"{stateList.Count} records found.",
                };

                if (stateList.Count > 0)
                {
                    var stateResponse = stateList.Select(c => new StateInfo
                    {
                        StateId = c.Code,
                        StateName = c.Name,
                    }).ToList();
                    _dbContext.Database.ExecuteSqlRaw($"TRUNCATE TABLE States");
                    foreach (var k in stateResponse)
                    {
                        State state = new State();
                        state.StateCode = k.StateId;
                        state.StateName = k.StateName;
                        _dbContext.Add(state);
                        _dbContext.SaveChanges();
                    }
                    var response = new
                    {
                        status = true,
                        message = "State List fetch successfully",
                        data = stateResponse
                    };
                    return Ok(response);

                }
                else
                {
                    var response = new
                    {
                        status = true,
                        message = "No State list found",
                        data = stateList
                    };
                    return Ok(response);
                }




                //var apiResponse = new
                //{
                //    data = stateList,
                //    status = true,
                //    message = $"{stateList.Count} records found.",
                //};
                //return Ok(apiResponse);

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { status = false, message = "An error occurred while fetching the detail." });
            }
        }


        [HttpGet("GetCity")]
        public async Task<IActionResult> GetCity()
        {
            try
            {
                List<CityMaster> cityList = await GetList<CityMaster>("City");


                var apiResponse = new
                {
                    data = cityList,
                    status = true,
                    message = $"{cityList.Count} records found.",
                };
                // return Ok(apiResponse);


                if (cityList.Count > 0)
                {
                    var cityResponse = cityList.Select(c => new CityInfo
                    {
                        CityId = c.Code,
                        CityName = c.Name,
                    }).ToList();
                    _dbContext.Database.ExecuteSqlRaw($"TRUNCATE TABLE City");
                    foreach (var k in cityResponse)
                    {
                        City city = new City();
                        city.CityCode = k.CityId;
                        city.CityName = k.CityName;
                        _dbContext.Add(city);
                        _dbContext.SaveChanges();
                    }
                    var response = new
                    {
                        status = true,
                        message = "City List fetch successfully",
                        data = cityResponse
                    };
                    return Ok(response);

                }
                else
                {
                    var response = new
                    {
                        status = true,
                        message = "No City list found",
                        data = cityList
                    };
                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            //var apiResponse = new
            //    {
            //        data = cityList,
            //        status = true,
            //        message = $"{cityList.Count} records found.",
            //    };
            //    return Ok(apiResponse);

            //}
            //catch (Exception ex)
            //{
            //    return StatusCode(500, new { status = false, message = "An error occurred while fetching the detail." });
            //}
        }

        [HttpGet("GetPostCode")]
        public async Task<IActionResult> GetPostCode()
        {
            try
            {
                List<PostCodeMaster> cityList = await GetList<PostCodeMaster>("PostCode");
                _dbContext.Database.ExecuteSqlRaw($"TRUNCATE TABLE PostCode");
                foreach (var k in cityList)
                {
                    PostCode code = new PostCode();
                    code.PostCode1 = k.PostCode;
                    code.City = k.City;
                    _dbContext.Add(code);
                    _dbContext.SaveChanges();
                }
                var apiResponse = new
                {
                    data = cityList,
                    status = true,
                    message = $"{cityList.Count} records found.",
                };
                return Ok(apiResponse);

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { status = false, message = "An error occurred while fetching the detail." });
            }
        }


        [HttpGet("GetCustomer")]
        public async Task<IActionResult> GetCustomer()
        {
            try
            {
                List<CustomerModel> CustomerList = await GetList<CustomerModel>("Customer");
                _dbContext.Database.ExecuteSqlRaw($"TRUNCATE TABLE ERPCustomer");
                foreach (var k in CustomerList)
                {
                    Erpcustomer cust = new Erpcustomer();
                    cust.CustNo = k.No;
                    cust.CustName = k.Name;
                    cust.CustName2 = k.Name2;
                    cust.CustAddress = k.Address;
                    cust.CustAddress2 = k.Address2;
                    cust.City = k.City;
                    cust.StateCode = k.State_Code;
                    cust.PinCode = k.PinCode;
                    cust.CountryRegionCode = k.Country_Region_Code;
                    cust.GstregistrationNo = k.GSTRegistrationNo;
                    cust.GstcustomerType = k.GSTCustomerType;
                    cust.Contact = k.Contact;
                    cust.PanNo = k.PAN_No;
                    cust.Email = k.EMail;
                    cust.PrimaryContactNo = k.PrimaryContactNo;
                    _dbContext.Add(cust);
                    _dbContext.SaveChanges();
                }
                var apiResponse = new
                {
                    data = CustomerList,
                    status = true,
                    message = $"{CustomerList.Count} records found.",
                };
                return Ok(apiResponse);

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { status = false, message = "An error occurred while fetching the detail." });
            }
        }

        [HttpGet("GetEmployee")]
        public async Task<IActionResult> GetEmployee()
        {
            try
            {
                List<EmployeeModel> cityList = await GetList<EmployeeModel>("Employee");

                var apiResponse = new
                {
                    data = cityList,
                    status = true,
                    message = $"{cityList.Count} records found.",
                };
                return Ok(apiResponse);

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { status = false, message = "An error occurred while fetching the detail." });
            }
        }

        [HttpGet("GetCOAMaster")]
        public async Task<IActionResult> GetCOAMaster()
        {
            try
            {
                List<COAMaster> COAMaster = await GetList<COAMaster>("COA");
                if (COAMaster.Count > 0)
                {
                    var coaResponse = COAMaster.Select(c => new COAInfo
                    {
                        No = c.No,
                        Name = c.Name,
                    }).ToList();
                    _dbContext.Database.ExecuteSqlRaw($"TRUNCATE TABLE COA_Master");
                    foreach (var coa in coaResponse)
                    {
                        CoaMaster master = new CoaMaster();
                        master.CoaNo = coa.No;
                        master.CoaName = coa.Name;
                        _dbContext.Add(master);
                        _dbContext.SaveChanges();
                    }
                    var apiResponse = new
                    {
                        data = coaResponse,
                        status = true,
                        message = $"{COAMaster.Count} records found.",
                    };
                    return Ok(apiResponse);
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        message = "No Coa list found",
                        data = COAMaster
                    };
                    return Ok(response);
                }

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { status = false, message = "An error occurred while fetching the detail." });
            }
        }

        [HttpGet("GetGSTGroup")]
        public async Task<IActionResult> GetGSTGroup()
        {
            try
            {
                List<GSTGroup> GSTGroup = await GetList<GSTGroup>("GSTGroup");
                if (GSTGroup.Count > 0)
                {
                    var gstResponse = GSTGroup.Select(c => new GSTGroupInfo
                    {
                        Code = c.Code,
                    }).ToList();
                    _dbContext.Database.ExecuteSqlRaw($"TRUNCATE TABLE GSTGroup");
                    foreach (var coa in gstResponse)
                    {
                        Gstgroup master = new Gstgroup();
                        master.GroupCode = coa.Code;
                        _dbContext.Add(master);
                        _dbContext.SaveChanges();
                    }
                    var apiResponse = new
                    {
                        data = gstResponse,
                        status = true,
                        message = $"{GSTGroup.Count} records found.",
                    };
                    return Ok(apiResponse);

                }
                else
                {
                    var response = new
                    {
                        status = true,
                        message = "No GSTGroup list found",
                        data = GSTGroup
                    };
                    return Ok(response);
                }

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { status = false, message = "An error occurred while fetching the detail." });
            }
        }

        [HttpGet("GetHSNSAC")]
        public async Task<IActionResult> GetHSNSAC()
        {
            try
            {
                List<HSNSACMaster> HSNSACMaster = await GetList<HSNSACMaster>("HSNSAC");
                if (HSNSACMaster.Count > 0)
                {
                    var hsnResponse = HSNSACMaster.Select(c => new HSNSACMasterInfo
                    {
                        Code = c.Code,
                        GST_Group_Code = c.GST_Group_Code
                    }).ToList();
                    _dbContext.Database.ExecuteSqlRaw($"TRUNCATE TABLE HSNSAC");
                    foreach (var hsn in hsnResponse)
                    {
                        Hsnsac master = new Hsnsac();
                        master.HsnCode = hsn.Code;
                        master.HsnGroup = hsn.GST_Group_Code;
                        _dbContext.Add(master);
                        _dbContext.SaveChanges();
                    }
                    var apiResponse = new
                    {
                        data = hsnResponse,
                        status = true,
                        message = $"{HSNSACMaster.Count} records found.",
                    };
                    return Ok(apiResponse);
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        message = "No HSNSAC list found",
                        data = HSNSACMaster
                    };
                    return Ok(response);
                }

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { status = false, message = "An error occurred while fetching the detail." });
            }
        }

        [HttpGet("GetLocation")]
        public async Task<IActionResult> GetLocation()
        {
            try
            {
                List<GetLocation> GetLocation = await GetList<GetLocation>("Location");
                if (GetLocation.Count > 0)
                {
                    var loactionResponse = GetLocation.Select(c => new LoactionInfo
                    {
                        code = c.code,
                        name = c.name,
                    }).ToList();
                    _dbContext.Database.ExecuteSqlRaw($"TRUNCATE TABLE Location");
                    foreach (var coa in loactionResponse)
                    {
                        Location master = new Location();
                        master.LocationCode = coa.code;
                        master.LocationName = coa.name;
                        _dbContext.Add(master);
                        _dbContext.SaveChanges();
                    }
                    var apiResponse = new
                    {
                        data = loactionResponse,
                        status = true,
                        message = $"{GetLocation.Count} records found.",
                    };
                    return Ok(apiResponse);

                }
                else
                {
                    var response = new
                    {
                        status = true,
                        message = "No Location list found",
                        data = GetLocation
                    };
                    return Ok(response);
                }

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { status = false, message = "An error occurred while fetching the detail." });
            }
        }

        [HttpGet("GetProject")]
        public async Task<IActionResult> GetProject()
        {
            try
            {
                List<ProjectModel> cityList = await GetList<ProjectModel>("Project");
                _dbContext.Database.ExecuteSqlRaw($"TRUNCATE TABLE FICCI_ERP_PROJECT_DETAILS");
                foreach (var k in cityList)
                {
                    FicciErpProjectDetail proj = new FicciErpProjectDetail();
                    proj.ProjectCode = k.Code;
                    proj.ProjectName = k.Name;
                    proj.ProjectDepartmentCode = k.DepartmentCode;
                    proj.ProjectDepartmentName = k.DepartmentName;
                    proj.ProjectDivisionName = k.DivisionName;
                    proj.ProjectDivisionCode = k.DivisionCode;
                    proj.TlApprover = k.TLApprover;
                    proj.ChApprover = k.CHApprover;
                    proj.FinanceApprover = k.FinanceApprover;
                    proj.SupportApprover = k.SupportApprover;
                    proj.DimensionCode = k.Dimension_Code;
                    _dbContext.Add(proj);
                    _dbContext.SaveChanges();
                }
                var apiResponse = new
                {
                    data = cityList,
                    status = true,
                    message = $"{cityList.Count} records found.",
                };
                return Ok(apiResponse);

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { status = false, message = "An error occurred while fetching the detail." });
            }
        }

        [HttpGet("GetCustomerInformation")]
        public async Task<IActionResult> GetCustomerInformation(string Name, string GSTNo)
        {
            try
            {
                List<CustomerModel> CustomerListInfo = await GetList<CustomerModel>("Customer");

                CustomerListInfo = CustomerListInfo.Where(x => x.Name.Contains(Name) || x.GSTRegistrationNo.Contains(GSTNo)).ToList();

                var apiResponse = new
                {
                    data = CustomerListInfo,
                    status = true,
                    message = $"{CustomerListInfo.Count} records found.",
                };
                return Ok(apiResponse);


                //_dbContext.Database.ExecuteSqlRaw($"TRUNCATE TABLE ERPCustomer");
                //foreach (var k in CustomerList)
                //{
                //    Erpcustomer cust = new Erpcustomer();
                //    cust.CustNo = k.No;
                //    cust.CustName = k.Name;
                //    cust.CustName2 = k.Name2;
                //    cust.CustAddress = k.Address;
                //    cust.CustAddress2 = k.Address2;
                //    cust.City = k.City;
                //    cust.StateCode = k.State_Code;
                //    cust.PinCode = k.PinCode;
                //    cust.CountryRegionCode = k.Country_Region_Code;
                //    cust.GstregistrationNo = k.GSTRegistrationNo;
                //    cust.GstcustomerType = k.GSTCustomerType;
                //    cust.Contact = k.Contact;
                //    cust.PanNo = k.PAN_No;
                //    cust.Email = k.EMail;
                //    cust.PrimaryContactNo = k.PrimaryContactNo;
                //    _dbContext.Add(cust);
                //    _dbContext.SaveChanges();
                // }


            }
            catch (Exception ex)
            {
                return StatusCode(500, new { status = false, message = "An error occurred while fetching the detail." });
            }
        }



        [HttpGet("GetInvoiceSummary")]
        public async Task<ApiResponseModel> GetInvoiceSummary()
        {
            try
            {
                string result = await GetSecurityToken();
                var httpClient = new HttpClient();

                httpClient.DefaultRequestHeaders.Add("prefer", "odata.maxpagesize=10");
                httpClient.DefaultRequestHeaders.Add("Authorization", result);

                string URL = _navURL + "/FICCI_CRM/ODataV4/Company('FICCI')/GetTaxInvoiceSummary";

                var response = await httpClient.GetAsync(URL);

                response.EnsureSuccessStatusCode();

                var responseContent = response.Content.ReadAsStringAsync().Result;

                List<TAX_INVOICE_HEADER_RESPONSE> list = JsonConvert.DeserializeObject<ODataResponse<TAX_INVOICE_HEADER_RESPONSE>>(responseContent).Value.ToList();


                if (list != null)
                {
                    _responseModel.Data = list;
                    _responseModel.Status = true;
                    _responseModel.Message = "list fetched";
                }
                else
                {
                    _responseModel.Data = null;
                    _responseModel.Status = false;
                    _responseModel.Message = "error";
                }
            }
            catch (Exception ex)
            {
                _responseModel.Data = ex;
                _responseModel.Status = false;
                _responseModel.Message = ex.Message;
            }
            return _responseModel;


        }



        [HttpGet("GetTaxInvoiceInformation")]
        public async Task<ApiResponseModel> GetTaxInvoiceInformation()
        {
            try
            {

                string result = await GetSecurityToken();
                var httpClient = new HttpClient();

                httpClient.DefaultRequestHeaders.Add("prefer", "odata.maxpagesize=5");
                httpClient.DefaultRequestHeaders.Add("Authorization", result);

                var response = await httpClient.GetAsync("https://api.businesscentral.dynamics.com/v2.0/d3a55687-ec5c-433b-9eaa-9d952c913e94/FICCI_CRM/api/FICCI/FICCI/v1.0/companies(2d9345bb-769a-ed11-bff5-000d3af29678)/GetTaxInvoiceInfos?$expand=GetTaxInvoiceInfoLines");

                response.EnsureSuccessStatusCode();

                var responseContent = response.Content.ReadAsStringAsync().Result;

                List<TAX_INVOICE_HEADER_RESPONSE> FinalResult = JsonConvert.DeserializeObject<ODataResponse<TAX_INVOICE_HEADER_RESPONSE>>(responseContent).Value.ToList();


                //   PURCHASE_INVOICE_HEADER_RESPONSE odatavalue = JsonConvert.DeserializeObject<PURCHASE_INVOICE_HEADER_RESPONSE>(responseContent);

                if (FinalResult != null)
                {
                    _responseModel.Data = FinalResult;
                    _responseModel.Status = true;
                    _responseModel.Message = "list fetched";
                }
                else
                {
                    _responseModel.Data = null;
                    _responseModel.Status = false;
                    _responseModel.Message = "error";
                }
            }
            catch (Exception ex)
            {
                _responseModel.Data = ex;
                _responseModel.Status = false;
                _responseModel.Message = ex.Message;
            }
            return _responseModel;


        }

        [HttpPost("PostCustomer")]

        //Submit customer not drafted
        public async Task<ApiResponseModel> PostCustomer(CustomerPost data)
        {
            try
            {

                string result = await GetSecurityToken();
                var httpClient = new HttpClient();

                httpClient.DefaultRequestHeaders.Add("Authorization", result);

                var insertCustomerDetails = CreateCustomerJson(data);

                HttpContent content = new StringContent(insertCustomerDetails, null, "application/json");

                var response = await httpClient.PostAsync("https://api.businesscentral.dynamics.com/v2.0/d3a55687-ec5c-433b-9eaa-9d952c913e94/FICCI_CRM/ODataV4/Company('FICCI')/CustomerAPI", content);

                response.EnsureSuccessStatusCode();

                var responseContent = response.Content.ReadAsStringAsync().Result;

                CustomerResponse odatavalue = JsonConvert.DeserializeObject<CustomerResponse>(responseContent);

                if (odatavalue != null)
                {
                    _responseModel.Data = odatavalue;
                    _responseModel.Status = true;
                    _responseModel.Message = "Customer Created";
                }
                else
                {
                    _responseModel.Data = null;
                    _responseModel.Status = false;
                    _responseModel.Message = "error";
                }
            }
            catch (Exception ex)
            {
                _responseModel.Data = ex;
                _responseModel.Status = false;
                _responseModel.Message = ex.Message;
            }
            return _responseModel;
        }

        private dynamic CreateCustomerJson(CustomerPost objCustomerModel)
        {

            string CustomerName = objCustomerModel.Name;
            string CustomerLastName = objCustomerModel.Name2;

            string Address = objCustomerModel.Address;
            string Address2 = objCustomerModel.Address2;

            string Contact = objCustomerModel.Contact;
            string Phone = objCustomerModel.PrimaryContactNo;


            string PinCode = objCustomerModel.PostCode;
            string Email = objCustomerModel.EMail;


            string CityCode = objCustomerModel.City;

            string StateCode = objCustomerModel.State_Code;


            string CountryCode = objCustomerModel.Country_Region_Code;

            string GSTNumber = objCustomerModel.GSTRegistrationNo;

            string GSTCustomerType = objCustomerModel.GSTCustomerType;


            string PAN = objCustomerModel.PAN_No;

            string InsertCustomerDetailsInNAV = CustomerName + "," + CustomerLastName + "," + Address + "," + Address2 + "," + Contact + "," + Phone + "," + PinCode + "," + Email + "," + CityCode + "," + StateCode + "," + CountryCode + "," + GSTNumber + "," + GSTCustomerType + "," + PAN + ";";


            string json = JsonConvert.SerializeObject(objCustomerModel);
            return json;


        }


        [HttpGet("GetTaxInvoiceAttachment")]
        public async Task<ApiResponseModel> GetTaxInvoiceAttachment()
        {
            try
            {


                string result = await GetSecurityToken();
                var httpClient = new HttpClient();

                httpClient.DefaultRequestHeaders.Add("prefer", "odata.maxpagesize=5");
                httpClient.DefaultRequestHeaders.Add("Authorization", result);

                var response = await httpClient.GetAsync("https://api.businesscentral.dynamics.com/v2.0/d3a55687-ec5c-433b-9eaa-9d952c913e94/FICCI_CRM/ODataV4/Company('FICCI')/GetTaxInvoiceAttachment");

                response.EnsureSuccessStatusCode();

                var responseContent = response.Content.ReadAsStringAsync().Result;


                List<TAX_INVOICE_ATTACHMENT> odatavalue = JsonConvert.DeserializeObject<ODataResponse<TAX_INVOICE_ATTACHMENT>>(responseContent).Value.ToList();


                if (odatavalue != null)
                {
                    _responseModel.Data = odatavalue;
                    _responseModel.Status = true;
                    _responseModel.Message = "list fetched";
                }
                else
                {
                    _responseModel.Data = null;
                    _responseModel.Status = false;
                    _responseModel.Message = "error";
                }
            }
            catch (Exception ex)
            {
                _responseModel.Data = ex;
                _responseModel.Status = false;
                _responseModel.Message = ex.Message;
            }
            return _responseModel;


        }



        [HttpPost("PostUpdateCustomer")]
        public async Task<ApiResponseModel> PostUpdateCustomer(CustomerPostUpdate data)
        {
            try
            {

                string result = await GetSecurityToken();
                var httpClient = new HttpClient();

                httpClient.DefaultRequestHeaders.Add("Authorization", result);

                var insertCustomerDetails = CreateCustomerUpdateJson(data);

                HttpContent content = new StringContent(insertCustomerDetails, null, "application/json");

                string URL = _navURL + "/FICCI_CRM/ODataV4/Company('FICCI')/CustomerAPI";

                URL += "((No='"+data.No+ "')";

                var response = await httpClient.PatchAsync(URL, content);

             //   var response = await httpClient.PatchAsync("https://api.businesscentral.dynamics.com/v2.0/d3a55687-ec5c-433b-9eaa-9d952c913e94/FICCI_CRM/ODataV4/Company('FICCI')/CustomerAPI", content);

                response.EnsureSuccessStatusCode();

                var responseContent = response.Content.ReadAsStringAsync().Result;

                CustomerResponse odatavalue = JsonConvert.DeserializeObject<CustomerResponse>(responseContent);

                if (odatavalue != null)
                {
                    _responseModel.Data = odatavalue;
                    _responseModel.Status = true;
                    _responseModel.Message = "Customer updated";
                }
                else
                {
                    _responseModel.Data = null;
                    _responseModel.Status = false;
                    _responseModel.Message = "error";
                }
            }
            catch (Exception ex)
            {
                _responseModel.Data = ex;
                _responseModel.Status = false;
                _responseModel.Message = ex.Message;
            }
            return _responseModel;
        }

        private dynamic CreateCustomerUpdateJson(CustomerPostUpdate objCustomerModel)
        {

            string No = objCustomerModel.No;

            string CustomerName = objCustomerModel.Name;
            string CustomerLastName = objCustomerModel.Name2;

            string Address = objCustomerModel.Address;
            string Address2 = objCustomerModel.Address2;

            string Contact = objCustomerModel.Contact;
            string Phone = objCustomerModel.PrimaryContactNo;


            string PinCode = objCustomerModel.PostCode;
            string Email = objCustomerModel.EMail;


            string CityCode = objCustomerModel.City;

            string StateCode = objCustomerModel.State_Code;


            string CountryCode = objCustomerModel.Country_Region_Code;

            string GSTNumber = objCustomerModel.GSTRegistrationNo;

            string GSTCustomerType = objCustomerModel.GSTCustomerType;


            string PAN = objCustomerModel.PAN_No;

            string InsertCustomerDetailsInNAV = CustomerName + "," + CustomerLastName + "," + Address + "," + Address2 + "," + Contact + "," + Phone + "," + PinCode + "," + Email + "," + CityCode + "," + StateCode + "," + CountryCode + "," + GSTNumber + "," + GSTCustomerType + "," + PAN + ";";


            string json = JsonConvert.SerializeObject(objCustomerModel);
            return json;


        }

        [HttpPost("PostPIData")]
        //Submit customer not drafted
        public async Task<IActionResult> PostPIData(PURCHASE_INVOICE_HEADER purchase_header)
        {
            try
            {
                string result = await GetSecurityToken();
                var httpClient = new HttpClient();

                httpClient.DefaultRequestHeaders.Add("Authorization", result);

                var insertPIDetails = CreatePostPIHeaderJson(purchase_header);

                HttpContent content = new StringContent(insertPIDetails, null, "application/json");

                var response = await httpClient.PostAsync(_navURL + "/FICCI_CRM/api/FICCI/FICCI/v1.0/companies(2d9345bb-769a-ed11-bff5-000d3af29678)/getPerformaInvoiceInfos", content);

                response.EnsureSuccessStatusCode();

                var responseContent = response.Content.ReadAsStringAsync().Result;

                PURCHASE_INVOICE_HEADER_RESPONSE odatavalue = JsonConvert.DeserializeObject<PURCHASE_INVOICE_HEADER_RESPONSE>(responseContent);


                if (odatavalue != null)
                {
                    _responseModel.Data = odatavalue;
                    _responseModel.Status = true;
                    _responseModel.Message = "PI Created";
                }
                else
                {
                    _responseModel.Data = null;
                    _responseModel.Status = false;
                    _responseModel.Message = "error";
                }
            }
            catch (Exception ex)
            {
                _responseModel.Data = ex;
                _responseModel.Status = false;
                _responseModel.Message = ex.Message;
            }
            return Ok(_responseModel);
        }

        [HttpPost("UpdatePIData")]
        //Submit customer not drafted
        public async Task<IActionResult> UpdatePIData(PURCHASE_INVOICE_HEADER_UPDATE purchase_headerupdate)
        {
            try
            {
                string result = await GetSecurityToken();
                var httpClient = new HttpClient();

                httpClient.DefaultRequestHeaders.Add("If-Match", "*");
                httpClient.DefaultRequestHeaders.Add("Authorization", result);

                var insertPIDetails = CreateUpdatePIHeaderJson(purchase_headerupdate);

                HttpContent content = new StringContent(insertPIDetails, null, "application/json");

                var response = await httpClient.PatchAsync(_navURL + "/FICCI_CRM/api/FICCI/FICCI/v1.0/companies(2d9345bb-769a-ed11-bff5-000d3af29678)/getPerformaInvoiceInfos(DocumentType='Invoice',no='" + purchase_headerupdate.no + "')", content);

                response.EnsureSuccessStatusCode();

                var responseContent = response.Content.ReadAsStringAsync().Result;

                PURCHASE_INVOICE_HEADER_RESPONSE odatavalue = JsonConvert.DeserializeObject<PURCHASE_INVOICE_HEADER_RESPONSE>(responseContent);


                if (odatavalue != null)
                {
                    _responseModel.Data = odatavalue;
                    _responseModel.Status = true;
                    _responseModel.Message = "PI Updated";
                }
                else
                {
                    _responseModel.Data = null;
                    _responseModel.Status = false;
                    _responseModel.Message = "error";
                }
            }
            catch (Exception ex)
            {
                _responseModel.Data = ex;
                _responseModel.Status = false;
                _responseModel.Message = ex.Message;
            }
            return Ok(_responseModel);
        }
        [HttpPost("PostPILineData")]
        public async Task<IActionResult> PostPILineData(PURCHASE_INVOICE_LINE purchase_line)
        {
            try
            {
                string result = await GetSecurityToken();
                var httpClient = new HttpClient();

                
                httpClient.DefaultRequestHeaders.Add("Authorization", result);

                var insertPIDetails = CreatePostPILineJson(purchase_line);

                HttpContent content = new StringContent(insertPIDetails, null, "application/json");

                var response = await httpClient.PostAsync(_navURL + "/FICCI_CRM/ODataV4/Company('FICCI')/GetPerformaInvoiceInfoLine", content);

                response.EnsureSuccessStatusCode();

                var responseContent = response.Content.ReadAsStringAsync().Result;

                VMS_PURCHASE_LINE_RESPONSE odatavalue = JsonConvert.DeserializeObject<VMS_PURCHASE_LINE_RESPONSE>(responseContent);



                if (odatavalue != null)
                {
                    _responseModel.Data = odatavalue;
                    _responseModel.Status = true;
                    _responseModel.Message = "PI Line Updated";
                }
                else
                {
                    _responseModel.Data = null;
                    _responseModel.Status = false;
                    _responseModel.Message = "error";
                }
            }
            catch (Exception ex)
            {
                _responseModel.Data = ex;
                _responseModel.Status = false;
                _responseModel.Message = ex.Message;
            }
            return Ok();
        }


        [HttpPost("UpdatePILineData")]
        public async Task<IActionResult> UpdatePILineData(PURCHASE_INVOICE_LINE purchase_line)
        {
            try
            {
                string result = await GetSecurityToken();
                var httpClient = new HttpClient();

                httpClient.DefaultRequestHeaders.Add("If-Match", "*");
                httpClient.DefaultRequestHeaders.Add("Authorization", result);

                var insertPIDetails = CreatePostPILineJson(purchase_line);

                HttpContent content = new StringContent(insertPIDetails, null, "application/json");

                string URL = _navURL + "/FICCI_CRM/api/FICCI/FICCI/v1.0/companies(2d9345bb-769a-ed11-bff5-000d3af29678)/GetPerformaInvoiceInfoLine";
                URL += "(documentType = 'Invoice',documentNo = '" + purchase_line.documentNo + "',lineNo = " + purchase_line.lineNo + ")";

                var response = await httpClient.PatchAsync(URL,content);
                         //  var response = await httpClient.PatchAsync("https://api.businesscentral.dynamics.com/v2.0/d3a55687-ec5c-433b-9eaa-9d952c913e94/FICCI_CRM/api/FICCI/FICCI/v1.0/companies(2d9345bb-769a-ed11-bff5-000d3af29678)/GetPerformaInvoiceInfoLine(documentType='''Invoice''',documentNo='" + purchase_line.no_ + "',lineNo=" + purchase_line.lineNo + "", content);

                //  var response = await httpClient.PatchAsync(_navURL + "/FICCI_CRM/ODataV4/Company('FICCI')/GetPerformaInvoiceInfoLine", content);

                response.EnsureSuccessStatusCode();

                var responseContent = response.Content.ReadAsStringAsync().Result;

                VMS_PURCHASE_LINE_RESPONSE odatavalue = JsonConvert.DeserializeObject<VMS_PURCHASE_LINE_RESPONSE>(responseContent);



                if (odatavalue != null)
                {
                    _responseModel.Data = odatavalue;
                    _responseModel.Status = true;
                    _responseModel.Message = "PI Line Created";
                }
                else
                {
                    _responseModel.Data = null;
                    _responseModel.Status = false;
                    _responseModel.Message = "error";
                }
            }
            catch (Exception ex)
            {
                _responseModel.Data = ex;
                _responseModel.Status = false;
                _responseModel.Message = ex.Message;
            }
            return Ok();
        }

        [HttpPost("DeletePILineData")]
        public async Task<IActionResult> DeletePILineData(PURCHASE_INVOICE_LINE purchase_line)
        {
            try
            {
                string result = await GetSecurityToken();
                var httpClient = new HttpClient();

                httpClient.DefaultRequestHeaders.Add("If-Match", "*");
                httpClient.DefaultRequestHeaders.Add("Authorization", result);

                var insertPIDetails = CreatePostPILineJson(purchase_line);

                //var content = new StringContent("", null, "text/plain");

               HttpContent content = new StringContent(insertPIDetails, null, "application/json");

                string URL = _navURL + "/FICCI_CRM/api/FICCI/FICCI/v1.0/companies(2d9345bb-769a-ed11-bff5-000d3af29678)/GetPerformaInvoiceInfoLine";
                URL += "(documentType = 'Invoice',documentNo = '" + purchase_line.documentNo + "',lineNo = " + purchase_line.lineNo + ")";

                var response = await httpClient.DeleteAsync(URL);
             
                response.EnsureSuccessStatusCode();

                var responseContent = response.Content.ReadAsStringAsync().Result;

                VMS_PURCHASE_LINE_RESPONSE odatavalue = JsonConvert.DeserializeObject<VMS_PURCHASE_LINE_RESPONSE>(responseContent);



                if (odatavalue != null)
                {
                    _responseModel.Data = odatavalue;
                    _responseModel.Status = true;
                    _responseModel.Message = "PI Line Created";
                }
                else
                {
                    _responseModel.Data = null;
                    _responseModel.Status = false;
                    _responseModel.Message = "error";
                }
            }
            catch (Exception ex)
            {
                _responseModel.Data = ex;
                _responseModel.Status = false;
                _responseModel.Message = ex.Message;
            }
            return Ok();
        }

        [HttpPost("PostAttachmentData")]
        public async Task<bool> UploadFileInERP(string PINo, string PIHederNo)
        {
            bool status = false;

            string folderpath = Path.Combine("wwwroot", "PurchaseInvoice");
            folderpath = folderpath + "/" + PIHederNo.Trim() + "/";

            try
            {

                if (Directory.Exists(folderpath))
                {

                    string result = await GetSecurityToken();
                    var httpClient = new HttpClient();
                    httpClient.DefaultRequestHeaders.Add("Authorization", result);

                    DirectoryInfo di = new DirectoryInfo(folderpath);
                    FileInfo[] files = di.GetFiles("*");
                    for (int i = 0; i < files.Length; i++)
                    {
                        byte[] bytes = System.IO.File.ReadAllBytes(files[i].FullName);

                        string base64String = Convert.ToBase64String(bytes);

                        HttpContent contentAttachment = new StringContent("{'Number':1,'Attachment': '" + base64String + "','FileName': '" + Path.GetFileName(files[i].FullName) + "','InvoiceNo': '" + PINo + "'}", System.Text.Encoding.UTF8, "application/json");
                        var responseAttachment = await httpClient.PostAsync(_navURL + "/FICCI_CRM/ODataV4/Company('FICCI')/PerformaInvoiceAttachment", contentAttachment);
                        var finalcheck = responseAttachment.EnsureSuccessStatusCode();

                    }
                    return status = true;
                }
                else
                {
                    return status = false;
                }
            }
            catch (Exception ex)
            {
                //  log.Error("Error in UploadFileInERP function ", ex);
                return status;
            }
        }
        private dynamic CreatePostPIHeaderJson(PURCHASE_INVOICE_HEADER objPurchaseHeaderModel)
        {

            //string DocumentType = "Invoice";
           // string No = objPurchaseHeaderModel.no;

            string CustomerNo = objPurchaseHeaderModel.sellToCustomerNo;
            string CustomerName = objPurchaseHeaderModel.sellToCustomerName;
            string CustomerName2 = objPurchaseHeaderModel.sellToCustomerName2;
            //string Address = objPurchaseHeaderModel.sellToAddress;
            //string Address2 = objPurchaseHeaderModel.sellToAddress2;
            //string City = objPurchaseHeaderModel.sellToCity;
            //string PostCode = objPurchaseHeaderModel.sellToPostCode;
            //string CountryRegionCode = objPurchaseHeaderModel.sellToCountryRegionCode;
            //string GSTNo = objPurchaseHeaderModel.GSTNo;
            //string PANNo = objPurchaseHeaderModel.PANNo;
            string ProjectCode = objPurchaseHeaderModel.ProjectCode;
            string DepartmentName = objPurchaseHeaderModel.DepartmentName;
            string DepartmentCode = objPurchaseHeaderModel.DepartmentCode;
            string DivisionCode = objPurchaseHeaderModel.DivisionCode;
            string DivisionName = objPurchaseHeaderModel.DivisionName;
            string ApproverTL = objPurchaseHeaderModel.ApproverTL;
            string ApproverCH = objPurchaseHeaderModel.ApproverCH;
            string FinanceApprover = objPurchaseHeaderModel.FinanceApprover;
            string ApproverSupport = objPurchaseHeaderModel.ApproverSupport;
            bool InvoicePortalOrder = objPurchaseHeaderModel.InvoicePortalOrder;
            bool InvoicePortalSubmitted = objPurchaseHeaderModel.InvoicePortalSubmitted;

            //  string InsertPIDetailsInNAV = DocumentType + "," + CustomerNo + "," + CustomerName + "," + Address + "," + Address2 + "," + City + "," + PostCode + "," + CountryRegionCode + "," + GSTNo + "," + PANNo + "," + ProjectCode + "," + DepartmentName + "," + DepartmentCode + "," + DivisionCode + "," + DivisionName + "," + ApproverTL + ","+ ApproverCH +","+ FinanceApprover+","+ApproverSupport+","+InvoicePortalOrder+","+InvoicePortalSubmitted+ ";";

            string InsertPIDetailsInNAV = CustomerName2 + "," + CustomerNo + "," + CustomerName + "," + ProjectCode + "," + DepartmentName + "," + DepartmentCode + "," + DivisionCode + "," + DivisionName + "," + ApproverTL + "," + ApproverCH + "," + FinanceApprover + "," + ApproverSupport + "," + InvoicePortalOrder + "," + InvoicePortalSubmitted + ";";


            string json = JsonConvert.SerializeObject(objPurchaseHeaderModel);
            return json;


        }

        private dynamic CreateUpdatePIHeaderJson(PURCHASE_INVOICE_HEADER_UPDATE objPurchaseHeaderModel)
        {

            //string DocumentType = "Invoice";
            // string No = objPurchaseHeaderModel.no;

            if (objPurchaseHeaderModel.no != null)
            {
                string No = objPurchaseHeaderModel.no;
            }
            string CustomerNo = objPurchaseHeaderModel.sellToCustomerNo;
            string CustomerName = objPurchaseHeaderModel.sellToCustomerName;
            string CustomerName2 = objPurchaseHeaderModel.sellToCustomerName2;
            //string Address = objPurchaseHeaderModel.sellToAddress;
            //string Address2 = objPurchaseHeaderModel.sellToAddress2;
            //string City = objPurchaseHeaderModel.sellToCity;
            //string PostCode = objPurchaseHeaderModel.sellToPostCode;
            //string CountryRegionCode = objPurchaseHeaderModel.sellToCountryRegionCode;
            //string GSTNo = objPurchaseHeaderModel.GSTNo;
            //string PANNo = objPurchaseHeaderModel.PANNo;
            string ProjectCode = objPurchaseHeaderModel.ProjectCode;
            string DepartmentName = objPurchaseHeaderModel.DepartmentName;
            string DepartmentCode = objPurchaseHeaderModel.DepartmentCode;
            string DivisionCode = objPurchaseHeaderModel.DivisionCode;
            string DivisionName = objPurchaseHeaderModel.DivisionName;
            string ApproverTL = objPurchaseHeaderModel.ApproverTL;
            string ApproverCH = objPurchaseHeaderModel.ApproverCH;
            string FinanceApprover = objPurchaseHeaderModel.FinanceApprover;
            string ApproverSupport = objPurchaseHeaderModel.ApproverSupport;
            bool InvoicePortalOrder = objPurchaseHeaderModel.InvoicePortalOrder;
            bool InvoicePortalSubmitted = objPurchaseHeaderModel.InvoicePortalSubmitted;

            //  string InsertPIDetailsInNAV = DocumentType + "," + CustomerNo + "," + CustomerName + "," + Address + "," + Address2 + "," + City + "," + PostCode + "," + CountryRegionCode + "," + GSTNo + "," + PANNo + "," + ProjectCode + "," + DepartmentName + "," + DepartmentCode + "," + DivisionCode + "," + DivisionName + "," + ApproverTL + ","+ ApproverCH +","+ FinanceApprover+","+ApproverSupport+","+InvoicePortalOrder+","+InvoicePortalSubmitted+ ";";

            string InsertPIDetailsInNAV = CustomerName2 + "," + CustomerNo + "," + CustomerName + "," + ProjectCode + "," + DepartmentName + "," + DepartmentCode + "," + DivisionCode + "," + DivisionName + "," + ApproverTL + "," + ApproverCH + "," + FinanceApprover + "," + ApproverSupport + "," + InvoicePortalOrder + "," + InvoicePortalSubmitted + ";";


            string json = JsonConvert.SerializeObject(objPurchaseHeaderModel);
            return json;


        }

        private string CreatePostPILineJson(PURCHASE_INVOICE_LINE objPurchaseLineModel)
        {

            //long LINE_NO = 0;



            // long LineNo = item.lineNo;
            string DocumentNo = objPurchaseLineModel.documentNo;
            string DocumentType = "Invoice";
            string Type = "G/L Account";
            string No_ = objPurchaseLineModel.no_;
            string LocationCode = "FICCI-DL"; //item.LocationCode;
            int Quantity = objPurchaseLineModel.quantity;
            Nullable<decimal> UnitPrice = objPurchaseLineModel.unitPrice;
            Nullable<decimal> LineAmount = objPurchaseLineModel.lineAmount;
            string GSTGroupCode = objPurchaseLineModel.gSTGroupCode;
           // string GST_Group_Type = objPurchaseLineModel.gST_Group_Type;
            string HSN_SAC_Code = objPurchaseLineModel.hSN_SAC_Code;
            //  Nullable<decimal> GSTCredit = item.gSTCredit;

            //foreach (var item in objPurchaseLineModel)
            //{
            //    if (LINE_NO <= 0)
            //    {
            //        LINE_NO = 10000;
            //    }
            //    else
            //    {
            //        LINE_NO = Convert.ToInt32(LINE_NO) + 10000;
            //    }

            //   // long LineNo = item.lineNo;
            //    string DocumentNo = item.documentNo;
            //    string DocumentType = "Invoice";
            //    string Type = "G/L Account";
            //    string No_ = item.no_;
            //    string LocationCode = "FICCI-DL"; //item.LocationCode;
            //    int Quantity =item.quantity;
            //    Nullable<decimal> UnitPrice = item.unitPrice;
            //    Nullable<decimal> LineAmount = item.lineAmount;
            //    string GSTGroupCode = item.gSTGroupCode;
            //    string GST_Group_Type = item.gST_Group_Type;
            //    string HSN_SAC_Code = item.hSN_SAC_Code;
            //  //  Nullable<decimal> GSTCredit = item.gSTCredit;

            //}

            string json = JsonConvert.SerializeObject(objPurchaseLineModel);
            return json;


        }
             
        //[HttpPost]
        //[ActionName("UpdatePIHeaderInfo")]
        //public async Task<IActionResult> UpdatePIHeaderInfo(PURCHASE_INVOICE_HEADER_RESPONSE piHeaderERPModel)
        //{
        //    return Ok(UpdatePIHeader(piHeaderERPModel));
        //}

        //[HttpPost]
        //[ActionName("UpdatePILineInfo")]
        //public async Task<IActionResult> UpdatePILineInfo(VMS_PURCHASE_LINE_RESPONSE piLineERPModel)
        //{
        //    return Ok(UpdatePILine(piLineERPModel));
        //}


        //[HttpPost]
        //[ActionName("UpdateHSNSACDetailsInfo")]
        //public async Task<IActionResult> UpdateHSNSACDetailsInfo(HSNSACMaster hSNErpModel)
        //{
        //    return Ok(UpdateHSNMastersDetails(hSNErpModel));
        //}



        //private ApiResponseModel UpdatePIHeader(PURCHASE_INVOICE_HEADER_RESPONSE piHeaderERPModel)
        //{
        //    try
        //    {             
        //        var _purchaseHeaderModel = _dbContext.FicciImpiHeaders.Where(x => x.ImpiHeaderPiNo == piHeaderERPModel.no).FirstOrDefault();

        //        if (_purchaseHeaderModel != null)
        //        {

        //            _purchaseHeaderModel.ImpiHeaderCustomerCode = piHeaderERPModel.sellToCustomerNo;
        //            _purchaseHeaderModel.ImpiHeaderCustomerName = piHeaderERPModel.sellToCustomerName;

        //          //  _purchaseHeaderModel.ImpiHeaderCustomerName = piHeaderERPModel.sellToCustomerName2;
        //            _purchaseHeaderModel.ImpiHeaderCustomerAddress = piHeaderERPModel.sellToAddress;

        //          // _purchaseHeaderModel.ImpiHeaderCustomerAddress = piHeaderERPModel.sellToAddress2;

        //            _purchaseHeaderModel.ImpiHeaderCustomerCity = piHeaderERPModel.sellToCity;
        //            _purchaseHeaderModel.ImpiHeaderCustomerPinCode = piHeaderERPModel.sellToPostCode;


        //            _purchaseHeaderModel.ImpiHeaderGstNo = piHeaderERPModel.GST_No;
        //            _purchaseHeaderModel.ImpiHeaderPanNo = piHeaderERPModel.PAN_No;

        //            _purchaseHeaderModel.ImpiHeaderProjectCode = piHeaderERPModel.ProjectCode;

        //            _purchaseHeaderModel.ImpiHeaderProjectDepartmentCode = piHeaderERPModel.DepartmentCode;
        //            _purchaseHeaderModel.ImpiHeaderProjectDepartmentName = piHeaderERPModel.DepartmentName;


        //            _purchaseHeaderModel.ImpiHeaderProjectDivisionCode = piHeaderERPModel.DivisionCode;
        //            _purchaseHeaderModel.ImpiHeaderProjectDivisionName = piHeaderERPModel.DivisionName;


        //            _purchaseHeaderModel.ImpiHeaderTlApprover = piHeaderERPModel.ApproverTL;
        //            _purchaseHeaderModel.ImpiHeaderClusterApprover = piHeaderERPModel.ApproverCH;

        //            _purchaseHeaderModel.ImpiHeaderFinanceApprover = piHeaderERPModel.FinanceApprover;

        //            //_purchaseHeaderModel. = piHeaderERPModel.FinanceApprover;
        //            //_purchaseHeaderModel.ImpiHeaderFinanceApprover = piHeaderERPModel.FinanceApprover;


        //            _dbContext.Update(_purchaseHeaderModel);
        //            _dbContext.SaveChanges();


        //            int count = _dbContext.SaveChanges();

        //            if (count > 0)
        //            {                    
        //                _responseModel.Status = true;
        //                _responseModel.Message = "Success";
        //                _responseModel.StatusCode = HttpStatusCode.OK;
        //                _responseModel.Data = _dbContext.FicciImpiHeaders.Where(x => x.ImpiHeaderPiNo.Equals(piHeaderERPModel.no)).SingleOrDefault(); ;

        //            }
        //            else
        //            {
        //                _responseModel.Message = "Something went wrong";
        //                _responseModel.Status = false;
        //                _responseModel.StatusCode = HttpStatusCode.BadRequest;                      
        //            }
        //        }
        //        else
        //        {
        //            _responseModel.Message = "Purchase Invoice does not exists";
        //            _responseModel.Status = false;
        //            _responseModel.StatusCode = HttpStatusCode.BadRequest;
        //            _responseModel.Data = null;                  
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        _responseModel.Message = ex.Message;
        //        _responseModel.Status = false;
        //        _responseModel.StatusCode = HttpStatusCode.InternalServerError;
        //    }         


        //    return _responseModel;
        //}

        //private ApiResponseModel UpdatePILine(VMS_PURCHASE_LINE_RESPONSE piLineERPModel)
        //{
        //    try
        //    {
        //        var objpurchLine = _dbContext.FicciImpiLines.Where(x => x.ImpiDocumentNo.Equals(piLineERPModel.documentNo) && x.ImpiLineNo == Convert.ToString(piLineERPModel.lineNo)).FirstOrDefault();

        //        if (objpurchLine != null)
        //        {
        //            //objpurchLine.PL_NATURE_OF_EXPENSE = piLineERPModel.NATURE_OF_EXPENSE;
        //            objpurchLine.ImpiGlNo = piLineERPModel.no_;
        //            objpurchLine.ImpiQuantity = Convert.ToString(piLineERPModel.quantity);
        //            objpurchLine.ImpiUnitPrice = piLineERPModel.unitPrice;
        //            objpurchLine.ImpiLineAmount = piLineERPModel.lineAmount;
        //            objpurchLine.ImpiGstgroupCode = piLineERPModel.gSTGroupCode;
        //            objpurchLine.ImpiHsnsaccode = piLineERPModel.hSN_SAC_Code;

        //            _dbContext.Update(objpurchLine);
        //            _dbContext.SaveChanges();


        //            int count = _dbContext.SaveChanges();

        //            if (count > 0)
        //            {
        //                _responseModel.Message = "Success";
        //                _responseModel.Status = true;
        //                _responseModel.StatusCode = HttpStatusCode.OK;
        //                _responseModel.Data = _dbContext.FicciImpiLines.Where(x => x.ImpiDocumentNo.Equals(piLineERPModel.documentNo) && x.ImpiLineNo == Convert.ToString(piLineERPModel.lineNo)).FirstOrDefault();


        //            }
        //            else
        //            {
        //                _responseModel.Message = "Something went wrong";
        //                _responseModel.Status = false;
        //                _responseModel.StatusCode = HttpStatusCode.BadRequest;                      
        //            }
        //        }
        //        else
        //        {
        //            _responseModel.Message = "Purchase Invoice Line does not exists";
        //            _responseModel.Status = false;
        //            _responseModel.StatusCode = HttpStatusCode.BadRequest;
        //            _responseModel.Data = null;                   
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //  log.Error("UpdatePILine function", ex);
        //        _responseModel.Message = ex.Message;
        //        _responseModel.Status = false;
        //        _responseModel.StatusCode = HttpStatusCode.InternalServerError;              
        //    }           

        //    return _responseModel;
        //}

        //private ApiResponseModel UpdateHSNMastersDetails(HSNSACMaster hSNErpModel)
        //{
        //    try
        //    {
        //        var obj = _dbContext.Hsnsacs.Where(x => x.HsnGroup == hSNErpModel.GST_Group_Code&& x.HsnCode == hSNErpModel.Code).SingleOrDefault();
        //        if (obj != null)
        //        {
        //            obj.HsnCode = hSNErpModel.Code;                  
        //            obj.HsnGroup = hSNErpModel.GST_Group_Code;

        //            _dbContext.Hsnsacs.Add(obj);                  
        //            int count = _dbContext.SaveChanges();

        //            if (count > 0)
        //            {
        //                _responseModel.Message = "HSN SAC information updated successfully";
        //                _responseModel.Status = true;
        //                _responseModel.StatusCode = HttpStatusCode.OK;
        //                _responseModel.Data = _dbContext.Hsnsacs.Where(x => x.HsnGroup == obj.HsnGroup && x.HsnCode == obj.HsnCode).SingleOrDefault();

        //                return _responseModel;
        //            }
        //            else
        //            {
        //                _responseModel.Message = "Something went wrong";
        //                _responseModel.Status = false;
        //                _responseModel.StatusCode = HttpStatusCode.BadRequest;
        //                return _responseModel;
        //            }
        //        }
        //        else
        //        {
        //            Hsnsac obj1 = new Hsnsac();

        //            obj1.HsnCode = hSNErpModel.Code;
        //            obj1.HsnGroup = hSNErpModel.GST_Group_Code;  
        //            _dbContext.Hsnsacs.Add(obj1);
        //            int count = 0;                   
        //            count = _dbContext.SaveChanges();

        //            if (count > 0)
        //            {
        //                _responseModel.Message = "HSN SAC Information Inserted successfully";
        //                _responseModel.Status = true;
        //                _responseModel.StatusCode = HttpStatusCode.OK;
        //                _responseModel.Data = _dbContext.Hsnsacs.Where(x => x.HsnGroup == obj1.HsnGroup && x.HsnCode == obj1.HsnCode).SingleOrDefault();
        //                return _responseModel;
        //            }
        //            else
        //            {
        //                _responseModel.Message = "Something went wrong";
        //                _responseModel.Status = false;
        //                _responseModel.StatusCode = HttpStatusCode.BadRequest;
        //                return _responseModel;
        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        // log.Error("UpdateHSNMastersDetails function", ex);

        //        _responseModel.Message = ex.Message;
        //        _responseModel.Status = false;
        //        _responseModel.StatusCode = HttpStatusCode.InternalServerError;
        //        return _responseModel;
        //    }
        //}


        #region Private Functions
        private async Task<string> GetSecurityToken()
        {
            string ClientId = "fa7e3cb6-60be-4d54-b515-81173f58d31e";
            string ClientSecret = "1FM8Q~ywhdiC39l2Rwy3yEqmhYVyJwlBWwlhub6e";
            string BearerToken = "";
            string URL = "https://login.microsoftonline.com/d3a55687-ec5c-433b-9eaa-9d952c913e94/oauth2/v2.0/token";

            HttpClient client1 = new HttpClient();

            var content = new StringContent("grant_type= Client_Credentials" +
                "&Scope= https://api.businesscentral.dynamics.com/.default" +
                "&client_id=" + HttpUtility.UrlEncode(ClientId) +
                "&client_secret=" + HttpUtility.UrlEncode(ClientSecret));


            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/x-www-form-urlencoded");
            var response = client1.PostAsync(URL, content).Result;

            if (response.IsSuccessStatusCode)
            {
                JObject Result = JObject.Parse(await response.Content.ReadAsStringAsync());
                BearerToken = Result["access_token"].ToString();


            }
            return "Bearer " + BearerToken;
        }

        public async Task<List<T>> GetList<T>(string serviceKey)
        {
            try
            {
                string result = await GetSecurityToken();
                var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("Authorization", result);
                string serviceURL = $"{_navServiceURL}/{_configuration[$"ERP:Services:{serviceKey}"]}";
                var response = await httpClient.GetAsync(serviceURL);
                response.EnsureSuccessStatusCode();
                var responseContent = response.Content.ReadAsStringAsync().Result;
                List<T> list = JsonConvert.DeserializeObject<ODataResponse<T>>(responseContent).Value.ToList();

                return list;
            }
            catch
            {
                throw;
            }
        }

        #endregion
    }
}
