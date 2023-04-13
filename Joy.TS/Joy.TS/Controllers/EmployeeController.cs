using Joy.TS.BAL.Implementation;
using Joy.TS.DAL.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Joy.TS.BAL.DomainModel.EmployeeDomainModel;

namespace Joy.TS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        // private readonly EmployeeInterface employeeInterface;
        public EmployeeController(EmployeeInterface _employeeInterface)
        {
            employeeInterface = _employeeInterface;
        }
        public EmployeeInterface employeeInterface { get; set; }
        [HttpPost("AddTimeSheet_SummaryModel")]
        public string AddTimeSheet_Summary(AddTimeSheet_SummaryModel AddTimeSheet_SummaryModel)
        {
            return employeeInterface.AddTimeSheet_Summary(AddTimeSheet_SummaryModel);
        }
        [HttpGet("id")]
        public IEnumerable<GetTimeSheetByIdModel> GetTimeSheetById(int id)
        {
            return employeeInterface.GetTimeSheetById(id);
        }
        //[HttpGet]
        //public IEnumerable<TimeSheet> GetTimesheets()
        //{
        //    return employeeInterface.GetTimesheets();
        //}
        [HttpPost("project")]
        public IActionResult project(Projects projects)
        {
            return employeeInterface.project(projects);
        }

        //Dashboard

        [HttpGet]
        public IActionResult GetByDashboard(int Employee_Id)
        {
            var item = employeeInterface.GetByDashboard(Employee_Id);
            return new ObjectResult(item);
        }
    }
}
