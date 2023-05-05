using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Joy.TS.BAL.Implementation
{
    public interface ILogin
    {
        IActionResult ResetPassword(string email, string otp, string newPassword);
    }
}
