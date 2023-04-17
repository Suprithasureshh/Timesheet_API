using Joy.TS.DAL.Data;
using Joy.TS.DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Joy.TS.BAL.DomainModel.AdminDomainModel;

namespace Joy.TS.BAL.Implementation
{
    public class AdminRepo : IAdmin
    {

        private readonly TimeSheetContext _timesheetContext;
        public AdminRepo(TimeSheetContext timesheetContext)
        {
            _timesheetContext = timesheetContext;
        }

        //Dashboard

        public IEnumerable<GetDashboardModel> GetDashboard(int year, int Month_Id)
        {
            var item = new GetDashboardModel();
            var item1 = (from ts in this._timesheetContext.timeSheetSummarys
                         where (ts.Year == year && ts.Fiscal_Year_ID == Month_Id)
                         group ts by ts.Status into g
                         select new GetDashboardModel
                         {
                             x = g.Key,
                             y = g.Count()
                         });
            return item1;
        }

        //Client

        public void AddClient(AddClientModel model)
        {
            var data = new Client();
            data.Client_Name = model.Client_Name;
            data.Create_Date = DateTime.UtcNow.Date;
            data.Is_Active = true;

            _timesheetContext.clients.Add(data);
            _timesheetContext.SaveChanges();
        }

        public void EditClient(EditClientModel editClientModel)
        {
            var ClientId = _timesheetContext.clients.FirstOrDefault(e =>
            e.Client_Id == editClientModel.Client_Id);

            if (ClientId != null)
            {
                ClientId.Client_Name = editClientModel.Client_Name;
                ClientId.Modified_Date = DateTime.UtcNow.Date;
                _timesheetContext.SaveChanges();
            }
            else
            {
                throw new ClientIdException();
            }
        }

        public IQueryable<Client> GetByClientId(int id)
        {
            var clients = _timesheetContext.clients.AsQueryable();
            var item = _timesheetContext.clients.FirstOrDefault(d => d.Client_Id == id);
            if (item != null)
            {
                return clients.Where(e => e.Client_Id == id);
            }
            else
            {
                throw new ClientIdException();
            }
        }

        public IEnumerable<GetAllClientsByEmployeeModel> GetAllClientsByEmployee()
        {
            var data = (from a in _timesheetContext.clients
                        join b in _timesheetContext.employees
                        on a.Client_Id equals b.Client_Id
                        where (a.Is_Active == true)
                        select new { a, b } into t1
                        group t1 by new { t1.a.Client_Name, t1.a.Client_Id }
                         into g
                        orderby g.Key.Client_Name ascending
                        select new GetAllClientsByEmployeeModel
                        {
                            Client_Id = g.Key.Client_Id,
                            Client_Name = g.Key.Client_Name,
                            No_Of_Employees = g.Count()
                        });
            return data.ToList();
        }

        public void EditClientIsActive(IsActiveModel ClientIsActiveModel)
        {

            var records = _timesheetContext.clients.Where(a => ClientIsActiveModel.Id.Contains(a.Client_Id));
            if (records.Count() != 0)
            {
                foreach (var r in records)
                {
                    r.Is_Active = ClientIsActiveModel.Is_Active;
                }
                _timesheetContext.SaveChanges();
            }
            else
            {
                throw new ClientIdException();
            }

        }

        public IQueryable<Client> GetAllClients()
        {
            return _timesheetContext.clients.Where(e => e.Is_Active == true).OrderBy(c => c.Client_Name).AsQueryable();
        }

        //Project

        public void AddProject(AddProjectsModel addProjectsModel)
        {
            var name = _timesheetContext.projects.FirstOrDefault(p => p.Project_Name == addProjectsModel.Project_Name);
            var code = _timesheetContext.projects.FirstOrDefault(e => e.Project_Code == addProjectsModel.Project_Code);
            var client = _timesheetContext.clients.FirstOrDefault(c => c.Client_Id == addProjectsModel.Client_Id);
            if (code == null)
            {
                if (name == null)
                {
                    if (client != null)
                    {
                        var pro = new Projects();

                        pro.Project_Name = addProjectsModel.Project_Name;
                        pro.Project_Code = addProjectsModel.Project_Code;
                        pro.Client_Id = addProjectsModel.Client_Id;
                        pro.Project_Start_Date = addProjectsModel.Project_Start_Date;
                        pro.Project_End_Date = addProjectsModel.Project_End_Date;
                        pro.Create_Date = DateTime.UtcNow.Date;
                        pro.Is_Active = true;

                        _timesheetContext.projects.Add(pro);
                        _timesheetContext.SaveChanges();
                    }
                    else
                    {
                        throw new ClientNotExistException();
                    }
                }
                else
                {
                    throw new ProjectNameExistException();
                }

            }
            else
            {
                throw new ProjectCodeExistException();
            }

        }

