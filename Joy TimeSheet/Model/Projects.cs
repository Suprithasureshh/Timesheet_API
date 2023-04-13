using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TimeSheet.Model
{
    public class Projects
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Project_Id { get; set; }

        [Required(ErrorMessage = "Project name should not be empty")]
        public string Project_Name { get; set; }

        [Required(ErrorMessage = "Project code should not be empty")]
        public string Project_Code { get; set; }

        [ForeignKey("Client_Id")]
        public int Client_Id { get; set; }

        public bool Is_Active { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/DD/YYYY}")]
        public DateTime Project_Start_Date { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/DD/YYYY}")]
        public DateTime Project_End_Date { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/DD/YYYY}")]
        public DateTime Create_Date { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/DD/YYYY}")]
        public DateTime? Modified_Date { get; set; }
    }
}
