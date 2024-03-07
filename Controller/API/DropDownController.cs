using Azure;
using FICCI_API.DTO;
using FICCI_API.Interface;
using FICCI_API.Models;
using FICCI_API.ModelsEF;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.Metrics;

namespace FICCI_API.Controller.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class DropDownController : BaseController
    {
        private readonly FICCI_DB_APPLICATIONSContext _dbContext;
        private readonly ILogger<DropDownController> _logger;
        public DropDownController(FICCI_DB_APPLICATIONSContext dbContext, ILogger<DropDownController> logger) : base(dbContext)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        [HttpGet("GetRole")]

        public async Task<IActionResult> GetRole(int id)
        {
            try
            {
                var roles = await _dbContext.GetProcedures().prc_Role_listAsync(id);
                if (roles.Count > 0)
                {
                    var roleResponses = roles.Select(c => new Role
                    {
                        Role_id = c.Role_Id,
                        RoleName = c.Role_name,
                        IsActive = c.IsActive
                    }).ToList();

                    var response = new GetRoleResponse
                    {
                        status = true,
                        message = "Roles fetched successfully",
                        data = roleResponses
                    };

                    return Ok(response);
                }
                else
                {
                    var response = new GetRoleResponse
                    {
                        status = true,
                        message = "No roles found",
                        data = new List<Role>()
                    };

                    return Ok(response);
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }


        [HttpGet("GetEmployeeList")]
        public async Task<IActionResult> GetEmployeeList(int id)
        {
            try
            {
                var employees = await _dbContext.GetProcedures().prc_EmployeeMaster_listAsync(id);
                if (employees.Count > 0)
                {
                    var employeeResponse = employees.Select(c => new Employee_Master
                    {
                        IMEM_ID = c.IMEM_ID,
                        IMEM_Email = c.IMEM_EMAIL,
                        IMEM_Name = c.IMEM_NAME,
                        IMEM_EmpId = c.IMEM_EMPID,
                        IMEM_Username = c.IMEM_USERNAME,
                        IsActive = c.IMEM_ACTIVE

                    }).ToList();

                    var response = new GetEmployee_MasterResponse
                    {
                        status = true,
                        message = "Employee List Fetch",
                        data = employeeResponse
                    };

                    return Ok(response);
                }
                else
                {
                    var response = new GetEmployee_MasterResponse
                    {
                        status = true,
                        message = "No List Found",
                        data = new List<Employee_Master>()
                    };
                    return Ok(response);
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetCity")]
        public async Task<IActionResult> GetCity()
        {
            try
            {
                // Retrieve the bearer token from the Authorization header
                string accessToken = HttpContext.Request.Headers["Authorization"].ToString();

                // Remove the "Bearer " prefix to get just the token value
                accessToken = accessToken.Replace("Bearer ", "");
                CheckToken(accessToken);
                if(CheckToken == null)
                {
                    return Unauthorized("Token is expire");
                }
                _logger.LogWarning("Waring");
                _logger.LogInformation("Waring");
                _logger.LogDebug("Waring");
                var city = await _dbContext.Cities.Where(x => x.IsDelete != true && x.IsActive != false).OrderBy(x => x.CityName).ToListAsync();
                if (city.Count > 0)
                {
                    var cityResponse = city.Select(c => new CityInfo
                    {
                        cityCode = c.CityCode,
                        CityName = c.CityName,
                    }).ToList();

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
                        data = city
                    };
                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        [HttpGet("GetState")]
        public async Task<IActionResult> GetState()
        {
            try
            {
                var state = await _dbContext.States.Where(x => x.IsDelete != true && x.IsActive != false).OrderBy(x => x.StateName).ToListAsync();
                if (state.Count > 0)
                {
                    var stateResponse = state.Select(c => new StateInfo
                    {
                        stateCode = c.StateCode,
                        StateName = c.StateName,
                    }).ToList();

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
                        data = state
                    };
                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        [HttpGet("GetCountry")]
        public async Task<IActionResult> GetCountry()
        {
            try
            {
                var country = await _dbContext.Countries.Where(x => x.IsDelete != true && x.IsActive != false).OrderBy(x => x.CountryName).ToListAsync();
                if (country.Count > 0)
                {
                    var countryResponse = country.Select(c => new CountryInfo
                    {
                        countryCode = c.CountryCode,
                        CountryName = c.CountryName,
                    }).ToList();

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
                        data = country
                    };
                    return NotFound(response);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GstCustomerType")]
        public async Task<IActionResult> GstCustomerType()
        {
            try
            {
                var custType = await _dbContext.GstCustomerTypes.Where(x => x.IsDelete != true && x.IsActive != false).OrderBy(x => x.CustomerTypeName).ToListAsync();
                if (custType.Count > 0)
                {
                    var custResponse = custType.Select(c => new GSTCustomerTypeInfo
                    {
                        GstTypeId = c.CustomerTypeId,
                        GstTypeName = c.CustomerTypeName,
                    }).ToList();
                    var response = new
                    {
                        status = true,
                        message = "GstCustomerType List fetch successfully",
                        data = custResponse
                    };
                    return Ok(response);
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        message = "No Country list found",
                        data = custType
                    };
                    return NotFound(response);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetProject")]
        public async Task<IActionResult> GetProject(int id = 0)
        {
           // var result = new ProjectDTO();
            //var resu = new List<AllProjectList>();
            try
            {

                var result = await _dbContext.FicciErpProjectDetails.Where(x => x.IsDelete != true && x.ProjectActive != false).ToListAsync();
                if (result.Count > 0)
                {
                    var resonse = result.Select(c => new ProjectDTO
                    {
                        dimension_Code = c.DimensionCode,
                        code = c.ProjectCode,
                        name = c.ProjectName,
                        departmentCode = c.ProjectDepartmentCode,
                        departmentName = c.ProjectDepartmentName,
                        divisionCode = c.ProjectDivisionCode,
                        divisionName = c.ProjectDivisionName,
                        tlApprover =c.TlApprover,
                        chApprover =c.ChApprover,
                        financeApprover =c.FinanceApprover,
                        supportApprover =c.SupportApprover
                    }).ToList();

                    if (id > 0)
                    {
                        result = result.Where(m => m.ProjectId == id).ToList();
                    }
                    var response = new
                    {
                        status = true,
                        message = "List fetch successfully",
                        data = resonse
                    };

                    return Ok(response);
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        message = "No records found",

                    };

                    return NotFound(response);
                }

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { status = false, message = "An error occurred while fetching the detail of projects." });
            }
        }

        [HttpGet("GetCustomer")]
        public async Task<IActionResult> GetCustomer()
        {
            try
            {
                var result = await _dbContext.Erpcustomers.ToListAsync();
                if(result.Count > 0)
                {
                    var response = new
                    {
                        status = true,
                        message = "List fetch successfully",
                        data = result
                    };
                    return Ok(response);
                }
                else
                {
                    var response = new
                    {
                        status = true,
                        message = "No records found",

                    };

                    return NotFound(response);
                }
            }
            catch(Exception ex)
            {
                return StatusCode(500, new { status = false, message = "An error occurred while fetching the detail of projects." });

            }
        }

        [HttpGet("GetCategory")]
        public async Task<IActionResult> GetCategory()
        {
            Drp_CategoryList drp_CategoryList = new Drp_CategoryList();
            try
            {
                var list = await _dbContext.GetProcedures().prc_drp_categorylistAsync();
                if (list.Count > 0)
                {
                    foreach (var k in list)
                    {
                        Drp_CategoryListResponse drp_CategoryListResponse = new Drp_CategoryListResponse();
                        drp_CategoryListResponse.Id = k.Id;
                        drp_CategoryListResponse.Category_Name = k.Category_Name;
                        drp_CategoryList.Data.Add(drp_CategoryListResponse);
                    }
                    drp_CategoryList.status = true;
                    drp_CategoryList.message = "List Fecth Successfully";
                    return StatusCode(200, drp_CategoryList);
                }
                else
                {
                    drp_CategoryList.status = false;
                    drp_CategoryList.message = "Data Not found";
                    return StatusCode(200, drp_CategoryList);
                }

            }
            catch (Exception ex)
            {
                drp_CategoryList.status = false;
                drp_CategoryList.message = ex.InnerException.Message.ToString();
                return StatusCode(500, drp_CategoryList);
            }
        }



        [HttpGet("GetCOAMaster")]
        public async Task<IActionResult> GetCOAMaster()
        {
            try
            {
                var city = await _dbContext.CoaMasters.Where(x => x.IsActive != false).OrderBy(x => x.CoaName).ToListAsync();
                if (city.Count > 0)
                {
                    var cityResponse = city.Select(c => new COAInfo
                    {
                        No = c.CoaNo,
                        Name = c.CoaName,
                    }).ToList();

                    var response = new
                    {
                        status = true,
                        message = "COA List fetch successfully",
                        data = cityResponse
                    };
                    return Ok(response);

                }
                else
                {
                    var response = new
                    {
                        status = true,
                        message = "No COA list found",
                        data = city
                    };
                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetLocation")]
        public async Task<IActionResult> GetLocation()
        {
            try
            {
                var city = await _dbContext.Locations.OrderBy(x => x.LocationName).ToListAsync();
                if (city.Count > 0)
                {
                    var cityResponse = city.Select(c => new LoactionInfo
                    {
                        code = c.LocationCode,
                        name = c.LocationName,
                    }).ToList();

                    var response = new
                    {
                        status = true,
                        message = "Location List fetch successfully",
                        data = cityResponse
                    };
                    return Ok(response);

                }
                else
                {
                    var response = new
                    {
                        status = true,
                        message = "No Location list found",
                        data = city
                    };
                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetGSTGroup")]
        public async Task<IActionResult> GetGSTGroup()
        {
            try
            {
                var city = await _dbContext.Gstgroups.Where(x => x.IsActive !=false).ToListAsync();
                if (city.Count > 0)
                {
                    var cityResponse = city.Select(c => new GSTGroupInfo
                    {
                        Code = c.GroupCode,

                    }).ToList();

                    var response = new
                    {
                        status = true,
                        message = "GSTGroup List fetch successfully",
                        data = cityResponse
                    };
                    return Ok(response);

                }
                else
                {
                    var response = new
                    {
                        status = true,
                        message = "No GSTGroup list found",
                        data = city
                    };
                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetHSNSAC")]
        public async Task<IActionResult> GetHSNSAC(string gstCode)
        {
            try
            {
                var city = await _dbContext.Hsnsacs.Where(x => x.HsnGroup == gstCode).ToListAsync();
               // var city = await _dbContext.Hsnsacs.ToListAsync();
                if (city != null)
                {


                    var response = new
                    {
                        status = true,
                        message = "HSNSAC List fetch successfully",
                        data = city
                    };
                    return Ok(response);

                }
                else
                {
                    var response = new
                    {
                        status = true,
                        message = "No HSNSAC list found",
                        data = city
                    };
                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