        public void EditProject(EditProjectsModel editProjectsModel)
        {
            var IdCheck = _timesheetContext.projects.FirstOrDefault(p => p.Project_Id == editProjectsModel.Project_Id);
            var doubleentry = _timesheetContext.projects.FirstOrDefault(e => e.Project_Id != editProjectsModel.Project_Id &&
                                e.Project_Name == editProjectsModel.Project_Name);
            if (IdCheck != null)
            {
                if (doubleentry == null || doubleentry.Project_Id == IdCheck.Project_Id)
                {
                    IdCheck.Project_Name = editProjectsModel.Project_Name;
                    IdCheck.Project_Code = IdCheck.Project_Code;
                    IdCheck.Client_Id = editProjectsModel.Client_Id;
                    IdCheck.Project_Start_Date = IdCheck.Project_Start_Date;
                    IdCheck.Project_End_Date = editProjectsModel.End_Date;
                    IdCheck.Modified_Date = DateTime.UtcNow.Date;
                    IdCheck.Is_Active = true;
                    _timesheetContext.SaveChanges();

                }
                else
                {
                    throw new ProjectNameExistException();
                }
            }
            else
            {
                throw new ProjectIdNotExistException();
            }
        }

        public void EditProjectIsActive(IsActiveModel ProjectIsActiveModel)
        {
            var records = _timesheetContext.projects.Where(a => ProjectIsActiveModel.Id.Contains(a.Project_Id));
            if (records.Count() != 0)
            {
                foreach (var r in records)
                {
                    r.Is_Active = ProjectIsActiveModel.Is_Active;
                }
                _timesheetContext.SaveChanges();
            }
            else
            {
                throw new ProjectIdNotExistException();
            }
        }

        public IQueryable<Projects> GetByProjectId(int id)
        {
            var project = _timesheetContext.projects.AsQueryable();
            var item = _timesheetContext.projects.FirstOrDefault(d => d.Project_Id == id);
            if (item != null)
            {
                return project.Where(e => e.Project_Id == id);
            }
            else
            {
                throw new ProjectIdNotExistException();
            }
        }

        public IEnumerable<GetAllProjectsByEmployeeModel> GetAllProjectsByEmployee()
        {
            var data = (from a in _timesheetContext.projects
                        join b in _timesheetContext.employees
                        on a.Project_Id equals b.Project_Id
                        where (a.Is_Active == true)
                        select new { a, b } into t1
                        group t1 by new { t1.a.Project_Name, t1.a.Project_Id, t1.a.Project_Code, t1.a.Project_Start_Date, t1.a.Project_End_Date }
                        into g
                        orderby g.Key.Project_Name ascending
                        select new GetAllProjectsByEmployeeModel
                        {
                            Project_Id = g.Key.Project_Id,
                            Project_Name = g.Key.Project_Name,
                            Project_Code = g.Key.Project_Code,
                            No_Of_Employees = g.Count(),
                            Start_Date = g.Key.Project_Start_Date,
                            End_Date = g.Key.Project_End_Date,
                        });
            return data.ToList();
        }

        public IQueryable<Projects> GetAllProjects()
        {
            return _timesheetContext.projects.Where(e => e.Is_Active == true).OrderBy(e => e.Project_Name).AsQueryable();
        }

        //Designation

        public void AddDesignation(PostDesignationModel postDesignationModel)
        {
            var table = _timesheetContext.designations.FirstOrDefault(e => e.Designation == postDesignationModel.Designation);
            if (table == null)
            {
                var data = new Designations();
                data.Designation = postDesignationModel.Designation;
                data.Create_Date = DateTime.UtcNow.Date;
                data.Is_Active = true;
                _timesheetContext.designations.Add(data);
                _timesheetContext.SaveChanges();
            }
            else
            {
                throw new DesignationNameException();
            }
        }

