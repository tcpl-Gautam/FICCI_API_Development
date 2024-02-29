using FICCI_API.DTO;
using FICCI_API.Models;
using FICCI_API.ModelsEF;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FICCI_API.Controller
{
    //[Route("api/[controller]")]
    //[ApiController]
    public class ApproverController : BaseController
    {
        private readonly FICCI_DB_APPLICATIONSContext _dbContext;
        public ApproverController(FICCI_DB_APPLICATIONSContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        //[HttpPost]
        //public async Task<IActionResult> Post(ApproveCustomer cust)
        //{
        //    ApproverCustomer_Crud crud = new ApproverCustomer_Crud();

        //    try
        //    {
        //        var res = await _dbContext.GetProcedures().prc_Approval_CustomerAsync(cust.CustomerId.ToString(), cust.IsApproved, cust.LoginId, cust.StatusId, cust.Remarks);
        //        if (res[0].returncode == 1)
        //        { 
        //            var result = await _dbContext.FicciErpCustomerDetails.Where(x => x.CustomerId == Convert.ToInt32(res[0].CustomerId)).FirstOrDefaultAsync();

        //            string htmlbody = htmlBody(res[0].Status, result.CusotmerNo, result.CustomerName, result.CityCode, result.CustomerPanNo, result.CustomerGstNo);
        //          //  SendEmail(res[0].InitiatedBy, result.CustomerEmailId, "http", $"New Customer Assigned for Approval : {result.CustomerName}", htmlbody,);

        //        }
        //        crud.status = res[0].returncode == 1 ? true : false;
        //        crud.message = res[0].Message;
        //        return StatusCode(200, crud);

        //    }
        //    catch (Exception ex)
        //    {
        //        crud.status = false;
        //        crud.message = ex.InnerException.Message.ToString();
        //        return StatusCode(500, crud);
        //    }
        //}
    }
}
