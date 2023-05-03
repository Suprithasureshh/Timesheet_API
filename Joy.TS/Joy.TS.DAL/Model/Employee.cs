using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Joy.TS.DAL.Model
{
    public class Employee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Employee_Id { get; set; }

        [Required(ErrorMessage = "First name should not be empty")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "First Name should contains only alphabets.")]
        public string First_Name { get; set; }

        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Last Name should contains only alphabets.")]
        public string Last_Name { get; set; }

        [Required(ErrorMessage = "Employee Code is required")]
        public string Employee_code { get; set; }

        public string Reporting_Manager1 { get; set; }

        [ForeignKey("Employee_Type_Id")]
        public int Employee_Type_Id { get; set; }

        [Required(ErrorMessage = "Official_Email field is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Official_Email { get; set; }

        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string? Alternate_Email { get; set; }

        [ForeignKey("Client_Id")]
        public int Client_Id { get; set; }

        [ForeignKey("Project_Id")]
        public int Project_Id { get; set; }

        [ForeignKey("Designation_Id")]
        public int Designation_Id { get; set; }

        [ForeignKey("Role_Id")]
        public int Role_Id { get; set; }

        [Required(ErrorMessage = "Contact_No is required")]
        [RegularExpression(@"^\+(?:[0-9] ?){6,14}[0-9]$", ErrorMessage = "Contact allows only digits.")]
        public string Contact_No { get; set; }

        public bool Is_Active { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/DD/YYYY}")]
        public DateTime Joining_Date { get; set; }

        [Required(ErrorMessage = "Password field is required")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$",
        ErrorMessage = "Password should contain atleast one lowercase,uppercase,number and a symbol")]
        public string Password { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/DD/YYYY}")]
        public DateTime Create_Date { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/DD/YYYY}")]
        public DateTime? End_Date { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/DD/YYYY}")]
        public DateTime? Modified_Date { get; set; }
    }
}