        public void EditDesignation(EditDesignationModel editDesignationModel)
        {
            var DesignationIdCheck = _timesheetContext.designations.FirstOrDefault
                 (e => (e.Designation_Id == editDesignationModel.Designation_Id));
            var DesignationNameCheck = _timesheetContext.designations.FirstOrDefault
                 (e => (e.Designation == editDesignationModel.Designation));

            if (DesignationNameCheck == null)
            {
                if (DesignationIdCheck != null)
                {
                    DesignationIdCheck.Designation = editDesignationModel.Designation;
                    DesignationIdCheck.Modified_Date = DateTime.UtcNow.Date;
                    _timesheetContext.SaveChanges();
                }
                else
                {
                    throw new DesignationIdException();
                }
            }
            else
            {
                throw new DesignationNameException();
            }
        }

        public void EditDesignationIsActive(IsActiveModel DesignationIsActiveModel)
        {
            var records = _timesheetContext.designations.Where(a => DesignationIsActiveModel.Id.Contains(a.Designation_Id));
            if (records.Count() != 0)
            {
                foreach (var r in records)
                {
                    r.Is_Active = DesignationIsActiveModel.Is_Active;
                }
                _timesheetContext.SaveChanges();
            }
            else
            {
                throw new DesignationIdException();
            }
        }

        public IQueryable<Designations> GetByDesignationId(int id)
        {
            var designations = _timesheetContext.designations.AsQueryable();
            var item = _timesheetContext.designations.FirstOrDefault(d => d.Designation_Id == id);
            if (item != null)
            {
                return designations.Where(e => e.Designation_Id == id);
            }
            else
            {
                throw new DesignationIdException();
            }
        }

        public IEnumerable<GetAllDesignationsByEmployeeModel> GetAllDesignationsByEmployee()
        {
            var data = from a in _timesheetContext.designations
                       join b in _timesheetContext.employees
                       on a.Designation_Id equals b.Designation_Id
                       where (a.Is_Active == true)
                       select new { a, b } into t1
                       group t1 by new { t1.a.Designation, t1.a.Designation_Id, empcount = t1.b.Employee_Id == null ? 0 : 1 }
                        into g
                       orderby g.Key.Designation
                       select new GetAllDesignationsByEmployeeModel
                       {
                           Designation_Id = g.Key.Designation_Id,
                           Designation = g.Key.Designation,
                           No_of_Employees = g.Key.empcount == 0 ? 0 : g.Count()
                       };
            return data.ToList();
        }

        public IQueryable<Designations> GetAllDesignations()
        {
            return _timesheetContext.designations.Where(e => e.Is_Active == true).OrderBy(e => e.Designation).AsQueryable();
        }

        //Employee Type

        public void AddEmployeeType(PostEmployeeTypeModel postEmployeeTypeModel)
        {
            var table = _timesheetContext.employeeTypes.FirstOrDefault(e => e.Employee_Type == postEmployeeTypeModel.Employee_Type);

            if (table == null)
            {
                var data = new EmployeeType();
                data.Employee_Type = postEmployeeTypeModel.Employee_Type;
                data.Create_Date = DateTime.UtcNow.Date;
                data.Is_Active = true;

                _timesheetContext.employeeTypes.Add(data);
                _timesheetContext.SaveChanges();
            }
            else
            {
                throw new EmployeeTypeNameException();
            }
        }

        public void EditEmployeetype(EditEmployeeTypeModel editEmployeeTypeModel)
        {
            var IdCheck = _timesheetContext.employeeTypes.FirstOrDefault
                 (e => (e.Employee_Type_Id == editEmployeeTypeModel.Employee_Type_Id));
            var NameCheck = _timesheetContext.employeeTypes.FirstOrDefault
                 (e => (e.Employee_Type == editEmployeeTypeModel.Employee_Type));

            if (NameCheck == null)
            {
                if (IdCheck != null)
                {
                    IdCheck.Employee_Type = editEmployeeTypeModel.Employee_Type;
                    IdCheck.Modified_Date = DateTime.UtcNow.Date;
                    _timesheetContext.SaveChanges();
                }
                else
                {
                    throw new EmployeeTypeIdException();
                }
            }
            else
            {
                throw new EmployeeTypeNameException();
            }
        }

