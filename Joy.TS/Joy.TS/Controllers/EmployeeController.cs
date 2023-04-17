using Joy.TS.BAL.Implementation;
using Joy.TS.DAL.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Joy.TS.BAL.DomainModel.EmployeeDomainModel;

namespace Joy.TS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {

        public EmployeeController(EmployeeInterface _employeeInterface)
        {
            employeeInterface = _employeeInterface;
        }
        public EmployeeInterface employeeInterface { get; set; }




        [HttpPost, Authorize]
        [Route("AddTimesheet")]
        public string AddTimeSheet_Summary(AddTimeSheet_SummaryModel AddTimeSheet_SummaryModel)
        {
            return employeeInterface.AddTimeSheet_Summary(AddTimeSheet_SummaryModel);
        }



        [HttpPost, Authorize]
        [Route("Image")]
        public async Task<IActionResult> UploadImage(IFormFile image)
        {
            return await employeeInterface.UploadImage(image);
        }

        [HttpGet, Authorize]
        [Route("ViewTimeSheet")]
        public List<TimesheetsummaryModel> GetAllTimeSheet_Summary(int Employee_Id, int year)
        {
            return employeeInterface.GetAllTimeSheet_Summary(Employee_Id, year);
        }



        [HttpGet, Authorize]
        [Route("ViewTimeSheetById")]
        public IEnumerable<GetTimeSheetByIdModel> GetTimeSheetById(int id)
        {
            return employeeInterface.GetTimeSheetById(id);
        }



        [HttpGet, Authorize]
        [Route("UserProfile")]
        public IEnumerable<UserProfileModel> GetUserProfileMail(string mail_id)
        {
            return employeeInterface.GetUserProfile(mail_id);
        }

        [HttpGet, Authorize]
        [Route("GetByDashboard")]
        public IActionResult GetByDashboard(int Employee_Id)
        {
            var item = employeeInterface.GetByDashboard(Employee_Id);
            return new ObjectResult(item);
        }

        [HttpGet,Authorize]
        [Route("ExportExcel")]
        public string ExportExcel(int id, int monthid, int year, int project_id)
        {
            return employeeInterface.ExportExcel(id, monthid, year, project_id);
        }
        [HttpGet, Authorize]
        [Route("ImagePath")]
        public IActionResult GetImage(string imagePath)
        {
            return employeeInterface.GetImage(imagePath);
        }
        [HttpPost, Authorize]
        [Route("Fiscal_Year")]
        public IActionResult Fiscal_Years(Fiscal_Year fiscal_Year)
        {
            return employeeInterface.Fiscal_Years(fiscal_Year);
        }

    }
}
