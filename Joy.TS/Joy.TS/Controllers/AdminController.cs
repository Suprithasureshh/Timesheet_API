using Joy.TS.BAL.Implementation;
using Joy.TS.DAL.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Joy.TS.BAL.DomainModel.AdminDomainModel;

namespace Joy.TS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [CustomExceptionFilter]
    public class AdminController : ControllerBase
    {
        private readonly IAdmin _admin;
        public AdminController(IAdmin admin)
        {
            _admin = admin;
        }

        //Dashboard

        [HttpGet("GetDashboard")]

        public IEnumerable<GetDashboardModel> GetDashboard(int year, int Month_Id)
        {
            return _admin.GetDashboard(year, Month_Id);
        }

        //Clients

        [HttpPost("AddClient")]
        public void AddClient(AddClientModel model)
        {
            _admin.AddClient(model);
        }

        [HttpPut("EditClient")]
        public void EditClient(EditClientModel editClientModel)
        {
            _admin.EditClient(editClientModel);
        }

        [HttpPut("EditClientIsActive")]
        public void EditClientIsActive(IsActiveModel ClientIsActiveModel)
        {
            _admin.EditClientIsActive(ClientIsActiveModel);
        }

        [HttpGet("GetByClientId")]
        public IQueryable<Client> GetByClientId(int id)
        {
            return _admin.GetByClientId(id);
        }

        [HttpGet("GetAllClientsByEmployee")]
        public IEnumerable<GetAllClientsByEmployeeModel> GetAllClientsByEmployee()
        {
            return _admin.GetAllClientsByEmployee();
        }

        [HttpGet("GetAllClients")]
        public IQueryable<Client> GetAllClients()
        {
            return _admin.GetAllClients();
        }

        //Project

        [HttpPost("AddProject")]
        public void AddProject(AddProjectsModel addProjectsModel)
        {
            _admin.AddProject(addProjectsModel);
        }

        [HttpPut("EditProject")]
        public void EditProject(EditProjectsModel editProjectsModel)
        {
            _admin.EditProject(editProjectsModel);
        }

        [HttpPut("EditProjectIsActive")]
        public void EditProjectIsActive(IsActiveModel ProjectIsActiveModel)
        {
            _admin.EditProjectIsActive(ProjectIsActiveModel);
        }

        [HttpGet("GetByProjectId")]
        public IQueryable<Projects> GetByProjectId(int id)
        {
            return _admin.GetByProjectId(id);
        }

        [HttpGet("GetAllProjectsByEmployee")]
        public IEnumerable<GetAllProjectsByEmployeeModel> GetAllProjectsByEmployee()
        {
            return _admin.GetAllProjectsByEmployee();
        }

        [HttpGet("GetAllProjects")]
        public IQueryable<Projects> GetAllProjects()
        {
            return _admin.GetAllProjects();
        }

        //Designation

        [HttpPost("AddDesignation")]
        public void AddDesignation(PostDesignationModel postDesignationModel)
        {
            _admin.AddDesignation(postDesignationModel);
        }

        [HttpPut("EditDesignation")]
        public void EditDesignation(EditDesignationModel editDesignationModel)
        {
            _admin.EditDesignation(editDesignationModel);
        }

        [HttpPut("EditDesignationIsActive")]
        public void EditDesignationIsActive(IsActiveModel DesignationIsActiveModel)
        {
            _admin.EditDesignationIsActive(DesignationIsActiveModel);
        }

        [HttpGet("GetByDesignationId")]
        public IQueryable<Designations> GetByDesignationId(int id)
        {
            return _admin.GetByDesignationId(id);
        }

        [HttpGet("GetAllDesignationsByEmployee")]
        public IEnumerable<GetAllDesignationsByEmployeeModel> GetAllDesignationsByEmployee()
        {
            return _admin.GetAllDesignationsByEmployee();
        }

        [HttpGet("GetAllDesignations")]
        public IQueryable<Designations> GetAllDesignations()
        {
            return _admin.GetAllDesignations();
        }

        //EmployeeType

        [HttpPost("AddEmployeeType")]
        public void AddEmployeeType(PostEmployeeTypeModel postEmployeeTypeModel)
        {
            _admin.AddEmployeeType(postEmployeeTypeModel);
        }

        [HttpPut("EditEmployeetype")]
        public void EditEmployeetype(EditEmployeeTypeModel editEmployeeTypeModel)
        {
            _admin.EditEmployeetype(editEmployeeTypeModel);
        }

        [HttpPut("EditEmployeeTypeIsActive")]
        public void EditEmployeeTypeIsActive(IsActiveModel EmployeeTypeIsActiveModel)
        {
            _admin.EditEmployeeTypeIsActive(EmployeeTypeIsActiveModel);
        }

        [HttpGet("GetByEmployeeTypeId")]
        public IQueryable<EmployeeType> GetByEmployeeTypeId(int id)
        {
            return _admin.GetByEmployeeTypeId(id);
        }

        [HttpGet("GetAllEmployeeTypesByEmployee")]
        public IEnumerable<GetAllEmployeeTypeByEmployeeModel> GetAllEmployeeTypesByEmployee()
        {
            return _admin.GetAllEmployeeTypesByEmployee();
        }

        [HttpGet("GetAllEmplyoeeTypes")]
        public IQueryable<EmployeeType> GetAllEmplyoeeTypes()
        {
            return _admin.GetAllEmplyoeeTypes();
        }

        //Employee

        [HttpPost("AddEmployee")]
        public void AddEmployee(AddEmployeeModel addEmployeeModel)
        {
            _admin.AddEmployee(addEmployeeModel);
        }

        [HttpPut("EditEmployee")]
        public void EditEmployee(EditEmployeeModel editEmployeeModel)
        {
            _admin.EditEmployee(editEmployeeModel);
        }

        [HttpPut("EditEmployeIsActive")]
        public void EditEmployeIsActive(IsActiveModel EmployeIsActiveModel)
        {
            _admin.EditEmployeeTypeIsActive(EmployeIsActiveModel);
        }

        [HttpGet("GetByEmployeeId")]
        public IQueryable<Employee> GetByEmployeeId(int id)
        {
            return _admin.GetByEmployeeId(id);
        }

        [HttpGet("GetAllEmployeeByDesIdEmpTypeId")]
        public List<GetAllEmployeeByDesIdEmpTypeIdModel> GetAllEmployeeByDesIdEmpTypeId()
        {
            return _admin.GetAllEmployeeByDesIdEmpTypeId();
        }

        [HttpGet("GetAllEmployees")]
        public IQueryable<Employee> GetAllEmployees()
        {
            return _admin.GetAllEmployees();
        }

        //EmployeeProject

        [HttpPost("AddEmployeeProject")]
        public void AddEmployeeProject(AddEmployeeProjectModel addEmployeeProjectModel)
        {
            _admin.AddEmployeeProject(addEmployeeProjectModel);
        }

        [HttpPut("EditEmployeeproject")]
        public void EditEmployeeproject(EditEmployeeprojectModel editEmployeeprojectModel)
        {
            _admin.EditEmployeeProject(editEmployeeprojectModel);
        }

        [HttpGet("GetEmployeeProjectsById")]
        public List<GetEmployeeProjectsByIdModel> GetEmployeeProjectsById(int Id)
        {
            return _admin.GetEmployeeProjectsById(Id);
        }

        [HttpGet("getAllEmployeeProjectsByEmpPro")]
        public List<GetAllEmployeeProjects> getAllEmployeeProjectsByEmpPro()
        {
            return _admin.getAllEmployeeProjectsByEmpPro();
        }

        [HttpGet("GetAllEmployeeProjects")]
        public IQueryable<EmployeeProject> GetAllEmployeeProjects()
        {
            return _admin.GetAllEmployeeProjects();
        }

        //HrContactInfo

        [HttpPost("AddHrContactInfo")]
        public void AddHrContactInfo(AddHrContactModel addHrContactModel)
        {
            _admin.AddHrContactInfo(addHrContactModel);
        }

        [HttpPut("EditHrContactInfo")]
        public void EditHrContactInfo(EditHrContactModel editHrContactModel)
        {
            _admin.EditHrContactInfo(editHrContactModel);
        }

        [HttpPut("EditHrContactInfoIsActive")]
        public void EditHrContactInfoIsActive(IsActiveModel HrContactInfoIsActiveModel)
        {
            _admin.EditHrContactInfoIsActive(HrContactInfoIsActiveModel);
        }

        [HttpGet("GetByHrContactId")]
        public IQueryable<HrContactInformation> GetByHrContactId(int id)
        {
            return _admin.GetByHrContactId(id);
        }
        [HttpGet("GetHrcontactByEmployeeEmailId")]
        public IEnumerable<HrcontactByEmployeeModel> GetHrcontactByEmployeeEmailId(string Mail_Id)
        {
            return _admin.GetHrcontactByEmployeeEmailId(Mail_Id);
        }
        [HttpGet("GetAllHrContacts")]
        public IQueryable<HrContactInformation> GetAllHrContacts()
        {
            return _admin.GetAllHrContacts();
        }
    }
}