        public void EditEmployeeTypeIsActive(IsActiveModel EmployeeTypeIsActiveModel)
        {
            var records = _timesheetContext.employeeTypes.Where(a => EmployeeTypeIsActiveModel.Id.Contains(a.Employee_Type_Id));
            if (records.Count() != 0)
            {
                foreach (var r in records)
                {
                    r.Is_Active = EmployeeTypeIsActiveModel.Is_Active;
                }
                _timesheetContext.SaveChanges();
            }
            else
            {
                throw new EmployeeTypeIdException();
            }
        }

        public IQueryable<EmployeeType> GetByEmployeeTypeId(int id)
        {
            var employeeType = _timesheetContext.employeeTypes.AsQueryable();
            var item = _timesheetContext.employeeTypes.FirstOrDefault(d => d.Employee_Type_Id == id);
            if (item != null)
            {
                return employeeType.Where(e => e.Employee_Type_Id == id);
            }
            else
            {
                throw new EmployeeTypeIdException();
            }
        }

        public IEnumerable<GetAllEmployeeTypeByEmployeeModel> GetAllEmployeeTypesByEmployee()
        {
            var data = (from a in _timesheetContext.employeeTypes
                        join b in _timesheetContext.employees
                        on a.Employee_Type_Id equals b.Employee_Type_Id
                        where (a.Is_Active == true)
                        select new { a, b } into t1
                        group t1 by new { t1.a.Employee_Type, t1.a.Employee_Type_Id }
                         into g
                        orderby g.Key.Employee_Type ascending
                        select new GetAllEmployeeTypeByEmployeeModel
                        {
                            Employee_Type_Id = g.Key.Employee_Type_Id,
                            Employee_Type = g.Key.Employee_Type,
                            No_of_Employees = g.Count()
                        });
            return data.ToList();
        }

        public IQueryable<EmployeeType> GetAllEmplyoeeTypes()
        {
            return _timesheetContext.employeeTypes.Where(e => e.Is_Active == true).OrderBy(e => e.Employee_Type).AsQueryable();
        }

        //Employee

        public void AddEmployee(AddEmployeeModel addEmployeeModel)
        {
            var EmailContactCheck = _timesheetContext.employees.FirstOrDefault(e => e.Official_Email == addEmployeeModel.Official_Email || e.Contact_No == addEmployeeModel.Contact_No);
            var ts = new TimeSheetSummary();
            if (EmailContactCheck == null)
            {
                var emp = new Employee();
                emp.First_Name = addEmployeeModel.First_Name;
                emp.Last_Name = addEmployeeModel.Last_Name;
                emp.Employee_code = addEmployeeModel.Employee_code;
                emp.Reporting_Manager1 = addEmployeeModel.Reporting_Manager1;
                emp.Reportinng_Manager2 = addEmployeeModel.Reportinng_Manager2;
                emp.Employee_Type_Id = addEmployeeModel.Employee_Type_Id;
                emp.Role_Id = addEmployeeModel.Role_id;
                emp.Official_Email = addEmployeeModel.Official_Email;
                emp.Alternate_Email = addEmployeeModel.Alternate_Email;
                emp.Contact_No = addEmployeeModel.Contact_No;
                emp.Password = addEmployeeModel.Password;
                emp.Designation_Id = addEmployeeModel.Designation_Id;
                emp.Employee_Type_Id = addEmployeeModel.Employee_Type_Id;
                emp.Is_Active = true;
                emp.Joining_Date = addEmployeeModel.Joining_Date;
                emp.Create_Date = DateTime.UtcNow.Date;
                _timesheetContext.employees.Add(emp);
                _timesheetContext.SaveChanges();

                var max = _timesheetContext.employees.Max(e => e.Employee_Id);
                ts.Created_Date = DateTime.UtcNow.Date;
                ts.Employee_Id = max;
                ts.No_Of_days_Worked = 0;
                ts.No_Of_Leave_Taken = 0;
                ts.Status = "Pending";
                ts.Total_Working_Hours = 0;
                ts.Year = DateTime.UtcNow.Year;
                _timesheetContext.timeSheetSummarys.Add(ts);
                _timesheetContext.SaveChanges();
            }
            else
            {
                var a = _timesheetContext.employees.FirstOrDefault(e => ((e.Official_Email == addEmployeeModel.Official_Email || e.Alternate_Email == addEmployeeModel.Official_Email)) || e.Contact_No == addEmployeeModel.Contact_No);
                if (a.Official_Email == addEmployeeModel.Official_Email && a.Contact_No != addEmployeeModel.Contact_No)
                {
                    throw new EmployeeEmailExistException();
                }
                else if (a.Official_Email != addEmployeeModel.Official_Email && a.Contact_No == addEmployeeModel.Contact_No)
                {
                    throw new EmployeeContactExistException();
                }
                else
                {
                    throw new EmployeeEmailExistException();
                    throw new EmployeeContactExistException();
                }
            }
        }

