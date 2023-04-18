using CsvHelper;
using DocumentFormat.OpenXml.Office2010.Excel;
using Joy.TS.BAL.Implementation;
using Joy.TS.DAL.Data;
using Joy.TS.DAL.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Formats.Asn1;
using System.Globalization;
using System.Text;
using static Joy.TS.BAL.DomainModel.EmployeeDomainModel;

namespace Joy.TS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly TimeSheetContext _timesheetContext;

        public EmployeeController(EmployeeInterface _employeeInterface)
        {
            employeeInterface = _employeeInterface;
        }
        public EmployeeInterface employeeInterface { get; set; }




        [HttpPost] //, Authorize
        [Route("AddTimesheet")]
        public string AddTimeSheet_Summary(AddTimeSheet_SummaryModel AddTimeSheet_SummaryModel)
        {
            return employeeInterface.AddTimeSheet_Summary(AddTimeSheet_SummaryModel);
        }



        [HttpPost] //, Authorize
        [Route("Image")]
        public async Task<IActionResult> UploadImage(IFormFile image)
        {
            return await employeeInterface.UploadImage(image);
        }

        [HttpGet]
        [Route("ViewTimeSheet")]
        public List<TimesheetsummaryModel> GetAllTimeSheet_Summary(int Employee_Id, int year)
        {
            return employeeInterface.GetAllTimeSheet_Summary(Employee_Id, year);
        }



        [HttpGet]
        [Route("ViewTimeSheetById")]
        public IEnumerable<GetTimeSheetByIdModel> GetTimeSheetById(int id)
        {
            return employeeInterface.GetTimeSheetById(id);
        }



        [HttpGet]
        [Route("UserProfile")]
        public IEnumerable<UserProfileModel> GetUserProfileMail(string mail_id)
        {
            return employeeInterface.GetUserProfile(mail_id);
        }

        [HttpGet]
        [Route("GetByDashboard")]
        public IActionResult GetByDashboard(int Employee_Id)
        {
            var item = employeeInterface.GetByDashboard(Employee_Id);
            return new ObjectResult(item);
        }

        [HttpGet]
        [Route("ExportExcel")]
        public string ExportExcel(int id, int monthid, int year, int project_id)
        {
            return employeeInterface.ExportExcel(id, monthid, year, project_id);
        }
        [HttpGet]
        [Route("ImagePath")]
        public IActionResult GetImage(string imagePath)
        {
            return employeeInterface.GetImage(imagePath);
        }
        [HttpPost]
        [Route("Fiscal_Year")]
        public IActionResult Fiscal_Years(Fiscal_Year fiscal_Year)
        {
            return employeeInterface.Fiscal_Years(fiscal_Year);
        }

        [HttpGet("TExport the data")]
        public IActionResult ExportTimesheetSumaries(int id)
        {
            var tssummaries = employeeInterface.GetTimeSheetById(id);
            using (var writer = new StringWriter())
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(tssummaries);
                var content = writer.ToString();
                var bytes = Encoding.UTF8.GetBytes(content);
                var result = new FileContentResult(bytes, "text/csv")
                {
                    FileDownloadName = "TimeSheetDetails.csv"
                }; return result;
            }
        }

    }
}
