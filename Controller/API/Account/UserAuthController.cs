using FICCI_API.DTO.Account;
using FICCI_API.Interface;
using FICCI_API.Models;
using FICCI_API.ModelsEF;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FICCI_API.Controller;

namespace FICCI_API.Controller.API.Account
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAuthController : BaseController
    {
        private readonly FICCI_DB_APPLICATIONSContext _dbContext;
       // private readonly ITokenService _tokenRepo;
        public UserAuthController(FICCI_DB_APPLICATIONSContext dbContext) : base(dbContext)
        {
           // _tokenRepo = tokenRepo ?? throw new ArgumentNullException(nameof(tokenRepo));
            _dbContext = dbContext;
        }


        //[AllowAnonymous]
        //[HttpPost]
        //public async Task<IActionResult> ValidateUser([FromBody] UserRequestDto requestData)
        //{
        //    if (requestData == null)
        //    {
        //        return Unauthorized("User does not exist. Please check Email and Password");
        //    }
        //}


        //private async Task LogUserData(int empId, string generatedToken, bool status)
        //{
        //    _dbContext.Userloginlogs.Add(new Userloginlog
        //    {
        //        EmpId = empId,
        //        JwtToken = generatedToken,
        //        LoginStatus = status,
        //    });
        //    await _dbContext.SaveChangesAsync();
        //}

        [HttpPost]
        public async Task<IActionResult> Login(UserRequestDto requestData)
        {
            try
            {
                if (requestData != null)
                {
                  

                    //check if email exist in database
                    bool emailValid = await _dbContext.FicciImums.AnyAsync(x => x.ImumEmail == requestData.Email);
                    if (!emailValid)
                    {
                        var response = new
                        {
                            status = false,
                            message = "Email does not exist",
                        };
                        return StatusCode(200, response);
                    }
                   
                    //checks password and email for authenticate
                    var res = await _dbContext.FicciImums
                        .Where(x => x.ImumEmail == requestData.Email && x.ImumPassword == requestData.Password && x.ImumActive != false)
                        .Include(x => x.Role)
                        .Select(user  => new LoginData
                        {
                            Email = user.ImumEmail,
                            Name = user.ImumName,
                            EmpId = user.ImumEmpid,
                            RoleName = _dbContext.TblFicciRoles.Where(m => m.RoleId == user.RoleId).Select(x => x.RoleName).FirstOrDefault().ToString(),
                            IsApprover =  _dbContext.FicciImems.Any(m => (m.ImemManagerEmail == requestData.Email|| m.ImemDepartmentHeadEmail == requestData.Email || m.ImemClusterEmail == requestData.Email && m.ImemActive != false)),
                            Invoice_IsApprover = _dbContext.FicciImpiHeaders.Any(m => (m.ImpiHeaderTlApprover == requestData.Email || m.ImpiHeaderClusterApprover == requestData.Email || m.ImpiHeaderFinanceApprover == requestData.Email && m.ImpiHeaderActive != false)),
                            Token = user.ImumEmail
                        })
                        .FirstOrDefaultAsync();

                    if (res == null)
                    {
                        var response = new
                        {
                            status = false,
                            message = "Password is incorrect",
                            data = res
                        };
                        return Ok(response);
                    }
                    var respons = new
                    {
                        status = true,
                        message = "Login Successful",
                        data = res,
                    };
                    return Ok(respons);
                }
                else
                {
                    return Unauthorized("User does not exist. Please check Email and Password");

                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { status = false, message = "An error occurred while fetching the detail of Customers." });
            }
        }   
    }
}
