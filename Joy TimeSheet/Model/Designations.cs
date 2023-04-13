using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TimeSheet.Model
{
    public class Designations
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Designation_Id { get; set; }

        [Required(ErrorMessage = "Designation should not be empty")]
        public string Designation { get; set; }

        public bool Is_Active { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/DD/YYYY}")]
        public DateTime Create_Date { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/DD/YYYY}")]
        public DateTime? Modified_Date { get; set; }
    }
}
