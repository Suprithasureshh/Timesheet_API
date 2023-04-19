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
        void EditClientIsActive(IsActiveModel ClientIsActiveModel, bool Is_Active);
        IQueryable<Client> GetByClientId(int id);
        IEnumerable<GetAllClientsByEmployeeModel> GetAllClientsByEmployee();
        IEnumerable<ClinetIsActiveModel> GetClientIsActive(bool? isActive);
        IQueryable<Client> GetAllClients();

        //Project
        void AddProject(AddProjectsModel addProjectsModel);
        void EditProject(EditProjectsModel editProjectsModel);
        void EditProjectIsActive(IsActiveModel ProjectIsActiveModel, bool Is_Active);
        IQueryable<Projects> GetByProjectId(int id);

        IEnumerable<ProjectIsActiveModel> GetProjectIsActive(bool? isActive);
        public IEnumerable<GetAllProjectsByEmployeeModel> GetAllProjectsByEmployee();
        IQueryable<Projects> GetAllProjects();

        //Designation
        void AddDesignation(PostDesignationModel postDesignationModel);
        void EditDesignation(EditDesignationModel editDesignationModel);
        void EditDesignationIsActive(IsActiveModel DesignationIsActiveModel, bool Is_Active);
        IQueryable<Designations> GetByDesignationId(int id);
        IEnumerable<DesignationIsActiveModel> GetDesignationIsActive(bool? isActive);
        IEnumerable<GetAllDesignationsByEmployeeModel> GetAllDesignationsByEmployee();
        IQueryable<Designations> GetAllDesignations();

        //EmployeeType
        void AddEmployeeType(PostEmployeeTypeModel postEmployeeTypeModel);
        void EditEmployeetype(EditEmployeeTypeModel editEmployeeTypeModel);
        void EditEmployeeTypeIsActive(IsActiveModel EmployeeTypeIsActiveModel, bool Is_Active);
        IQueryable<EmployeeType> GetByEmployeeTypeId(int id);
        IEnumerable<EmployeeTypeIsActiveModel> GetEmployeeTypeIsActive(bool? isActive);
        IEnumerable<GetAllEmployeeTypeByEmployeeModel> GetAllEmployeeTypesByEmployee();
        IQueryable<EmployeeType> GetAllEmplyoeeTypes();

        //Role
        void AddRole(AddRoleModel addRoleModel);
        void EditRole(EditRoleModel editRoleModel);

        //Employee
        void AddEmployee(AddEmployeeModel addEmployeeModel);
        void EditEmployee(EditEmployeeModel editEmployeeModel);
        void EditEmployeeIsActive(IsActiveModel EmployeIsActiveModel, bool Is_Active);
        IQueryable<Employee> GetByEmployeeId(int id);
        List<GetAllEmployeeByDesIdEmpTypeIdModel> GetAllEmployeeByDesIdEmpTypeId();
        IEnumerable<EmployeeIsActiveModel> GetEmployeeIsActive(bool? isActive);
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
        void EditHrContactInfoIsActive(IsActiveModel HrContactInfoIsActiveModel, bool Is_Active);
        IQueryable<HrContactInformation> GetByHrContactId(int id);
        IEnumerable<HrContactInfoIsActiveModel> GetHrContactInfoIsActive(bool? isActive);
        IEnumerable<HrcontactByEmployeeModel> GetHrcontactByEmployeeEmailId(string Mail_Id);
        IQueryable<HrContactInformation> GetAllHrContacts();

        //Timesheet status
        List<GetTimeSheetStatusModel> GetTimeSheetStatus();
        IEnumerable<TimeSheetStatusByYearModel> GetTimeSheetStatusByYear(int Year);
        IEnumerable<EmployeeTimeSheetByMonthModel> GetTimeSheetStatusByMonth(int Month_id, int Year);
        IEnumerable<GetTimesheetSummaryMonthYearEmployeeModel> GetTimesheetSummaryMonthYearEmployee(int Month_id, int Year_id, int Employee_Id);

        //View Previous changes
        IEnumerable<ViewPreviousChangesModel> GetViewPreviousChanges();
        IEnumerable<ViewPreviousChangesByIdModel> GetViewPreviousChangesById(int Id);
    }
}
