using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TimeSheet.Model
{
    public class EmployeeType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Employee_Type_Id { get; set; }

        [Required(ErrorMessage = "Employee type should not be empty")]
        public string Employee_Type { get; set; }

        public bool Is_Active { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/DD/YYYY}")]
        public DateTime Create_Date { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/DD/YYYY}")]
        public DateTime? Modified_Date { get; set; }
    }
}
