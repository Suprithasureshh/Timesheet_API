using Joy.TS.DAL.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Joy.TS.BAL.Implementation
{
    public class LoginRepo : ControllerBase, ILogin
    {

        private readonly TimeSheetContext _timesheetContext;

        public LoginRepo(TimeSheetContext timesheetContext)
        {
            _timesheetContext = timesheetContext;
        }


        public IActionResult ResetPassword(string email, string otp, string newPassword)
        {
            var data = _timesheetContext.employees.FirstOrDefault(i => i.Official_Email == email && i.Otp == otp);
            if (data == null)
            {
                return BadRequest("Invalid OTP");
            }

            string Passwordpattern = "^(?=.*[A-Z])(?=.*[!@#$%^&*()_+\\-=\\[\\]{};':\"\\\\|,.<>\\/?])[A-Za-z0-9!@#$%^&*()_+\\-=\\[\\]{};':\"\\\\|,.<>\\/?]{8,}$";
            if (!Regex.IsMatch(newPassword, Passwordpattern))
            {
                return BadRequest("Password should contain first letter should capital letter and one special symbol");
            }

            data.Password = newPassword;
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
            data.Hashpassword = passwordHash;
            data.Otp = ""; // clear the OTP
            _timesheetContext.SaveChanges();
            return Ok("Password is reset");
        }

    }
}
