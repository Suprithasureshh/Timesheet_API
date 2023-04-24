using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Joy.TS.BAL.DomainModel
{
    public class AdminDomainModel
    {

        //dashboard
        public class GetDashboardModel
        {
            public string x { get; set; }
            public int y { get; set; }

        }

        //ClientModel
        public class AddClientModel
        {
            public string Client_Name { get; set; }
        }
        public class EditClientModel
        {
            public int Client_Id { get; set; }
            public string Client_Name { get; set; }
        }
        public class GetAllClientsByEmployeeModel
        {
            public int Client_Id { get; set; }
            public string Client_Name { get; set; }
            public int No_Of_Employees { get; set; }
        }

        //Projectmodel

        public class AddProjectsModel
        {
            public string Project_Name { get; set; }
            public string Project_Code { get; set; }
            public int Client_Id { get; set; }

            [DataType(DataType.Date)]
            [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/DD/YYYY}")]
            public DateTime Project_Start_Date { get; set; }

            [DataType(DataType.Date)]
            [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/DD/YYYY}")]
            public DateTime Project_End_Date { get; set; }
        }

        public class EditProjectsModel
        {
            public int Project_Id { get; set; }
            public string Project_Name { get; set; }
            public int Client_Id { get; set; }

            [DataType(DataType.Date)]
            [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/DD/YYYY}")]
            public DateTime Start_Date { get; set; }

            [DataType(DataType.Date)]
            [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/DD/YYYY}")]
            public DateTime End_Date { get; set; }
        }

        public class GetAllProjectsByEmployeeModel
        {
            public int Project_Id { get; set; }
            public string Project_Name { get; set; }
            public string Project_Code { get; set; }
            public int No_Of_Employees { get; set; }

            [DataType(DataType.Date)]
            [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/DD/YYYY}")]
            public DateTime Start_Date { get; set; }

            [DataType(DataType.Date)]
            [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/DD/YYYY}")]
            public DateTime End_Date { get; set; }
        }



        //DesignationModel

        public class PostDesignationModel
        {
            public string Designation { get; set; }
        }
        public class EditDesignationModel
        {
            public int Designation_Id { get; set; }
            public string Designation { get; set; }
        }
        public class GetAllDesignationsByEmployeeModel
        {
            public int Designation_Id { get; set; }
            public string Designation { get; set; }
            public int No_of_Employees { get; set; }
        }

        //EmployeeType
        public class PostEmployeeTypeModel
        {
            public string Employee_Type { get; set; }
        }
        public class EditEmployeeTypeModel
        {
            public int Employee_Type_Id { get; set; }
            public string Employee_Type { get; set; }
        }
        public class GetAllEmployeeTypeByEmployeeModel
        {
            public int Employee_Type_Id { get; set; }
            public string Employee_Type { get; set; }
            public int No_of_Employees { get; set; }
        }

        //RoleModels

        public class AddRoleModel
        {
            public string Role { get; set; }
        }
        public class EditRoleModel
        {
            public int Role_Id { get; set; }
            public string Role { get; set; }
        }

        //EmployeeModels

        public class AddEmployeeModel
        {
            public string First_Name { get; set; }
            public string Last_Name { get; set; }
            public string Employee_code { get; set; }
            public string Reporting_Manager1 { get; set; }
            public string Reportinng_Manager2 { get; set; }
            public int Employee_Type_Id { get; set; }

            [EmailAddress(ErrorMessage = "Invalid Email Address")]
            public string Official_Email { get; set; }

            [EmailAddress(ErrorMessage = "Invalid Email Address")]
            public string Alternate_Email { get; set; }
            public string Password { get; set; }
            public int Designation_Id { get; set; }
            public string Contact_No { get; set; }

            [DataType(DataType.Date)]
            [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/DD/YYYY}")]
            public DateTime Joining_Date { get; set; }
            public DateTime? End_Date { get; set; }
            public int role_id { get; set; }    
        }

        public class EditEmployeeModel
        {
            public int Employee_Id { get; set; }
            public string First_Name { get; set; }
            public string Last_Name { get; set; }
            public string Employee_code { get; set; }
            public string Reporting_Manager1 { get; set; }
            public string Reportinng_Manager2 { get; set; }
            public int Employee_Type_Id { get; set; }
            public int Role_id { get; set; }

            [Required(ErrorMessage = "Official_Email field is required")]
            [EmailAddress(ErrorMessage = "Invalid Email Address")]
            public string Official_Email { get; set; }

            [EmailAddress(ErrorMessage = "Invalid Email Address")]
            public string Alternate_Email { get; set; }
            public int Designation_Id { get; set; }
            public string Contact_No { get; set; }

            [DataType(DataType.Date)]
            [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/DD/YYYY}")]
            public DateTime Joining_Date { get; set; }

            [DataType(DataType.Date)]
            [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/DD/YYYY}")]
            public DateTime End_Date { get; set; }
        }

        public class GetAllEmployeeByDesIdEmpTypeIdModel
        {
            public int Employee_Id { get; set; }
            public string First_Name { get; set; }
            public string last_Name { get; set; }
            public string Full_Name { get; set; }
            public string Employee_code { get; set; }
            public string Reporting_Manager1 { get; set; }
            public string Reportinng_Manager2 { get; set; }
            public int Employee_Type_Id { get; set; }
            public string Employee_Type { get; set; }
            public string Official_Email { get; set; }
            public string Alternate_Email { get; set; }
            public int Role_Id { get; set; }
            public int Designation_Id { get; set; }
            public string Designation { get; set; }
            public string Contact_No { get; set; }
            public DateTime Joining_Date { get; set; }
            public DateTime End_Date { get; set; }
        }

        //EmployeeProject

        public class AddEmployeeProjectModel
        {
            [ForeignKey("Employee_Id")]
            public int Employee_Id { get; set; }

            [ForeignKey("Project_Id")]
            public int Project_Id { get; set; }

            [DataType(DataType.Date)]
            [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/DD/YYYY}")]
            public DateTime Start_Date { get; set; }

            [DataType(DataType.Date)]
            [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/DD/YYYY}")]
            public DateTime? End_Date { get; set; }
            public string Location { get; set; }

        }

        public class EditEmployeeprojectModel
        {
            public int Employee_Project_Id { get; set; }

            [ForeignKey("Employee_Id")]
            public int Employee_Id { get; set; }

            [ForeignKey("Project_Id")]
            public int Project_Id { get; set; }

            [DataType(DataType.Date)]
            [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/DD/YYYY}")]
            public DateTime Start_Date { get; set; }

            [DataType(DataType.Date)]
            [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/DD/YYYY}")]
            public Nullable<DateTime> End_Date { get; set; }
            public string Location { get; set; }

        }

        public class GetEmployeeProjectsByIdModel
        {
            public int Employee_Project_Id { get; set; }
            public string Employee_Name { get; set; }
            public string Project_Name { get; set; }
            public string Start_Date { get; set; }
            public string End_Date { get; set; }
            public string Location { get; set; }
        }

        public class GetAllEmployeeProjects
        {
            public int Employee_Project_Id { get; set; }
            public string First_Name { get; set; }
            public string Last_Name { get; set; }
            public string Project_Name { get; set; }
            public string Start_Date { get; set; }
            public string End_Date { get; set; }
            public string Location { get; set; }
        }

        //HrContactInformation

        public class AddHrContactModel
        {
            public string Hr_Email_Id { get; set; }
        }

        public class EditHrContactModel
        {
            public string First_Name { get; set; }
            public string Last_Name { get; set; }
            public int Hr_Contact_Id { get; set; }
            public string Hr_Email_Id { get; set; }
            public string Hr_Contact_No { get; set; }
        }
        public class HrcontactByEmployeeModel
        {
            public int Hr_Contact_Id { get; set; }
            public string Name { get; set; }
            public string Designation { get; set; }
            public string Joining_Date { get; set; }
            public string Email { get; set; }
            public string Mobile_No { get; set; }
        }

        //EditIsActive
        public class IsActiveModel
        {
            public int[] Id { get; set; }
        }

        //GetIsActive

        public class ClinetIsActiveModel
        {
            public int Client_Id { get; set; }
            public string Client_Name { get; set; }
            public bool Is_Active { get; set; }
            public int No_Of_Employees { get; set; }

            [DataType(DataType.Date)]
            [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/DD/YYYY}")]
            public DateTime Create_Date { get; set; }


        }

        public class DesignationIsActiveModel
        {
            public int Designation_Id { get; set; }
            public string Designation { get; set; }
            public bool Is_Active { get; set; }
            [DataType(DataType.Date)]
            [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/DD/YYYY}")]

            public int No_of_Employees { get; set; }

        }
        public class ProjectIsActiveModel
        {
            public int Project_Id { get; set; }
            public string Project_Name { get; set; }
            public string Project_Code { get; set; }
            public int Client_Id { get; set; }
            public int No_Of_Employees { get; set; }
            public bool Is_Active { get; set; }
            [DataType(DataType.Date)]
            [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/DD/YYYY}")]
            public DateTime Project_Start_Date { get; set; }
            [DataType(DataType.Date)]
            [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/DD/YYYY}")]
            public DateTime Project_End_Date { get; set; }




        }
        public class EmployeeTypeIsActiveModel
        {
            public int Employee_Type_Id { get; set; }
            public string Employee_Type { get; set; }
            public int No_of_Employees { get; set; }
            public bool Is_Active { get; set; }

        }
        public class EmployeeIsActiveModel
        {
            public int Employee_Id { get; set; }
            public string First_Name { get; set; }
            public string Last_Name { get; set; }
            public string Full_Name { get; set; }
            public string Employee_code { get; set; }
            public string Reporting_Manager1 { get; set; }
            public string Reportinng_Manager2 { get; set; }
            public string Employee_Type { get; set; }
            public string Email { get; set; }
            public int Role_Id { get; set; }
            public string Designation { get; set; }
            public string Contact_No { get; set; }
            public DateTime Joining_Date { get; set; }
            public DateTime End_Date { get; set; }
            public bool Is_Active { get; set; }

        }

        public class HrContactInfoIsActiveModel
        {
            public int Hr_Contact_Id { get; set; }
            public string Hr_Name { get; set; }
            public string Hr_Email_Id { get; set; }
            public string Hr_Contact_No { get; set; }
            public bool Is_Active { get; set; }




        }

        //Timesheet status

        public class GetTimeSheetStatusModel
        {
            public int Year { get; set; }
        }

        public class TimeSheetStatusByYearModel
        {
            public int MonthID { get; set; }
            public string Month { get; set; }
            public string status { get; set; }
            public int Statuscount { get; set; }
        }

        public class EmployeeTimeSheetByMonthModel
        {
            public int Employee_Id { get; set; }
            public string Full_Name { get; set; }
            public string Employee_Type { get; set; }
            public double NoOfDaysWorked { get; set; }
            public double NoOfLeaveTaken { get; set; }
            public double Total_Hours { get; set; }
            public string EmailId { get; set; }
            public string Reporting_Manager { get; set; }
            public string Status { get; set; }
        }

        public class GetTimesheetSummaryMonthYearEmployeeModel
        {
            public string Date { get; set; }
            public string Day { get; set; }
            public string Status { get; set; }
            public string Project { get; set; }
            public string Duration { get; set; }
        }

        public class EditTimeSheetStatusModel
        {

            public string Timesheet_Status { get; set; }
            public List<EditTimeSheetModelById> EditTimeSheetModelById { get; set; }
        }
        public class EditTimeSheetModelById
        {

            public int Employee_id { get; set; }
            public int Month_Id { get; set; }
            public int Year { get; set; }
        }



        //View Previous changes
        public class ViewPreviousChangesModel
        {
            public int Employee_Id { get; set; }
            public string Full_Name { get; set; }
            public string Employee_Type { get; set; }
            public string Joining_Date { get; set; }
            public string Modified_Date { get; set; }
            public string Designation { get; set; }
            public string Reporting_Manager { get; set; }
            public string EmailId { get; set; }
            public string Contact_No { get; set; }
        }
        public class ViewPreviousChangesByIdModel
        {
            public int Employee_Id { get; set; }
            public string Full_Name { get; set; }
            public string Employee_Type { get; set; }
            public string Designation { get; set; }

            public string Reporting_Manager1 { get; set; }
            public string EmailId { get; set; }
            public string Contact_No { get; set; }

            public DateTime Joining_Date { get; set; }
            public DateTime End_Date { get; set; }
            public DateTime Modified_Date { get; set; }

        }


        //Excel
        public class ExcelEditTimesheetStatusByMonthModel
        {
            public int EmployeeId { get; set; }
            public string EmployeeName { get; set; }
            public string EmployeeType { get; set; }
            public double NoOfDaysWorked { get; set; }
            public string Month { get; set; }
            public int Year { get; set; }
            public double NoOfLeaveTaken { get; set; }
            public double TotalHours { get; set; }
            public string EmailId { get; set; }
            public string ReportingManager { get; set; }
            public string Status { get; set; }

        }
        public class ImageUpdate
        {
            public string ImagePathUpload { get; set; }
            public string ImagePathTimesheet { get; set; }
        }

    }
}
