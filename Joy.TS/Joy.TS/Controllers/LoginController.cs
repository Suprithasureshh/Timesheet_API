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
                return NotFound("Email Not Found");
            }
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(loginModel.Password);
            string Passwordpattern = "^(?=.*[A-Z])(?=.*[!@#$%^&*()_+\\-=\\[\\]{};':\"\\\\|,.<>\\/?])[A-Za-z0-9!@#$%^&*()_+\\-=\\[\\]{};':\"\\\\|,.<>\\/?]{8,}$";
            if (loginModel.Password == "" || !Regex.IsMatch(loginModel.Password, Passwordpattern))
            {
                return BadRequest("Wrong Password");
            }
            bool isPasswordCorrect = BCrypt.Net.BCrypt.Verify(Email.Hashpassword, passwordHash);
            //var Password = await _timesheetContext.employees.FirstOrDefaultAsync(i => i.Hashpassword == passwordHash);
            if (isPasswordCorrect)
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
            //var message = new MimeMessage();
            //message.From.Add(new MailboxAddress("Shivakumara S C", "shivukumarasc010@gmail.com"));
            //message.To.Add(new MailboxAddress("Shivakumara S C", "shivakumarasc010@gmail.com"));
            //message.Subject = "Test Email";
            //message.Body = new TextPart("plain")
            //{
            //    Text = "This is a test email."
            //};

            //// send message
            //using var client = new MailKit.Net.Smtp.SmtpClient();
            //client.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            //client.Authenticate("bharathkvpessmca@gmail.com", "vdkywjudsntfergv");
            //client.Send(message);
            //client.Disconnect(true);
            //string fromAddress = "bharathkvpessmca@gmail.com";
            //string toAddress = "shivukumarasc010@gmail.com";
            //string subject = "Test email";
            //string body = "This is a test email sent using C#.";

            //MailMessage message = new MailMessage(fromAddress, toAddress, subject, body);
            //System.Net.Mail.SmtpClient smtpClient = new System.Net.Mail.SmtpClient("smtp.gmail.com", 587);

            //smtpClient.UseDefaultCredentials = false;
            //smtpClient.EnableSsl = true;
            //smtpClient.Credentials = new NetworkCredential(fromAddress, "vdkywjudsntfergv");

            //smtpClient.Send(message);
            //string toAddress = "shivukumarasc010@gmail.com";
            //string fromAddress = "bharathkvpessmca@gmail.com";
            //string subject = "Test Email";
            //string body = "This is a test email sent using Gmail SMTP server.";

            //// Set Gmail SMTP server details
            //System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient();
            //smtp.Host = "smtp.gmail.com";
            //smtp.Port = 587;
            //smtp.EnableSsl = true;
            //smtp.UseDefaultCredentials = false;
            //smtp.Credentials = new NetworkCredential("bharathkvpessmca@gmail.com", "vdkywjudsntfergv");

            // Create and send email message
            //MailMessage message = new MailMessage(fromAddress, toAddress, subject, body);
            //smtp.Send(message);
            //string recipientEmail = registerModel.Official_Email;
            //string subject ="nothing";
            //string body ="no body";
            //var message = new MailMessage();
            //message.From = new MailAddress("shivukumarasc010@gmail.com");
            //message.To.Add(new MailAddress(recipientEmail));
            //message.Subject = subject;
            //message.Body = body;

            //var smtpClient = new System.Net.Mail.SmtpClient("smtp.gmail.com", 587);
            //smtpClient.UseDefaultCredentials = false;
            //smtpClient.Credentials = new NetworkCredential("bharathkvpessmca@gmail.com", "Bharath@123");

            //smtpClient.EnableSsl = true;

            //smtpClient.Send(message);

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
            emp.Role_Id = registerModel.role_id;
            emp.Joining_Date = registerModel.Joining_Date;
            emp.Is_Active = true;
            emp.Password = "Joyit@Admin@1234";
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(emp.Password);
            emp.Hashpassword = passwordHash;
            emp.Reporting_Manager1 = registerModel.Reporting_Manager1;
            _timesheetContext.employees.Add(emp);
            _timesheetContext.SaveChanges();


            //string senderEmail = "bharathkvpessmca@gmail.com";
            //string appPassword = "Bharath@123";

            //// Define the recipient's email address
            //string recipientEmail = registerModel.Official_Email; 

            //// Create a new email message
            //MailMessage message = new MailMessage(senderEmail, recipientEmail);
            //message.Subject = "Employee Added";
            //message.Body = "Dear Employee, \n\n You have been successfully added to our system. \n\n Thank you.";

            //// Create a new SMTP client
            //SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
            //smtpClient.UseDefaultCredentials = false;
            //smtpClient.Credentials = new NetworkCredential(senderEmail, appPassword);
            //smtpClient.EnableSsl = true;

            //// Send the email message
            //smtpClient.Send(message);

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

        [HttpPut]
        [Route("ForgetPassword")]
        public IActionResult ResetPassword(ForgetPassword forgetPassword)
        {
            var data = _timesheetContext.employees.FirstOrDefault(i => i.Official_Email == forgetPassword.Email);
            if (data == null)
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