        public void EditEmployee(EditEmployeeModel editEmployeeModel)
        {
            var IdCheck = _timesheetContext.employees.FirstOrDefault(e => e.Employee_Id == editEmployeeModel.Employee_Id);
            var doubleentry = _timesheetContext.employees.FirstOrDefault(e => e.Employee_Id != editEmployeeModel.Employee_Id && (e.Official_Email == editEmployeeModel.Official_Email || e.Contact_No == editEmployeeModel.Contact_No
            || e.Official_Email == editEmployeeModel.Alternate_Email || e.Alternate_Email == editEmployeeModel.Alternate_Email));
            var data = new ViewPreviousChanges();
            if (IdCheck != null)
            {
                if (doubleentry == null || doubleentry.Employee_Id == IdCheck.Employee_Id)
                {
                    data.Employee_Id = IdCheck.Employee_Id;
                    data.First_Name = IdCheck.First_Name;
                    data.Last_Name = IdCheck.Last_Name;
                    data.Employee_code = IdCheck.Employee_code;
                    data.Employee_Type_Id = IdCheck.Employee_Type_Id;
                    data.Email = IdCheck.Official_Email;
                    data.Alternate_Email = IdCheck.Alternate_Email;
                    data.Designation_Id = IdCheck.Designation_Id;
                    data.Role_Id = IdCheck.Role_Id;
                    data.Contact_No = IdCheck.Contact_No;
                    data.Reporting_Manager1 = IdCheck.Reporting_Manager1;
                    data.Reportinng_Manager2 = IdCheck.Reportinng_Manager2;
                    data.Is_Active = IdCheck.Is_Active;
                    data.Joining_Date = IdCheck.Joining_Date;
                    data.End_Date = IdCheck.End_Date;
                    data.Modified_Date = IdCheck.Modified_Date;
                    _timesheetContext.viewPreviousChanges.Update(data);
                    _timesheetContext.SaveChanges();

                    //data.Employee_Id = editEmployeeModel.Employee_Id;
                    //data.First_Name = editEmployeeModel.First_Name;
                    //data.Last_Name = editEmployeeModel.Last_Name;
                    //data.Employee_code = editEmployeeModel.Employee_code;
                    //data.Employee_Type_Id = editEmployeeModel.Employee_Type_Id;
                    //data.Email = editEmployeeModel.Official_Email;
                    //data.Alternate_Email = editEmployeeModel.Alternate_Email;
                    //data.Designation_Id = editEmployeeModel.Designation_Id;
                    //data.Contact_No = editEmployeeModel.Contact_No;
                    //data.Reporting_Manager1 = editEmployeeModel.Reporting_Manager1;
                    //data.Reportinng_Manager2 = editEmployeeModel.Reportinng_Manager2;
                    //data.Joining_Date = editEmployeeModel.Joining_Date;
                    //data.End_Date = editEmployeeModel.End_Date;
                    //data.Modified_Date = DateTime.Now.Date;
                    //_timesheetContext.viewPreviousChanges.Update(data);
                    //_timesheetContext.SaveChanges();

                    IdCheck.Employee_Id = editEmployeeModel.Employee_Id;
                    IdCheck.First_Name = editEmployeeModel.First_Name;
                    IdCheck.Last_Name = editEmployeeModel.Last_Name;
                    IdCheck.Employee_Type_Id = editEmployeeModel.Employee_Type_Id;
                    IdCheck.Official_Email = editEmployeeModel.Official_Email;
                    IdCheck.Employee_code = editEmployeeModel.Employee_code;
                    IdCheck.Alternate_Email = editEmployeeModel.Alternate_Email;
                    IdCheck.Designation_Id = editEmployeeModel.Designation_Id;
                    IdCheck.Role_Id = editEmployeeModel.Role_id;
                    IdCheck.Contact_No = editEmployeeModel.Contact_No;
                    IdCheck.Reporting_Manager1 = editEmployeeModel.Reporting_Manager1;
                    IdCheck.Reportinng_Manager2 = editEmployeeModel.Reportinng_Manager2;
                    IdCheck.Joining_Date = editEmployeeModel.Joining_Date;
                    IdCheck.End_Date = editEmployeeModel.End_Date;
                    IdCheck.Modified_Date = DateTime.Now.Date;
                    _timesheetContext.SaveChanges();
                }
                else
                {
                    var p = _timesheetContext.employees.FirstOrDefault(e => e.Official_Email == editEmployeeModel.Official_Email || e.Contact_No == editEmployeeModel.Contact_No);
                    if (p.Official_Email == editEmployeeModel.Official_Email && p.Contact_No != editEmployeeModel.Contact_No)
                    {
                        throw new EmployeeContactExistException();
                    }
                    else if (p.Official_Email != editEmployeeModel.Official_Email && p.Contact_No == editEmployeeModel.Contact_No)
                    {
                        throw new EmployeeEmailExistException();
                    }
                    else
                    {
                        throw new EmployeeContactExistException();
                        throw new EmployeeEmailExistException();
                    }
                }
            }
            throw new EmployeeIdNotExistException();
        }

