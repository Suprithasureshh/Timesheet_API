using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Joy.TS.BAL.DomainModel
{
    public class EmployeeDomainModel
    {
        public class GetDashboardModel
        {
            public string Status { get; set; }
            public string Month { get; set; }
            public int Year { get; set; }
        }
        public class TimesheetsummaryModel
        {
            public int TimesheetSummary_Id { get; set; }
            public int Employee_Id { get; set; }
            public int Fiscal_Year_ID { get; set; }
            public string Month { get; set; }
            public int Year { get; set; }
            public double NoOfdays_Worked { get; set; }
            public double NoOfLeave_Taken { get; set; }
            public double Total_Working_Hours { get; set; }
            public string Status { get; set; }
            public string ImagePathUpload { get; set; }
            public string ImagePathTimesheet { get; set; }
        }
        //forgetPassword
        public class ForgetPassword
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }
        // AddTimesheet
        public class AddTimeSheet_SummaryModel
        {
            public int Employee_Id { get; set; }
            public double NoOfdays_Worked { get; set; }
            public double NoOfLeave_Taken { get; set; }
            public double Total_Working_Hours { get; set; }
            public string? ImagePathUpload { get; set; }
            public string? ImagePathTimesheet { get; set; }
            public List<AddTimesheetDayModel> addTimesheetDay { get; set; }

        }
        public class AddTimesheetDayModel
        {
            [ForeignKey("Employee_Id")]
            public int Employee_Id { get; set; }
            public int Project_Id { get; set; }
            [DataType(DataType.Date)]
            [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/DD/YYYY}")]
            public Nullable<DateTime> Date { get; set; }
            public string Day { get; set; }
            public bool Leave { get; set; }
            public int Duration_in_Hrs { get; set; }
        }
        //Get time sheet
        public class GetTimeSheetByIdModel
        {
            [ForeignKey("Employee_Id")]
            public int Employee_Id { get; set; }
            [ForeignKey("Fiscal_Year_ID")]
            public int Fiscal_Year_ID { get; set; }

            [DataType(DataType.Date)]
            [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/DD/YYYY}")]
            public int Year { get; set; }
            public double NoOfdays_Worked { get; set; }
            public double NoOfLeave_Taken { get; set; }
            public double Total_Working_Hours { get; set; }
            [DataType(DataType.Date)]
            [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/DD/YYYY}")]
            public Nullable<DateTime> Date { get; set; }

            [DataType(DataType.Date)]
            [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/DD/YYYY}")]
            public string Day { get; set; }

            public bool Leave { get; set; }


            [ForeignKey("Project_Id")]
            public int Project_Id { get; set; }

        }
        //getUse Data
        public class UserProfileModel
        {
            public int Employee_ID { get; set; }
            public string Employee_Name { get; set; }

            public string Designation { get; set; }
            public string Joining_Date { get; set; }
            public string Email { get; set; }
            public string Mobile_No { get; set; }
        }
        //Export to excel
        public class ExcelTimeSheetModel
        {
            public string Day { get; set; }
            public string Duration_in_Hrs { get; set; }
            public int Year { get; set; }
            public string Location { get; set; }
            public string NoOfdays_Worked { get; set; }
            public string NoOfLeave_Taken { get; set; }
            public string Total_Working_Hours { get; set; }
            public string Date { get; set; }
            public string Reporting_Manager1 { get; set; }
            public int TimesheetSummary_Id { get; set; }
            public int Employee_Id { get; set; }
            public bool Leave { get; set; }
            public string Employee_Name { get; set; }
            public string Project_Name { get; set; }
            public string Month { get; set; }
            public string status { get; set; }
            public string Designation_Name { get; set; }

        }

        public class GetAllProjectsModel
        {
            public int Project_Id { get; set; }
            public string Project_Name { get; set; }
            public string Project_Code { get; set; }
            public int Client_Id { get; set; }
            public string Start_Date { get; set; }
            public string End_Date { get; set; }

        }
        public class ChangePasswordModel
        {
            [Required(ErrorMessage = "user name is Required")]
            public string Email { get; set; }
            [Required(ErrorMessage = " Current password is Required")]
            public string Password { get; set; }
            [Required(ErrorMessage = "new password is Required")]
            public string NewPassword { get; set; }
            [Required(ErrorMessage = "Confrim NewPassword is Required")]
            public string ConfrimNewPassword { get; set; }
        }
    }
}
