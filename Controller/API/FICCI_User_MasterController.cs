using Azure;
using FICCI_API.Models;
using FICCI_API.ModelsEF;
using Microsoft.AspNetCore.Mvc;

namespace FICCI_API.Controller.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class FICCI_User_MasterController : BaseController
    {

        private readonly FICCI_DB_APPLICATIONSContext _dbContext;
        public FICCI_User_MasterController(FICCI_DB_APPLICATIONSContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost]
        public async Task<IActionResult> Post(PostUserMaster data)
        {
            GetEmployee_MasterResponse response = new GetEmployee_MasterResponse();

            try
            {
                var res = await _dbContext.GetProcedures().prc_UserMaster_FormAsync(data.IsUpdate, data.IMEM_EmpId, data.IMEM_ID, data.IMEM_Name, data.IMEM_Email, data.RoleId, data.IsActive, data.IMEM_Username);
                
                if (res[0].returncode > 0)
                {
                    data.IMEM_ID = res[0].returncode;
                    data.RoleName = _dbContext.TblFicciRoles .Where(m => m.RoleId == data.RoleId).Select(x => x.RoleName).FirstOrDefault().ToString();
                    var responseObject = new
                    {
                        data = data,
                        status = true,
                        message = res[0].Message
                    };
                    return StatusCode(200, responseObject);
                }
                else
                {
                    response.status = false;
                    response.message = res[0].Message;
                    return StatusCode(500, response);
                }
            }
            catch (Exception ex)
            {
                response.status = false;
                response.message = ex.InnerException.Message.ToString();
                return StatusCode(500, response);
            }
        }

        [HttpDelete("{IMEM_ID}")]
        public async Task<IActionResult> Delete(int IMEM_ID)
        {
            GetEmployee_MasterResponse response = new GetEmployee_MasterResponse();
            try
            {
                if(IMEM_ID == 0)
                {
                    return NotFound("No User Master found for Delete");
                }
                var res = await _dbContext.GetProcedures().prc_UserMaster_DeleteAsync(IMEM_ID);
                response.status = res[0].returncode == 1 ? true : false;
                response.message = res[0].Message;
                return StatusCode(200, response);

            }
            catch(Exception ex)
            {
                response.status = false;
                response.message = ex.InnerException.Message.ToString();
                return StatusCode(500, response);
            }
        }

        [HttpGet("{IMEM_ID}")]
        public async Task<IActionResult> Get(int IMEM_ID = 0)
        {
            GetEmployee_MasterResponse response = new GetEmployee_MasterResponse();
            try
            {
                var res = await _dbContext.GetProcedures().prc_FICCI_IMUM_listAsync(IMEM_ID);

                if (res.Count > 0)
                {
                    foreach (var emp in res)
                    {
                        Employee_Master list = new Employee_Master();
                        list.IMEM_ID = emp.IMUM_ID;
                        list.RoleName = emp.Role_name;
                        list.IMEM_Name = emp.IMUM_NAME;
                        list.IMEM_Email = emp.IMUM_EMAIL;
                        list.IMEM_EmpId = emp.IMUM_EMPID;
                        list.IsActive = emp.IMUM_ACTIVE;
                        list.IMEM_Username = emp.UserName;
                        response.data.Add(list);

                    }
                    response.status = true;
                    response.message = "List fetch successfully";
                    return StatusCode(200, response);

                }
                else
                {
                    response.status = false;
                    response.message = "Data not found";
                    return StatusCode(200, response);
                }
            
            }
            catch (Exception ex)
            {
                response.status = false;
                response.message = ex.InnerException.Message.ToString();
                return StatusCode(500, response);
            }
        }
    }
}
