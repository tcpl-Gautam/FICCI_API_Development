using FICCI_API.Models;

namespace FICCI_API.DTO.Account
{
    public class AppUser
    {
        public string EmpId { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public int RoleId { get; set; }
        public string UserId { get; set; }

    }
    public class Users
    {
        public string UserName
        {
            get;
            set;
        }
        public Guid Id
        {
            get;
            set;
        }
        public string EmailId
        {
            get;
            set;
        }
        public string Password
        {
            get;
            set;
        }
    }
    public class UserRequestDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
    public class LoginData
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string EmpId { get; set; }
        public string RoleName { get; set; }

        public bool IsApprover { get; set; }
        public bool Invoice_IsApprover { get; set; }
        
        public string? Token { get; set; }
    }
}