        public void EditEmployeIsActive(IsActiveModel EmployeIsActiveModel)
        {
            var records = _timesheetContext.employees.Where(a => EmployeIsActiveModel.Id.Contains(a.Employee_Id));
            if (records.Count() != 0)
            {
                foreach (var r in records)
                {
                    r.Is_Active = EmployeIsActiveModel.Is_Active;
                }
                _timesheetContext.SaveChanges();
            }
            else
            {
                throw new EmployeeIdNotExistException();
            }
        }

        public IQueryable<Employee> GetByEmployeeId(int id)
        {
            var employee = _timesheetContext.employees.AsQueryable();
            var item = _timesheetContext.employees.FirstOrDefault(d => d.Employee_Id == id);
            if (item != null)
            {
                return employee.Where(e => e.Employee_Id == id);
            }
            else
            {
                throw new EmployeeIdNotExistException();
            }
        }

        public List<GetAllEmployeeByDesIdEmpTypeIdModel> GetAllEmployeeByDesIdEmpTypeId()
        {
            var data = from Emp in _timesheetContext.employees
                       join Des in _timesheetContext.designations
                       on Emp.Designation_Id equals Des.Designation_Id
                       join EmpTy in _timesheetContext.employeeTypes
                       on Emp.Employee_Type_Id equals EmpTy.Employee_Type_Id
                       where Emp.Is_Active == true
                       orderby Emp.First_Name ascending
                       select new GetAllEmployeeByDesIdEmpTypeIdModel
                       {
                           Employee_Id = Emp.Employee_Id,
                           First_Name = Emp.First_Name,
                           last_Name = Emp.Last_Name,
                           Employee_code = Emp.Employee_code,
                           Reporting_Manager1 = Emp.Reporting_Manager1,
                           Reportinng_Manager2 = Emp.Reportinng_Manager2,
                           Employee_Type_Id = Emp.Employee_Type_Id,
                           Employee_Type_Name = EmpTy.Employee_Type,
                           Official_Email = Emp.Official_Email,
                           Alternate_Email = Emp.Alternate_Email,

                           Designation_Id = Emp.Designation_Id,
                           Designation_Name = Des.Designation,
                           Contact_No = Emp.Contact_No,
                           Joining_Date = Emp.Joining_Date,
                           End_Date = Emp.End_Date.HasValue ?
                                      Emp.End_Date.Value : DateTime.MinValue
                       };
            return data.ToList();
        }

        public IQueryable<Employee> GetAllEmployees()
        {
            return _timesheetContext.employees.Where(e => e.Is_Active == true).OrderBy(e => e.First_Name).AsQueryable();
        }

