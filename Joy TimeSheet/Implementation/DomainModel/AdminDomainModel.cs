using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeSheet.Implementation.DomainModel
{
    public class AdminDomainModel
    {
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

        //EmployeeModels

        public class AddEmployeeModel
        {
            public string First_Name { get; set; }
            public string Last_Name { get; set; }
            public string Employee_code { get; set; }
            public string Reporting_Manager1 { get; set; }
            public string Reportinng_Manager2 { get; set; }
            public int Employee_Type_Id { get; set; }
            public int Role_id { get; set; }
            public string Official_Email { get; set; }
            public string Alternate_Email { get; set; }
            public string Password { get; set; }
            public int Designation_Id { get; set; }
            public string Contact_No { get; set; }

            [DataType(DataType.Date)]
            [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/DD/YYYY}")]
            public DateTime Joining_Date { get; set; }
            public DateTime? End_Date { get; set; }
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
            public string Official_Email { get; set; }
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
            public string Employee_code { get; set; }
            public string Reporting_Manager1 { get; set; }
            public string Reportinng_Manager2 { get; set; }
            public int Employee_Type_Id { get; set; }
            public string Employee_Type_Name { get; set; }
            public string Official_Email { get; set; }
            public string Alternate_Email { get; set; }
            public int Role_Id { get; set; }
            public int Designation_Id { get; set; }
            public string Designation_Name { get; set; }
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

            [DataType(DataType.Date)]
            [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/DD/YYYY}")]
            public DateTime Create_Date { get; set; }
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















    }
}
