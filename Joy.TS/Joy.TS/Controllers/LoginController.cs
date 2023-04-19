using Joy.TS.DAL.Data;
using Joy.TS.DAL.Model;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using static Joy.TS.BAL.DomainModel.AdminDomainModel;
using static Joy.TS.BAL.DomainModel.EmployeeDomainModel;
using Microsoft.EntityFrameworkCore;

namespace Joy.TS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly TimeSheetContext _timesheetContext;

        public LoginController(IConfiguration configuration
            , TimeSheetContext timesheetContext)
        {
            _configuration = configuration;
            _timesheetContext = timesheetContext;
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] Login loginModel)
        {
            var Email = await _timesheetContext.employees.FirstOrDefaultAsync(i => i.Official_Email == loginModel.Email);
            if (Email == null)
            {
                return NotFound();
            }
            string Passwordpattern = "^(?=.*[A-Z])(?=.*[!@#$%^&*()_+\\-=\\[\\]{};':\"\\\\|,.<>\\/?])[A-Za-z0-9!@#$%^&*()_+\\-=\\[\\]{};':\"\\\\|,.<>\\/?]{8,}$";
            if (loginModel.Password == "" || !Regex.IsMatch(loginModel.Password, Passwordpattern))
            {
                return BadRequest("Password should contain first letter should capital letter and one special symbol");
            }
            List<Claim> claims = new List<Claim>
            {
                        new Claim(ClaimTypes.Email, loginModel.Password)
            };
            var newKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            _configuration.GetSection("AppSettings:Token").Value!));
            var creds = new SigningCredentials(newKey, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(claims: claims, expires: DateTime.Now.AddDays(1), signingCredentials: creds);
            var y = _timesheetContext.employees.FirstOrDefault(e => e.Official_Email == loginModel.Email);
            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo,
                Employee_Id = y.Employee_Id,
                Role_Id = y.Role_Id,
            });

        }
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> RegisterAdmin([FromBody] AddEmployeeModel registerModel)
        {
            string email = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            if (registerModel.Official_Email == "" || !Regex.IsMatch(registerModel.Official_Email, email))
            {
                return BadRequest("Invalied Email");
            }
            var Email = await _timesheetContext.employees.FirstOrDefaultAsync(i => i.Official_Email == registerModel.Official_Email);
            if (Email != null)
            {
                return BadRequest("User already existes");
            }
            string Passwordpattern = "^(?=.*[A-Z])(?=.*[!@#$%^&*()_+\\-=\\[\\]{};':\"\\\\|,.<>\\/?])[A-Za-z0-9!@#$%^&*()_+\\-=\\[\\]{};':\"\\\\|,.<>\\/?]{8,}$";
            if (registerModel.Password == "" || !Regex.IsMatch(registerModel.Password, Passwordpattern))
            {
                return BadRequest("Password should contain first letter should capital letter and one special symbol");
            }
            Employee emp = new Employee();
            emp.First_Name = registerModel.First_Name;
            emp.Last_Name = registerModel.Last_Name;
            emp.Employee_code = registerModel.Employee_code;
            emp.Employee_Type_Id = registerModel.Employee_Type_Id;
            emp.Designation_Id = registerModel.Designation_Id;
            emp.Official_Email = registerModel.Official_Email;
            emp.Alternate_Email = registerModel.Alternate_Email;
            emp.Contact_No = registerModel.Contact_No;
            emp.Create_Date = DateTime.Now.Date;
            emp.Role_Id = registerModel.role_id;
            emp.Joining_Date = registerModel.Joining_Date;
            emp.Is_Active = true;
            emp.Password = registerModel.Password;
            emp.Reporting_Manager1 = registerModel.Reporting_Manager1;
            emp.Reportinng_Manager2 = registerModel.Reportinng_Manager2;
            _timesheetContext.employees.Add(emp);
            _timesheetContext.SaveChanges();
            return Ok("User Added Successfully..!");
        }

        [HttpPost, Authorize]
        [Route("Change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordModel Cmodel)
        {
            var Data = await _timesheetContext.employees.FirstOrDefaultAsync(i => i.Official_Email == Cmodel.Email && i.Password == Cmodel.Password);
            if (Data == null)
            {
                return BadRequest("Email is not existes or entered password is Wrong ");
            }
            if (Cmodel.NewPassword == "" && Cmodel.ConfrimNewPassword == "")
            {
                return BadRequest("NewPassword and ConfrimNewPassword should not be empty");
            }
            if (Cmodel.NewPassword != Cmodel.ConfrimNewPassword)
            {
                return BadRequest("new password and ConfrimNewPassword should be match");
            }
            string Passwordpattern = "^(?=.*[A-Z])(?=.*[!@#$%^&*()_+\\-=\\[\\]{};':\"\\\\|,.<>\\/?])[A-Za-z0-9!@#$%^&*()_+\\-=\\[\\]{};':\"\\\\|,.<>\\/?]{8,}$";
            if (!Regex.IsMatch(Cmodel.NewPassword, Passwordpattern))
            {
                return BadRequest("Password should contain first letter should capital letter and one special symbol");
            }

            Data.Password = Cmodel.NewPassword;
            _timesheetContext.SaveChanges();

            return Ok("Password Updated successFully..!");

        }
        [HttpGet]
        public IEnumerable<Employee> Get()
        {
            return _timesheetContext.employees.ToList();
        }

        [HttpPut]
        [Route("ForgetPassword")]
        public IActionResult ResetPassword(ForgetPassword forgetPassword)
        {
            var data = _timesheetContext.employees.FirstOrDefault(i => i.Official_Email==forgetPassword.Email);
            if (data== null)
            {
                return NotFound();
            }
            string Passwordpattern = "^(?=.*[A-Z])(?=.*[!@#$%^&*()_+\\-=\\[\\]{};':\"\\\\|,.<>\\/?])[A-Za-z0-9!@#$%^&*()_+\\-=\\[\\]{};':\"\\\\|,.<>\\/?]{8,}$";
            if (!Regex.IsMatch(forgetPassword.Password, Passwordpattern))
            {
                return BadRequest("Password should contain first letter should capital letter and one special symbol");
            }
            data.Password = forgetPassword.Password;
            _timesheetContext.employees.Update(data);
            _timesheetContext.SaveChanges();
            return Ok("Password is reset");
        }
    }
}
