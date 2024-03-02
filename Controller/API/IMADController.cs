using FICCI_API.ModelsEF;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FICCI_API.Controller.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class IMADController : BaseController
    {
        private readonly FICCI_DB_APPLICATIONSContext _dbContext;
        public IMADController(FICCI_DB_APPLICATIONSContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        [HttpDelete]

        public async Task<IActionResult> DELETE(int imadId)
        {
            PO_delete pO_Delete = new PO_delete();

            try
            {

                var list = await _dbContext.FicciImads.Where(m => m.ImadId == imadId).FirstOrDefaultAsync();

                list.ImadActive = false;
                await _dbContext.SaveChangesAsync();

                pO_Delete.status = true;
                pO_Delete.message = "Delete Successfully";
                return StatusCode(200, pO_Delete);

            }
            catch (Exception ex)
            {
                pO_Delete.status = false;
                pO_Delete.message = "Invalid Data";
                return StatusCode(500, pO_Delete);
            }
        }
    }
}
