using FICCI_API.Models;
using FICCI_API.ModelsEF;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FICCI_API.Controller.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConfigurationController : BaseController
    {
        private readonly FICCI_DB_APPLICATIONSContext _dbContext;
        public ConfigurationController(FICCI_DB_APPLICATIONSContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("{C_ID}")]
        public async Task<IActionResult> Get(int C_ID=0 )
        {
            Configuration_CRUD Configuration_CRUD = new Configuration_CRUD();
            try
            {
                var list = await _dbContext.GetProcedures().prc_Configuration_listAsync(C_ID);
                if (list.Count > 0)
                {
                    foreach (var k in list)
                    {
                        Configuration_List Configuration_List = new Configuration_List();
                        Configuration_List.C_ID = k.C_ID;
                        Configuration_List.C_Code = k.C_Code;
                        Configuration_List.C_Value = k.C_Value;
                        Configuration_List.Category_Name = k.Category_Name;
                        Configuration_List.IsActive = k.IsActive;
                        Configuration_CRUD.Data.Add(Configuration_List);
                    }
                    Configuration_CRUD.status = true;
                    Configuration_CRUD.message = "List Fecth Successfully";
                    return StatusCode(200, Configuration_CRUD);
                }
                else
                {
                    Configuration_CRUD.status = false;
                    Configuration_CRUD.message = "Data Not found";
                    return StatusCode(200, Configuration_CRUD);
                }

            }
            catch (Exception ex)
            {
                Configuration_CRUD.status = false;
                Configuration_CRUD.message = ex.InnerException.Message.ToString();
                return StatusCode(500, Configuration_CRUD);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(Configuration_Post_Request request)
        {
            Configuration_CRUD Configuration_CRUD = new Configuration_CRUD();
            try
            {
                var list = await _dbContext.GetProcedures().prc_Configuration_FormAsync(request.IsUpdate, request.C_ID, request.C_Code, request.C_Value, request.CategoryID, request.User, request.Isactive);
                Configuration_CRUD.status = list[0].returncode == 1 ? true : false;
                Configuration_CRUD.message = list[0].Message;
                return StatusCode(200, Configuration_CRUD);
            }
            catch (Exception ex)
            {
                Configuration_CRUD.status = false;
                Configuration_CRUD.message = ex.InnerException.Message.ToString();
                return StatusCode(500, Configuration_CRUD);
            }
        }

        [HttpDelete("{C_ID}")]
        public async Task<IActionResult> Delete(int C_ID)
        {
            Configuration_CRUD Configuration_CRUD = new Configuration_CRUD();
            try
            {
                var list = await _dbContext.GetProcedures().prc_Configuration_DeleteAsync(C_ID);



                Configuration_CRUD.status = list[0].returncode == 1 ? true : false;
                Configuration_CRUD.message = list[0].Message;
                return StatusCode(200, Configuration_CRUD);
            }
            catch (Exception ex)
            {
                Configuration_CRUD.status = false;
                Configuration_CRUD.message = ex.InnerException.Message.ToString();
                return StatusCode(500, Configuration_CRUD);
            }
        }
    }
}