        //EmployeeProject

        public void AddEmployeeProject(AddEmployeeProjectModel addEmployeeProjectModel)
        {
            var data = new EmployeeProject();
            data.Employee_Id = addEmployeeProjectModel.Employee_Id;
            data.Project_Id = addEmployeeProjectModel.Project_Id;
            data.Start_Date = addEmployeeProjectModel.Start_Date;
            data.End_Date = addEmployeeProjectModel.End_Date;
            data.Location = addEmployeeProjectModel.Location;
            data.Create_Date = DateTime.UtcNow.Date;

            _timesheetContext.employeeProject.Add(data);
            _timesheetContext.SaveChanges();
        }

        public void EditEmployeeProject(EditEmployeeprojectModel editEmployeeprojectModel)
        {
            var item = new EmployeeProject();
            item.Employee_Project_Id = editEmployeeprojectModel.Employee_Project_Id;
            item.Employee_Id = editEmployeeprojectModel.Employee_Id;
            item.Project_Id = editEmployeeprojectModel.Project_Id;
            item.Location = editEmployeeprojectModel.Location;
            item.Start_Date = editEmployeeprojectModel.Start_Date;
            item.End_Date = editEmployeeprojectModel.End_Date;
            item.Modified_Date = DateTime.UtcNow.Date;

            _timesheetContext.employeeProject.Update(item);
            _timesheetContext.SaveChanges();
        }

        public List<GetEmployeeProjectsByIdModel> GetEmployeeProjectsById(int Id)
        {
            var res = new GetEmployeeProjectsByIdModel();
            var data = from emppro in _timesheetContext.employeeProject
                       join pro in _timesheetContext.projects
                       on emppro.Project_Id equals pro.Project_Id
                       where (emppro.Employee_Project_Id == Id)
                       join emp in _timesheetContext.employees
                       on emppro.Employee_Id equals emp.Employee_Id
                       select new GetEmployeeProjectsByIdModel
                       {
                           Employee_Project_Id = emppro.Employee_Project_Id,
                           Employee_Name = emp.First_Name + " " + emp.Last_Name,
                           Project_Name = pro.Project_Name,
                           Location = emppro.Location,
                           Start_Date = emppro.Start_Date.ToString("yyyy/MM/dd"),
                           End_Date = emppro.End_Date.HasValue ?
                                      emppro.End_Date.Value.ToString("dd/MM/yyyy") : string.Empty,
                       };

            return data.ToList();
        }

        public List<GetAllEmployeeProjects> getAllEmployeeProjectsByEmpPro()
        {
            var data = from emppro in _timesheetContext.employeeProject
                       join pro in _timesheetContext.projects
                       on emppro.Project_Id equals pro.Project_Id
                       join emp in _timesheetContext.employees
                       on emppro.Employee_Id equals emp.Employee_Id
                       select new GetAllEmployeeProjects
                       {
                           Employee_Project_Id = emppro.Employee_Project_Id,
                           First_Name = emp.First_Name,
                           Last_Name = emp.Last_Name,
                           Project_Name = pro.Project_Name,
                           Start_Date = emppro.Start_Date.ToString("yyyy/MM/dd"),
                           End_Date = emppro.End_Date.HasValue ?
                                      emppro.End_Date.Value.ToString("dd/MM/yyyy") : string.Empty,
                           Location = emppro.Location
                       };
            return data.ToList();
        }

        public IQueryable<EmployeeProject> GetAllEmployeeProjects()
        {
            return _timesheetContext.employeeProject.AsQueryable();
        }

        //HR Contact Information

