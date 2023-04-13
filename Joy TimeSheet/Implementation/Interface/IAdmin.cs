using TimeSheet.Model;
using static TimeSheet.Implementation.DomainModel.AdminDomainModel;

namespace TimeSheet.Implementation.Interface
{
    public interface IAdmin
    {
        //Client
        void AddClient(AddClientModel model);
        void EditClient(EditClientModel editClientModel);
        IQueryable<Client> GetByClientId(int id);
        IQueryable<Client> GetAllClients();

        //Project
        void AddProject(AddProjectsModel addProjectsModel);
        void EditProject(EditProjectsModel editProjectsModel);
        IQueryable<Projects> GetByProjectId(int id);
        IQueryable<Projects> GetAllProjects();

        //Designation
        void AddDesignation(PostDesignationModel postDesignationModel);
        void EditDesignation(EditDesignationModel editDesignationModel);
        IQueryable<Designations> GetByDesignationId(int id);
        IEnumerable<GetAllDesignationsByEmployeeModel> GetAllDesignationsByEmployee();
        IQueryable<Designations> GetAllDesignations();

        //EmployeeType
        void AddEmployeeType(PostEmployeeTypeModel postEmployeeTypeModel);
        void EditEmployeetype(EditEmployeeTypeModel editEmployeeTypeModel);
        IQueryable<EmployeeType> GetByEmployeeTypeId(int id);
        IEnumerable<GetAllEmployeeTypeByEmployeeModel> GetAllEmployeeTypesByEmployee();
        IQueryable<EmployeeType> GetAllEmplyoeeTypes();

        //Employee
        void AddEmployee(AddEmployeeModel addEmployeeModel);
        void EditEmployee(EditEmployeeModel editEmployeeModel);
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
        IQueryable<HrContactInformation> GetByHrContactId(int id);
        IEnumerable<HrcontactByEmployeeModel> GetHrcontactByEmployeeEmailId(string Mail_Id);
        IQueryable<HrContactInformation> GetAllHrContacts();
    }
}
