using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FICCI_API.ModelsEF;
using FICCI_API.Models;

namespace FICCI_API.Controller.API
{
    //[Route("api/[controller]")]
    //[ApiController]
    public class Drp_CategoryListController : BaseController
    {
        private readonly FICCI_DB_APPLICATIONSContext _dbContext;
        public Drp_CategoryListController(FICCI_DB_APPLICATIONSContext dbContext):base(dbContext) 
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            Drp_CategoryList drp_CategoryList = new Drp_CategoryList();
            try
            {
                var list = await _dbContext.GetProcedures().prc_drp_categorylistAsync();
                if(list.Count > 0)
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
    }
}
