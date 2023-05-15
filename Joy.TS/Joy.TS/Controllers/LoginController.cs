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
using DocumentFormat.OpenXml.Spreadsheet;
using System.Net.Mail;
using System.Net;
using DocumentFormat.OpenXml.Wordprocessing;
using Joy.TS.BAL.Implementation;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace Joy.TS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly TimeSheetContext _timesheetContext;
        private readonly ILogin _login;

        public LoginController(IConfiguration configuration
            , TimeSheetContext timesheetContext, ILogin login)
        {
            _configuration = configuration;
            _timesheetContext = timesheetContext;
            _login = login;
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] Login loginModel)
        {
            var Email = await _timesheetContext.employees.FirstOrDefaultAsync(i => i.Official_Email == loginModel.Email);
            if (Email == null)
            {
                return NotFound("Email Not Found");
            }
            string Passwordpattern = "^(?=.*[A-Z])(?=.*[!@#$%^&*()_+\\-=\\[\\]{};':\"\\\\|,.<>\\/?])[A-Za-z0-9!@#$%^&*()_+\\-=\\[\\]{};':\"\\\\|,.<>\\/?]{8,}$";
            if (loginModel.Password == "" || !Regex.IsMatch(loginModel.Password, Passwordpattern))
            {
                return BadRequest("Wrong Password");
            }
            string storedPasswordHash = Email.Hashpassword;
            bool isPasswordCorrect = BCrypt.Net.BCrypt.Verify(loginModel.Password, storedPasswordHash);
            if (!isPasswordCorrect)
            {
                return NotFound("Password Not Found");
            }
            List<Claim> claims = new List<Claim>
            {

                        new Claim(ClaimTypes.Email, loginModel.Email)
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
                return BadRequest("User already exists");
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

            var Role = _timesheetContext.designations.FirstOrDefault(e => e.Designation_Id == registerModel.Designation_Id);
            if (Role.Designation.ToLower() == "hr" || Role.Designation.ToLower() == "human resource" || Role.Designation.ToLower() == " admin"
                  || Role.Designation.ToLower() == "hr manager" || Role.Designation.ToLower() == "hr admin")
            {
                emp.Role_Id = 1;
            }
            else
            {
                emp.Role_Id = 2;
            }

            emp.Joining_Date = registerModel.Joining_Date;
            emp.Is_Active = true;
            emp.Password = "Joyit@Admin@1234";
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(emp.Password);
            emp.Hashpassword = passwordHash;
            emp.Reporting_Manager1 = registerModel.Reporting_Manager1;
            _timesheetContext.employees.Add(emp);
            _timesheetContext.SaveChanges();
            return Ok("User Added Successfully..!");

        }

        [HttpPost]
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

            string passwordHash = BCrypt.Net.BCrypt.HashPassword(Cmodel.Password);
            bool isPasswordCorrect = BCrypt.Net.BCrypt.Verify(Data.Hashpassword, passwordHash);
            if (isPasswordCorrect)
            {
                return BadRequest("new password and ConfrimNewPassword should be match");
            }
            string Passwordpattern = "^(?=.*[A-Z])(?=.*[!@#$%^&*()_+\\-=\\[\\]{};':\"\\\\|,.<>\\/?])[A-Za-z0-9!@#$%^&*()_+\\-=\\[\\]{};':\"\\\\|,.<>\\/?]{8,}$";
            if (!Regex.IsMatch(Cmodel.NewPassword, Passwordpattern))
            {
                return BadRequest("Password should contain first letter should capital letter and one special symbol");
            }
            Data.Password = Cmodel.NewPassword;
            string passwordHash1 = BCrypt.Net.BCrypt.HashPassword(Data.Password);
            Data.Hashpassword = passwordHash1;
            _timesheetContext.SaveChanges();
            return Ok("Password Updated successFully..!");
        }
        [HttpGet]
        public IEnumerable<Employee> Get()
        {
            return _timesheetContext.employees.ToList();
        }

        //For reset password

        [HttpPost]
        [Route("GenerateOTP")]
        public IActionResult GeneratesOTP(string? email,string? PhoneNumber)
        {
            if(email==null&& PhoneNumber==null)
            {
                return Ok("Please enter Any one Informetion");
            }
            var emailCheck = _timesheetContext.employees.FirstOrDefault(e => e.Official_Email == email || e.Contact_No == PhoneNumber);
            // Generate a random 4-digit OTP
            Random random = new Random();
            string otp = (random.Next(1000, 9999)).ToString();
            if (email != "" || PhoneNumber != "")
            {
                if (emailCheck.Official_Email == null && emailCheck.Contact_No == null)
                {
                    return BadRequest("Please provid valied Mail Or Phone Number");
                }
                if (email == null && emailCheck.Contact_No != null)
                {
                    // Your Account SID and Auth Token from twilio.com/console
                    string accountSid = "AC6e58d36390a5ec00be0016b2d424e99fff";
                    string authToken = "c94e935541ed3f3b83f3712e1a48852eee";

                    // Initialize the Twilio client
                    TwilioClient.Init(accountSid, authToken);

                    // Send an SMS message
                    var message1 = MessageResource.Create(
                        body: $"Your otp is:{otp} Do not shere with any one...!",
                        from: new Twilio.Types.PhoneNumber("+12707173050"), // Twilio phone number
                        to: new Twilio.Types.PhoneNumber("+916363112696") // recipient's phone number
                    );
                    emailCheck.Otp = otp;
                    _timesheetContext.SaveChanges();
                    return Ok($"OTP generated and sent to Phone Number: '{PhoneNumber}'");
                }
                if (PhoneNumber == null)
                {
                    emailCheck.Otp = otp;
                    _timesheetContext.SaveChanges();

                    // Send the OTP to the user via email 
                    var fullname = emailCheck.First_Name + " " + emailCheck.Last_Name;
                    string fromAddress = "Joyitsolutions1@gmail.com";
                    string Password = "rpcfydphzeoafsig";
                    string toAddress = emailCheck.Official_Email;
                    string emailHeader = "<html><body><h1>OTP to Reset Password</h1></body></html>";
                    string emailFooter = $"<html><head><title>JoyItsolutions</title></head><body><p>Hi {fullname}, <br> This is the confidential email. Don't share your otp with anyone..!<br>  </p></body></html>";
                    string emailBody = $"<html><head><title>Don't replay this Mail</title></head><body><p>Your one time password(otp) is: <h3>{emailCheck.Otp}</h3></p></body></html>";
                    string emailContent = emailHeader + emailBody + emailFooter;
                    MailMessage message = new MailMessage();
                    message.From = new MailAddress(fromAddress);
                    message.Subject = "Reset Password";
                    message.To.Add(new MailAddress(toAddress));
                    message.Body = emailContent;
                    message.IsBodyHtml = true;

                    var smtpClient = new SmtpClient("smtp.gmail.com")
                    {
                        Port = 587,
                        Credentials = new NetworkCredential(fromAddress, Password),
                        EnableSsl = true,
                    };

                    smtpClient.Send(message);
                    return Ok($"OTP generated and sent to MailID: '{emailCheck.Official_Email}'");
                }
            }
            return Ok("OTP generated");
        }

        [HttpPost]
        [Route("VerifyOTP")]
        public IActionResult VerifyOTP(string? email,string? PhoneNumber,string otp, string newPassword)
        {
            var result = _login.ResetPassword(email, PhoneNumber, otp, newPassword);
            if (result is OkObjectResult)
            {
                return Ok("Password reset successful");
            }
            else
            {
                return result;
            }
        }

        //[HttpPut]
        //[Route("ForgetPassword")]
        //public IActionResult ResetPassword(ForgetPassword forgetPassword)
        //{
        //    var data = _timesheetContext.employees.FirstOrDefault(i => i.Official_Email == forgetPassword.Email);
        //    if (data == null)
        //    {
        //        return NotFound();
        //    }
        //    string Passwordpattern = "^(?=.*[A-Z])(?=.*[!@#$%^&*()_+\\-=\\[\\]{};':\"\\\\|,.<>\\/?])[A-Za-z0-9!@#$%^&*()_+\\-=\\[\\]{};':\"\\\\|,.<>\\/?]{8,}$";
        //    if (!Regex.IsMatch(forgetPassword.Password, Passwordpattern))
        //    {
        //        return BadRequest("Password should contain first letter should capital letter and one special symbol");
        //    }
        //    data.Password = forgetPassword.Password;
        //    string passwordHash = BCrypt.Net.BCrypt.HashPassword(forgetPassword.Password);
        //    data.Hashpassword = passwordHash;
        //    _timesheetContext.SaveChanges();
        //    return Ok("Password is reset");
        //}
    }
}