        public void AddHrContactInfo(AddHrContactModel addHrContactModel)
        {
            var table = _timesheetContext.employees.FirstOrDefault(a => a.Official_Email == addHrContactModel.Hr_Email_Id);
            var doubleentry = _timesheetContext.hrContactInformations.FirstOrDefault(a => a.Hr_Email_Id == table.Official_Email || a.Hr_Contact_No == table.Contact_No);

            if (table != null)
            {
                if (doubleentry == null)
                {
                    var data = new HrContactInformation();
                    data.First_Name = table.First_Name;
                    data.Last_Name = table.Last_Name;
                    data.Hr_Email_Id = table.Official_Email;
                    data.Hr_Contact_No = table.Contact_No;
                    data.Is_Active = true;

                    _timesheetContext.hrContactInformations.Add(data);
                    _timesheetContext.SaveChanges();
                }
                else
                {
                    if (table.Official_Email == doubleentry.Hr_Email_Id && table.Contact_No != doubleentry.Hr_Contact_No)
                    {
                        throw new HrMailExistException();
                    }
                    else if (doubleentry.Hr_Contact_No == table.Contact_No && doubleentry.Hr_Email_Id != table.Official_Email)
                    {
                        throw new HrConatactException();
                    }
                    else
                    {
                        throw new HrMailExistException();
                        throw new HrConatactException();
                    }
                }
            }
            else
            {
                throw new HrMailNotExistException();
            }
        }

        public void EditHrContactInfo(EditHrContactModel editHrContactModel)
        {
            var IdCheck = _timesheetContext.hrContactInformations.FirstOrDefault(h => h.Hr_Contact_Id == editHrContactModel.Hr_Contact_Id);
            var EmailCheck = _timesheetContext.hrContactInformations.FirstOrDefault(h => h.Hr_Email_Id == editHrContactModel.Hr_Email_Id);
            var PhoneCheck = _timesheetContext.hrContactInformations.FirstOrDefault(h => h.Hr_Contact_No == editHrContactModel.Hr_Contact_No);

            var doublee = _timesheetContext.hrContactInformations.FirstOrDefault(g => g.Hr_Contact_Id != editHrContactModel.Hr_Contact_Id &&
            (g.Hr_Contact_No == editHrContactModel.Hr_Contact_No));

            if (IdCheck != null)
            {
                if (EmailCheck == null)
                {
                    if (PhoneCheck == null)
                    {
                        IdCheck.First_Name = editHrContactModel.First_Name;
                        IdCheck.Last_Name = editHrContactModel.Last_Name;
                        IdCheck.Hr_Email_Id = editHrContactModel.Hr_Email_Id;
                        IdCheck.Hr_Contact_No = editHrContactModel.Hr_Contact_No;
                        IdCheck.Is_Active = true;

                        _timesheetContext.SaveChanges();
                    }
                    else
                    {
                        throw new Exception("Contact number already exists");
                    }
                }
                else
                {
                    throw new Exception("Email Id already exists");
                }
            }
            else
            {
                throw new Exception("Id does not Exist");
            }
        }

        public void EditHrContactInfoIsActive(IsActiveModel HrContactInfoIsActiveModel)
        {
            var records = _timesheetContext.hrContactInformations.Where(a => HrContactInfoIsActiveModel.Id.Contains(a.Hr_Contact_Id));
            if (records.Count() != 0)
            {
                foreach (var r in records)
                {
                    r.Is_Active = HrContactInfoIsActiveModel.Is_Active;
                }
                _timesheetContext.SaveChanges();
            }
            else
            {
                throw new HrIdException();
            }

        }

        public IQueryable<HrContactInformation> GetByHrContactId(int id)
        {
            var hrContact = _timesheetContext.hrContactInformations.AsQueryable();
            var item = _timesheetContext.hrContactInformations.FirstOrDefault(d => d.Hr_Contact_Id == id);
            if (item != null)
            {
                return hrContact.Where(e => e.Hr_Contact_Id == id);
            }
            else
            {
                throw new Exception();
            }
        }

        public IEnumerable<HrcontactByEmployeeModel> GetHrcontactByEmployeeEmailId(string Mail_Id)                                               //UserProfile
        {
            var data = from emp in this._timesheetContext.employees
                       where emp.Official_Email == Mail_Id
                       select new HrcontactByEmployeeModel
                       {
                           Hr_Contact_Id = emp.Employee_Id,
                           Name = emp.First_Name + " " + emp.Last_Name,
                           Joining_Date = emp.Joining_Date.ToString("dd-MM-yyyy"),
                           Email = emp.Official_Email,
                           Mobile_No = emp.Contact_No.ToString()
                       };
            return data.ToList();
        }

        public IQueryable<HrContactInformation> GetAllHrContacts()
        {
            return _timesheetContext.hrContactInformations.Where(e => e.Is_Active == true).OrderBy(e => e.First_Name).AsQueryable();
        }




    }
}
