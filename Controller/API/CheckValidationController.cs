using FICCI_API.DTO;
using FICCI_API.ModelsEF;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FICCI_API.Controller.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class CheckValidationController : BaseController
    {
        private readonly FICCI_DB_APPLICATIONSContext _dbContext;
        public CheckValidationController(FICCI_DB_APPLICATIONSContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("CheckGST")]
        public async Task<IActionResult> CheckGST(string GST)
        {
            try
            {
                var resu = await _dbContext.Erpcustomers.Where(x => x.GstregistrationNo == GST).FirstOrDefaultAsync();
                if (resu != null)
                {
                    var responseObject = new
                    {
                        status = false,
                        message = "GST number already exists"
                    };
                    return StatusCode(200, responseObject);
                }
                else
                {
                    var responseObject = new
                    {
                        status = true,
                        message = "GST number not exits"
                    };
                    return StatusCode(200, responseObject);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet("CheckPAN")]
        public async Task<IActionResult> CheckPAN(string PAN)
        {
            try
            {
                var resu = await _dbContext.Erpcustomers.Where(x => x.PanNo == PAN).FirstOrDefaultAsync();
                if (resu != null)
                {
                    var responseObject = new
                    {
                        status = false,
                        message = "PAN number already exists"
                    };
                    return StatusCode(200, responseObject);
                }
                else
                {
                    var responseObject = new
                    {
                        status = true,
                        message = "PAN number not exits"
                    };
                    return StatusCode(200, responseObject);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
