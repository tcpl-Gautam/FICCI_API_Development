using FICCI_API.DTO.Account;
using FICCI_API.Interface;
using FICCI_API.Models;
using FICCI_API.ModelsEF;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FICCI_API.Controller;
using FICCI_API.Models.Services;
using FICCI_API.DTO;
using FICCI_API.Models.JWT;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Azure;
using Newtonsoft.Json.Linq;

namespace FICCI_API.Controller.API.Account
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAuthController : BaseController
    {
        private readonly FICCI_DB_APPLICATIONSContext _dbContext;
        private readonly IConfiguration _configuration;
        //private readonly JwtSettings jwtSettings;

        public UserAuthController(FICCI_DB_APPLICATIONSContext dbContext, IConfiguration configuration) : base(dbContext)
        {
            _dbContext = dbContext;
            _configuration = configuration;
           // this.jwtSettings = jwtSettings;
        }

        private async Task<UserRequestDto> AuthenticateUser(UserRequestDto request)
        {
            try
            {


                bool emailValid = await _dbContext.FicciImums.AnyAsync(x => x.ImumEmail == request.Email && x.ImumPassword == request.Password);
                if (emailValid)
                {
                    return request;
                }
                else
                {
                    throw new ArgumentException("Invalid credentials: Email and/or password incorrect.");

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private async Task<string> GenerateToken(UserRequestDto user)
        {
            try
            {
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(_configuration["Jwt:Issuer"], _configuration["Jwt:Audience"], null,
                    expires: DateTime.Now.AddMinutes(1), signingCredentials: credentials);
                return new JwtSecurityTokenHandler().WriteToken(token);

           
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody]UserRequestDto requestData)
        {
            try
            {
                IActionResult response = Unauthorized();

                if (requestData != null)
                {
                    var user = await AuthenticateUser(requestData); 
                    if(user != null)
                    {
                        var token = await GenerateToken(user);
                        var res = await _dbContext.FicciImums
                                        .Where(x => x.ImumEmail == requestData.Email && x.ImumPassword == requestData.Password && x.ImumActive != false)
                                        .Include(x => x.Role)
                                        .Select(user => new LoginData
                                        {
                                            Email = user.ImumEmail,
                                            Name = user.ImumName,
                                            EmpId = user.ImumEmpid,
                                            RoleName = _dbContext.TblFicciRoles.Where(m => m.RoleId == user.RoleId).Select(x => x.RoleName).FirstOrDefault().ToString(),
                                            IsApprover = _dbContext.FicciImems.Any(m => (m.ImemManagerEmail == requestData.Email || m.ImemDepartmentHeadEmail == requestData.Email || m.ImemClusterEmail == requestData.Email && m.ImemActive != false)),
                                            Invoice_IsApprover = _dbContext.FicciImpiHeaders.Any(m => (m.ImpiHeaderTlApprover == requestData.Email || m.ImpiHeaderClusterApprover == requestData.Email || m.ImpiHeaderFinanceApprover == requestData.Email && m.ImpiHeaderActive != false)),
                                            Invoice_IsTLApprover = _dbContext.FicciImpiHeaders.Any(m => (m.ImpiHeaderTlApprover == requestData.Email && m.ImpiHeaderActive != false)),
                                            Invoice_IsCHApprover = _dbContext.FicciImpiHeaders.Any(m => (m.ImpiHeaderClusterApprover == requestData.Email && m.ImpiHeaderActive != false)),
                                            Invoice_IsFinanceApprover = _dbContext.FicciImpiHeaders.Any(m => (m.ImpiHeaderFinanceApprover == requestData.Email && m.ImpiHeaderActive != false)),
                                            Token = token
                                        })
                                        .FirstOrDefaultAsync();
                        return Ok(res);
                    }
                    else
                    {
                        return Unauthorized();
                    }
                }
                else
                {
                    return response;

                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { status = false, message = "An error occurred." });
            }
        }


        //[HttpPost]
        //public async Task<IActionResult> Login(UserRequestDto requestData)
        //{
        //    try
        //    {
        //        if (requestData != null)
        //        {

        //            //check if email exist in database
        //            bool emailValid = await _dbContext.FicciImums.AnyAsync(x => x.ImumEmail == requestData.Email);
        //            if (!emailValid)
        //            {
        //                var response = new
        //                {
        //                    status = false,
        //                    message = "Email does not exist",
        //                };
        //                return StatusCode(200, response);
        //            }

        //            //TokenService token = new TokenService(_configuration);
        //            //var generateToken = await token.CreateToken(requestData);

        //            //var userlog = LogUserData(requestData.Email,"123",true);
        //            //if(userlog == false)
        //            //{
        //            //    return StatusCode(500, new { status = false, message = "An error occurred while saving data ." });

        //            //}
        //            //checks password and email for authenticate
        //            var res = await _dbContext.FicciImums
        //                .Where(x => x.ImumEmail == requestData.Email && x.ImumPassword == requestData.Password && x.ImumActive != false)
        //                .Include(x => x.Role)
        //                .Select(user  => new LoginData
        //                {
        //                    Email = user.ImumEmail,
        //                    Name = user.ImumName,
        //                    EmpId = user.ImumEmpid,
        //                    RoleName = _dbContext.TblFicciRoles.Where(m => m.RoleId == user.RoleId).Select(x => x.RoleName).FirstOrDefault().ToString(),
        //                    IsApprover =  _dbContext.FicciImems.Any(m => (m.ImemManagerEmail == requestData.Email|| m.ImemDepartmentHeadEmail == requestData.Email || m.ImemClusterEmail == requestData.Email && m.ImemActive != false)),
        //                    Invoice_IsApprover = _dbContext.FicciImpiHeaders.Any(m => (m.ImpiHeaderTlApprover == requestData.Email || m.ImpiHeaderClusterApprover == requestData.Email || m.ImpiHeaderFinanceApprover == requestData.Email && m.ImpiHeaderActive != false)),
        //                    Token = "123"
        //                })
        //                .FirstOrDefaultAsync();



        //            if (res == null)
        //            {
        //                var response = new
        //                {
        //                    status = false,
        //                    message = "Password is incorrect",
        //                    data = res
        //                };
        //                return Ok(response);
        //            }
        //            var respons = new
        //            {
        //                status = true,
        //                message = "Login Successful",
        //                data = res,
        //            };
        //            return Ok(respons);
        //        }
        //        else
        //        {
        //            return Unauthorized("User does not exist. Please check Email and Password");

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, new { status = false, message = "An error occurred." });
        //    }
        //}

        [HttpPost("GetToken")]
        public IActionResult GetToken(UserRequestDto userLogins)
        {
            try
            {
                var Token = new UserTokens();
                var Valid = logins.Any(x => x.UserName.Equals(userLogins.Email, StringComparison.OrdinalIgnoreCase));
                if (Valid)
                {
                    var user = logins.FirstOrDefault(x => x.UserName.Equals(userLogins.Email, StringComparison.OrdinalIgnoreCase));
                    //Token = JwtHelpers.GenTokenkey(new UserTokens()
                    //{
                    //    EmailId = user.EmailId,
                    //    GuidId = Guid.NewGuid(),
                    //    UserName = user.UserName,
                    //    Id = user.Id
                    //}, jwtSettings);
                }
                else
                {
                    return BadRequest("wrong password");
                }
                return Ok(Token);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [HttpGet]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult GetList()
        {
            return Ok(logins);
        }
        private IEnumerable<Users> logins = new List<Users>() {
            new Users() {
                    Id = Guid.NewGuid(),
                        EmailId = "adminakp@gmail.com",
                        UserName = "Admin",
                        Password = "Admin",
            },
                new Users() {
                    Id = Guid.NewGuid(),
                        EmailId = "adminakp@gmail.com",
                        UserName = "User1",
                        Password = "Admin",
                }
        };


        [NonAction]
        private bool LogUserData(string loginId, string generatedToken, bool status)
        {
            try
            {
                _dbContext.Userloginlogs.Add(new Userloginlog
                {
                    LoginId = loginId,
                    JwtToken = generatedToken,
                    LoginStatus = status,
                    LoginDate = DateTime.Now,
                    ExpiryDate = null
                });
                _dbContext.SaveChangesAsync();
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
            
        }
    }
}
