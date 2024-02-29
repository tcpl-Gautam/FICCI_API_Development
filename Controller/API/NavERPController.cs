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
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Xml.Linq;

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
        public NavERPController(IConfiguration configuration, FICCI_DB_APPLICATIONSContext dbContext): base(dbContext)
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
                foreach(var k in cityList)
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
                List<CustomerModel> cityList = await GetList<CustomerModel>("Customer");
                _dbContext.Database.ExecuteSqlRaw($"TRUNCATE TABLE ERPCustomer");
                foreach (var k in cityList)
                {
                    Erpcustomer cust = new Erpcustomer();
                    cust.CustNo = k.No;
                    cust.CustName = k.Name;
                    cust.CustName2 = k.Name2;
                    cust.CustAddress = k.Address;
                    cust.CustAddress2 = k.Address2;
                    cust.City = k.City;
                    cust.StateCode = k.State_Code;
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
                if(COAMaster.Count > 0)
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
                if(GSTGroup.Count > 0)
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
                if(HSNSACMaster.Count > 0)
                {
                    var hsnResponse = HSNSACMaster.Select(c => new HSNSACMasterInfo
                    {
                        Code = c.Code,
                        GST_Group_Code = c.GST_Group_Code
                    }).ToList();
                    _dbContext.Database.ExecuteSqlRaw($"TRUNCATE TABLE HSNSAC");
                    foreach(var hsn in hsnResponse)
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
                if(GetLocation.Count > 0)
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


        [HttpPost("PostCustomer")]
        //Submit customer not drafted
        public async Task<IActionResult> PostCustomer(CustomerRequest data)
        {
            try
            {
            //    HttpContent content = new StringContent("{'insertPurchInvHeadDetails': '" + insertPurchCreditMemoDetails + "','insertPurchInvLineDetails': '" + insertPurchCreditMemoLineDetails + "' }", System.Text.Encoding.UTF8, "application/json");



            //    //var username = "TEAMCOMPUTERS.CRM";
            //    //var password = "gXAXZPsieceCYWsxzjzzCYayJe53ZtAFEXitC+xgA08=";
            //    //string svcCredentials = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(username + ":" + password));
            //    //var httpClient = new HttpClient();
            //    //httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", svcCredentials);

            //    var response = httpClient.PostAsync(erpURL + "ODataV4/VMSServiceCreditMemo_InsertPurchaseInvoice?company=FICCI", content);

            //    response.Result.EnsureSuccessStatusCode();

            //    var responseContent = response.Result.Content.ReadAsStringAsync().Result;

            //    var odata = JsonConvert.DeserializeObject<OData>(responseContent);

            }
            catch(Exception ex) {
                _responseModel.Data = ex;
                _responseModel.Status = false;
                _responseModel.Message = ex.Message;                
            }
            return Ok(_responseModel);
        }


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
