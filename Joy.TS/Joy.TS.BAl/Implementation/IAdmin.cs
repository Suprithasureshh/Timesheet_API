using Joy.TS.DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Joy.TS.BAL.DomainModel.AdminDomainModel;

namespace Joy.TS.BAL.Implementation
{
    public interface IAdmin
    {
        //Dashboard
        public IEnumerable<GetDashboardModel> GetDashboard(int year, int Month_Id);

        //Client
        void AddClient(AddClientModel model);
        void EditClient(EditClientModel editClientModel);
        void EditClientIsActive(IsActiveModel ClientIsActiveModel);
        IQueryable<Client> GetByClientId(int id);
        IEnumerable<GetAllClientsByEmployeeModel> GetAllClientsByEmployee();
        IQueryable<Client> GetAllClients();

        //Project
        void AddProject(AddProjectsModel addProjectsModel);
        void EditProject(EditProjectsModel editProjectsModel);
        void EditProjectIsActive(IsActiveModel ProjectIsActiveModel);
        IQueryable<Projects> GetByProjectId(int id);
        public IEnumerable<GetAllProjectsByEmployeeModel> GetAllProjectsByEmployee();
        IQueryable<Projects> GetAllProjects();

        //Designation
        void AddDesignation(PostDesignationModel postDesignationModel);
        void EditDesignation(EditDesignationModel editDesignationModel);
        void EditDesignationIsActive(IsActiveModel DesignationIsActiveModel);
        IQueryable<Designations> GetByDesignationId(int id);
        IEnumerable<GetAllDesignationsByEmployeeModel> GetAllDesignationsByEmployee();
        IQueryable<Designations> GetAllDesignations();

        //EmployeeType
        void AddEmployeeType(PostEmployeeTypeModel postEmployeeTypeModel);
        void EditEmployeetype(EditEmployeeTypeModel editEmployeeTypeModel);
        void EditEmployeeTypeIsActive(IsActiveModel EmployeeTypeIsActiveModel);
        IQueryable<EmployeeType> GetByEmployeeTypeId(int id);
        IEnumerable<GetAllEmployeeTypeByEmployeeModel> GetAllEmployeeTypesByEmployee();
        IQueryable<EmployeeType> GetAllEmplyoeeTypes();

        //Employee
        void AddEmployee(AddEmployeeModel addEmployeeModel);
        void EditEmployee(EditEmployeeModel editEmployeeModel);
        void EditEmployeIsActive(IsActiveModel EmployeIsActiveModel);
        IQueryable<Employee> GetByEmployeeId(int id);
        List<GetAllEmployeeByDesIdEmpTypeIdModel> GetAllEmployeeByDesIdEmpTypeId();
        IQueryable<Employee> GetAllEmployees();

        //EmployeeProject
        void AddEmployeeProject(AddEmployeeProjectModel addEmployeeProjectModel);
        void EditEmployeeProject(EditEmployeeprojectModel editEmployeeprojectModel);
        List<GetEmployeeProjectsByIdModel> GetEmployeeProjectsById(int Id);
        List<GetAllEmployeeProjects> getAllEmployeeProjectsByEmpPro();
        IQueryable<EmployeeProject> GetAllEmployeeProjects();

        //HrContactInfo

        void AddHrContactInfo(AddHrContactModel addHrContactModel);
        void EditHrContactInfo(EditHrContactModel editHrContactModel);
        void EditHrContactInfoIsActive(IsActiveModel HrContactInfoIsActiveModel);
        IQueryable<HrContactInformation> GetByHrContactId(int id);
        IEnumerable<HrcontactByEmployeeModel> GetHrcontactByEmployeeEmailId(string Mail_Id);
        IQueryable<HrContactInformation> GetAllHrContacts();
    }
}
