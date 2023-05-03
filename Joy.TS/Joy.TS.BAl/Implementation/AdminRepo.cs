using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml;
using Joy.TS.DAL.Data;
using Joy.TS.DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Joy.TS.BAL.DomainModel.AdminDomainModel;
using DocumentFormat.OpenXml.Math;
using HorizontalAlignmentValues = DocumentFormat.OpenXml.Spreadsheet.HorizontalAlignmentValues;
using Alignment = DocumentFormat.OpenXml.Spreadsheet.Alignment;
using System.Net.Mail;
using System.Net;

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
            var item1 = (from ts in _timesheetContext.timeSheetSummarys
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
            var ClientCheck = _timesheetContext.clients.FirstOrDefault(e => e.Client_Name == model.Client_Name);
            if (ClientCheck == null)
            {
                var data = new Client();
                data.Client_Name = model.Client_Name;
                data.Create_Date = DateTime.UtcNow.Date;
                data.Is_Active = true;

                _timesheetContext.clients.Add(data);
                _timesheetContext.SaveChanges();
            }
            else
            {
                throw new ClientNameExistException();
            }
        }

        public void EditClient(EditClientModel editClientModel)
        {
            var ClientId = _timesheetContext.clients.FirstOrDefault(e => e.Client_Id == editClientModel.Client_Id);
            var ClientCheck = _timesheetContext.clients.FirstOrDefault(e => e.Client_Id != editClientModel.Client_Id && e.Client_Name == editClientModel.Client_Name);

            if (ClientId != null)
            {
                if (ClientCheck == null)
                {
                    ClientId.Client_Name = editClientModel.Client_Name;
                    ClientId.Modified_Date = DateTime.UtcNow.Date;
                    _timesheetContext.SaveChanges();
                }
                else
                {
                    throw new ClientNameExistException();
                }
            }
            else
            {
                throw new ClientIdException();
            }
        }

        public void EditClientIsActive(IsActiveModel ClientIsActiveModel, bool Is_Active)
        {

            var records = _timesheetContext.clients.Where(a => ClientIsActiveModel.Id.Contains(a.Client_Id));
            if (records.Count() != 0)
            {
                foreach (var r in records)
                {
                    r.Is_Active = Is_Active;
                }
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

        public IEnumerable<ClinetIsActiveModel> GetClientIsActive(bool? isActive)
        {
            var data = from c in _timesheetContext.clients
                       join p in _timesheetContext.projects
                       on c.Client_Id equals p.Client_Id into cp
                       from x in cp.DefaultIfEmpty()
                       join ep in _timesheetContext.employeeProject
                       on x.Project_Id equals ep.Project_Id into epx
                       from y in epx.DefaultIfEmpty()
                       group y by new { c.Client_Id, c.Client_Name, c.Is_Active, c.Create_Date } into g
                       orderby g.Key.Client_Name
                       select new ClinetIsActiveModel
                       {
                           Client_Id = g.Key.Client_Id,
                           Client_Name = g.Key.Client_Name,
                           Is_Active = g.Key.Is_Active,
                           Create_Date = g.Key.Create_Date,
                           No_Of_Employees = g.Select(x => x.Employee_Id).Distinct().Count()
                       };
            if (isActive == true)
            {
                return data.Where(e => e.Is_Active).ToList();
            }
            else if (isActive == false)
            {
                return data.Where(e => !e.Is_Active).ToList();
            }
            else
            {
                return data.ToList();
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
                        pro.Project_End_Date = addProjectsModel.Project_End_Date.Date;
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
            var client = _timesheetContext.clients.FirstOrDefault(c => c.Client_Id == editProjectsModel.Client_Id);

            if (IdCheck != null)
            {
                if (client != null)
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
                    throw new ClientNotExistException();
                }
            }
            else
            {
                throw new ProjectIdNotExistException();
            }
        }

        public void EditProjectIsActive(IsActiveModel ProjectIsActiveModel, bool Is_Active)
        {
            var records = _timesheetContext.projects.Where(a => ProjectIsActiveModel.Id.Contains(a.Project_Id));
            if (records.Count() != 0)
            {
                foreach (var r in records)
                {
                    r.Is_Active = Is_Active;
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

        public IEnumerable<ProjectIsActiveModel> GetProjectIsActive(bool? isActive)
        {
            var data = from p in _timesheetContext.projects
                       join ep in _timesheetContext.employeeProject
                       on p.Project_Id equals ep.Project_Id into emp
                       from e in emp.DefaultIfEmpty()
                       select new { p, e } into t1
                       group t1 by new
                       {
                           t1.p.Project_Id,
                           t1.p.Project_Name,
                           t1.p.Project_Code,
                           t1.p.Client_Id,
                           t1.p.Project_Start_Date,
                           t1.p.Project_End_Date,
                           t1.p.Is_Active
                       } into g
                       orderby g.Key.Project_Name
                       select new ProjectIsActiveModel
                       {
                           Project_Id = g.Key.Project_Id,
                           Project_Name = g.Key.Project_Name,
                           Project_Code = g.Key.Project_Code,
                           Client_Id = g.Key.Client_Id,
                           Project_Start_Date = g.Key.Project_Start_Date,
                           Project_End_Date = g.Key.Project_End_Date,
                           Is_Active = g.Key.Is_Active,
                           No_Of_Employees = g.Count(x => x.e != null)
                       };
            if (isActive == true)
            {
                return data.Where(e => e.Is_Active).ToList();
            }
            else if (isActive == false)
            {
                return data.Where(e => !e.Is_Active).ToList();
            }
            else
            {
                return data.ToList();
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
                 (e => e.Designation_Id != editDesignationModel.Designation_Id && (e.Designation == editDesignationModel.Designation));

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

        public void EditDesignationIsActive(IsActiveModel DesignationIsActiveModel, bool Is_Active)
        {
            var records = _timesheetContext.designations.Where(a => DesignationIsActiveModel.Id.Contains(a.Designation_Id));
            if (records.Count() != 0)
            {
                foreach (var r in records)
                {
                    r.Is_Active = Is_Active;
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

        public IEnumerable<DesignationIsActiveModel> GetDesignationIsActive(bool? isActive)
        {

            var data = (from a in _timesheetContext.designations
                        join b in _timesheetContext.employees
                        on a.Designation_Id equals b.Designation_Id into employees
                        from b in employees.DefaultIfEmpty()
                        select new { a, b }
                        into t1
                        group t1 by new { t1.a.Designation, t1.a.Designation_Id, t1.a.Is_Active } into g
                        orderby g.Key.Designation

                        select new DesignationIsActiveModel
                        {
                            Designation_Id = g.Key.Designation_Id,
                            Designation = g.Key.Designation,
                            Is_Active = g.Key.Is_Active,
                            No_of_Employees = g.Count(x => x.b != null)

                        });

            if (isActive == true)
            {
                return data.Where(e => e.Is_Active).ToList();
            }
            else if (isActive == false)
            {
                return data.Where(e => !e.Is_Active).ToList();
            }
            else
            {
                return data.ToList();
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

        public void EditEmployeeTypeIsActive(IsActiveModel EmployeeTypeIsActiveModel, bool Is_Active)
        {
            var records = _timesheetContext.employeeTypes.Where(a => EmployeeTypeIsActiveModel.Id.Contains(a.Employee_Type_Id));
            if (records.Count() != 0)
            {
                foreach (var r in records)
                {
                    r.Is_Active = Is_Active;
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

        public IEnumerable<EmployeeTypeIsActiveModel> GetEmployeeTypeIsActive(bool? isActive)
        {

            var data = from a in _timesheetContext.employeeTypes
                       join b in _timesheetContext.employees
                       on a.Employee_Type_Id equals b.Employee_Type_Id into employees
                       from b in employees.DefaultIfEmpty()
                       select new { a, b } into t1
                       group t1 by new { t1.a.Employee_Type, t1.a.Employee_Type_Id, t1.a.Is_Active } into g
                       orderby g.Key.Employee_Type
                       select new EmployeeTypeIsActiveModel
                       {
                           Employee_Type_Id = g.Key.Employee_Type_Id,
                           Employee_Type = g.Key.Employee_Type,
                           Is_Active = g.Key.Is_Active,
                           No_of_Employees = g.Count(x => x.b != null)
                       };

            if (isActive == true)
            {
                return data.Where(e => e.Is_Active).ToList();
            }
            else if (isActive == false)
            {
                return data.Where(e => !e.Is_Active).ToList();
            }
            else
            {
                return data.ToList();
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

        //Role

        public void AddRole(AddRoleModel addRoleModel)
        {
            var table = _timesheetContext.roles.FirstOrDefault(e => e.Role == addRoleModel.Role);

            if (table == null)
            {
                var data = new Roles();
                data.Role = addRoleModel.Role;
                data.Create_Date = DateTime.UtcNow.Date;

                _timesheetContext.roles.Add(data);
                _timesheetContext.SaveChanges();
            }
            else
            {
                throw new RoleNameException();
            }
        }

        public void EditRole(EditRoleModel editRoleModel)
        {
            var IdCheck = _timesheetContext.roles.FirstOrDefault
                 (e => (e.Role_Id == editRoleModel.Role_Id));
            var NameCheck = _timesheetContext.roles.FirstOrDefault
                 (e => (e.Role == editRoleModel.Role));

            if (NameCheck == null)
            {
                if (IdCheck != null)
                {
                    IdCheck.Role = editRoleModel.Role;
                    IdCheck.Modified_Date = DateTime.UtcNow.Date;
                    _timesheetContext.SaveChanges();
                }
                else
                {
                    throw new RoleIdException();
                }
            }
            else
            {
                throw new RoleNameException();
            }
        }

        //Employee

        public void AddEmployee(AddEmployeeModel addEmployeeModel)
        {
            var EmailContactCheck = _timesheetContext.employees.FirstOrDefault(e => e.Official_Email == addEmployeeModel.Official_Email || e.Contact_No == addEmployeeModel.Contact_No);
            var ts = new TimeSheetSummary();
            if (EmailContactCheck == null)
            {
                if (_timesheetContext.designations.FirstOrDefault(e => e.Designation_Id == addEmployeeModel.Designation_Id) != null)
                {
                    if (_timesheetContext.employeeTypes.FirstOrDefault(e => e.Employee_Type_Id == addEmployeeModel.Employee_Type_Id) != null)
                    {
                        if (_timesheetContext.employees.FirstOrDefault(e => e.Employee_code == addEmployeeModel.Employee_code) == null)
                        {
                            if (_timesheetContext.employees.FirstOrDefault(e => e.Official_Email == addEmployeeModel.Alternate_Email) == null)
                            {
                                if (_timesheetContext.employees.FirstOrDefault(e => addEmployeeModel.Official_Email == addEmployeeModel.Alternate_Email) == null)
                                {
                                    var Role = _timesheetContext.designations.FirstOrDefault(e => e.Designation_Id == addEmployeeModel.Designation_Id);

                                    var emp = new Employee();
                                    emp.First_Name = addEmployeeModel.First_Name;
                                    emp.Last_Name = addEmployeeModel.Last_Name;
                                    emp.Employee_code = addEmployeeModel.Employee_code;
                                    emp.Reporting_Manager1 = addEmployeeModel.Reporting_Manager1;
                                    emp.Official_Email = addEmployeeModel.Official_Email;
                                    emp.Alternate_Email = addEmployeeModel.Alternate_Email;
                                    emp.Contact_No = addEmployeeModel.Contact_No;
                                    emp.Password = "Joyit@Admin@1234";
                                    string passwordHash = BCrypt.Net.BCrypt.HashPassword(emp.Password);
                                    emp.Hashpassword = passwordHash;
                                    emp.Designation_Id = addEmployeeModel.Designation_Id;
                                    emp.Employee_Type_Id = addEmployeeModel.Employee_Type_Id;
                                    emp.Is_Active = true;
                                    emp.Joining_Date = addEmployeeModel.Joining_Date;
                                    emp.Create_Date = DateTime.UtcNow.Date;

                                    if (Role.Designation.ToLower() == "hr" || Role.Designation.ToLower() == "human resource" || Role.Designation.ToLower() == " admin"
                                         || Role.Designation.ToLower() == "hr manager" || Role.Designation.ToLower() == "hr admin")
                                    {
                                        emp.Role_Id = 1;
                                    }
                                    else
                                    {
                                        emp.Role_Id = 2;
                                    }

                                    _timesheetContext.employees.Add(emp);
                                    _timesheetContext.SaveChanges();
                                    var fullname = emp.First_Name + " " + emp.Last_Name;
                                    string fromAddress = "Joyitsolutions1@gmail.com";
                                    string Password = "fgrgmlzwwtokccov";
                                    string toAddress = addEmployeeModel.Official_Email;
                                    string emailHeader = "<html><body><h1>Congratulations</h1></body></html>";
                                    string emailFooter = $"<html><head><title>JoyItsolutions</title></head><body><p>Hi {fullname}, <br> This is the confidencial email Don't Shere Password with any one..! <br>Don't replay this Mail</p></body></html>";
                                    string emailBody = $"<html><head><title>Don't replay this Mail</title></head><body><p>Your password is: {emp.Password}</p></body></html>";
                                    string emailContent = emailHeader + emailBody + emailFooter;
                                    MailMessage message = new MailMessage();
                                    message.From = new MailAddress(fromAddress);
                                    message.Subject = "WellCome To Joy Family";
                                    message.To.Add(new MailAddress(toAddress));
                                    message.Body = emailContent;
                                    message.IsBodyHtml = true;

                                    var smtpClient = new System.Net.Mail.SmtpClient("smtp.gmail.com")
                                    {
                                        Port = 587,
                                        Credentials = new NetworkCredential(fromAddress, Password),
                                        EnableSsl = true,
                                    };

                                    smtpClient.Send(message);
                                }
                                else
                                {
                                    throw new OEmailAEmailSameException();
                                }
                            }
                            else
                            {
                                throw new OEmailAEmailExistException();
                            }
                        }
                        else
                        {
                            throw new EmployeeCodeExistException();
                        }
                    }
                    else
                    {
                        throw new EmployeeTypeIdException();
                    }
                }
                else
                {
                    throw new DesignationIdException();
                }
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
            || e.Official_Email == editEmployeeModel.Alternate_Email || (e.Alternate_Email == editEmployeeModel.Alternate_Email && e.Alternate_Email!=null)));
            var data = new ViewPreviousChanges();
            var hrContact = _timesheetContext.hrContactInformations.FirstOrDefault(e => e.Hr_Email_Id == editEmployeeModel.Official_Email);
            var Role = _timesheetContext.designations.FirstOrDefault(e => e.Designation == editEmployeeModel.Designation);
            if (IdCheck != null)
            {
                if (_timesheetContext.employees.FirstOrDefault(e => editEmployeeModel.Official_Email == editEmployeeModel.Alternate_Email) == null)
                {
                    if (_timesheetContext.employees.FirstOrDefault(e => e.Official_Email == editEmployeeModel.Alternate_Email) == null)
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
                            data.Is_Active = IdCheck.Is_Active;
                            data.Joining_Date = IdCheck.Joining_Date;
                            data.End_Date = IdCheck.End_Date;
                            data.Modified_Date = IdCheck.Modified_Date;
                            _timesheetContext.viewPreviousChanges.Update(data);
                            _timesheetContext.SaveChanges();

                            IdCheck.Employee_Id = editEmployeeModel.Employee_Id;
                            IdCheck.First_Name = editEmployeeModel.First_Name;
                            IdCheck.Last_Name = editEmployeeModel.Last_Name;
                            IdCheck.Employee_Type_Id = editEmployeeModel.Employee_Type_Id;
                            IdCheck.Official_Email = editEmployeeModel.Official_Email;
                            IdCheck.Employee_code = editEmployeeModel.Employee_code;
                            IdCheck.Alternate_Email = editEmployeeModel.Alternate_Email;

                            if (Role.Designation.ToLower() == "hr" || Role.Designation.ToLower() == "human resource" || Role.Designation.ToLower() == " admin"
                             || Role.Designation.ToLower() == "hr manager" || Role.Designation.ToLower() == "hr admin")
                            {
                                IdCheck.Role_Id = 1;
                            }
                            else
                            {
                                IdCheck.Role_Id = 2;
                            }

                            var des = _timesheetContext.designations.FirstOrDefault(e => e.Designation == editEmployeeModel.Designation);
                            var empType = _timesheetContext.employeeTypes.FirstOrDefault(e => e.Employee_Type == editEmployeeModel.Employee_Type);

                            if (des != null)
                            {
                                IdCheck.Designation_Id = des.Designation_Id;
                            }
                            if (empType != null)
                            {
                                IdCheck.Employee_Type_Id = empType.Employee_Type_Id;
                            }

                            IdCheck.Contact_No = editEmployeeModel.Contact_No;
                            IdCheck.Reporting_Manager1 = editEmployeeModel.Reporting_Manager1;
                            IdCheck.Joining_Date = editEmployeeModel.Joining_Date;
                            IdCheck.End_Date = editEmployeeModel.End_Date;
                            IdCheck.Modified_Date = DateTime.Now.Date;
                            _timesheetContext.SaveChanges();

                            if (_timesheetContext.hrContactInformations.FirstOrDefault(e => e.Hr_Email_Id == editEmployeeModel.Official_Email) != null)
                            {
                                hrContact.First_Name = editEmployeeModel.First_Name;
                                hrContact.Last_Name = editEmployeeModel.Last_Name;
                                hrContact.Hr_Email_Id = editEmployeeModel.Official_Email;
                                hrContact.Hr_Contact_No = editEmployeeModel.Contact_No;
                                _timesheetContext.SaveChanges();
                            }
                        }
                        else
                        {
                            var e = _timesheetContext.employees.FirstOrDefault(e => e.Official_Email != editEmployeeModel.Official_Email);
                            var c = _timesheetContext.employees.FirstOrDefault(e => e.Contact_No != editEmployeeModel.Contact_No);
                            if (e.Official_Email == editEmployeeModel.Official_Email)
                            {
                                throw new EmployeeEmailExistException();
                            }
                            else if (c.Official_Email == editEmployeeModel.Official_Email)
                            {
                                throw new EmployeeContactExistException();
                            }
                        }
                    }
                    else
                    {
                        throw new OEmailAEmailExistException();
                    }
                }
                else
                {
                    throw new OEmailAEmailSameException();
                }
            }
            else
            {
                throw new EmployeeIdNotExistException();
            }
        }

        public void EditEmployeeIsActive(IsActiveModel EmployeIsActiveModel, bool Is_Active)
        {

            var records = _timesheetContext.employees.Where(a => EmployeIsActiveModel.Id.Contains(a.Employee_Id));
            if (records.Count() != 0)
            {
                foreach (var r in records)
                {
                    r.Is_Active = Is_Active;
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

        public IEnumerable<EmployeeIsActiveModel> GetEmployeeIsActive(bool? isActive)
        {

            var data = from Emp in _timesheetContext.employees
                       join Des in this._timesheetContext.designations
                       on Emp.Designation_Id equals Des.Designation_Id
                       join ty in this._timesheetContext.employeeTypes
                       on Emp.Employee_Type_Id equals ty.Employee_Type_Id
                       orderby Emp.First_Name
                       select new EmployeeIsActiveModel
                       {
                           Employee_Id = Emp.Employee_Id,
                           First_Name = Emp.First_Name,
                           Last_Name = Emp.Last_Name,
                           Full_Name = Emp.First_Name + " " + Emp.Last_Name,
                           Employee_code = Emp.Employee_code,
                           Reporting_Manager1 = Emp.Reporting_Manager1,
                           Employee_Type = ty.Employee_Type,
                           Official_Email = Emp.Official_Email,
                           Role_Id = Emp.Role_Id,
                           Designation = Des.Designation,
                           Contact_No = Emp.Contact_No,
                           Joining_Date = Emp.Joining_Date.Date,
                           End_Date = Emp.End_Date.HasValue ?
                                      Emp.End_Date.Value : DateTime.MinValue.Date,
                           Is_Active = Emp.Is_Active
                       };
            if (isActive == true)
            {
                return data.Where(e => e.Is_Active).ToList();
            }
            else if (isActive == false)
            {
                return data.Where(e => !e.Is_Active).ToList();
            }
            else
            {
                return data.ToList();
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
                           Full_Name = Emp.First_Name + " " + Emp.Last_Name,
                           Employee_code = Emp.Employee_code,
                           Reporting_Manager1 = Emp.Reporting_Manager1,
                           Employee_Type_Id = Emp.Employee_Type_Id,
                           Employee_Type = EmpTy.Employee_Type,
                           Official_Email = Emp.Official_Email,
                           Alternate_Email = Emp.Alternate_Email,

                           Designation_Id = Emp.Designation_Id,
                           Designation = Des.Designation,
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
                       where (emppro.Employee_Id == Id)
                       join emp in _timesheetContext.employees
                       on emppro.Employee_Id equals emp.Employee_Id
                       select new GetEmployeeProjectsByIdModel
                       {
                           Employee_Project_Id = emppro.Employee_Project_Id,
                           Employee_Name = emp.First_Name + " " + emp.Last_Name,
                           Project_Name = pro.Project_Name,
                           Project_Id =emppro.Project_Id,
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
            var table = _timesheetContext.employees.FirstOrDefault(a => a.Official_Email == addHrContactModel.Hr_Email_Id && a.Role_Id == 1);


            if (table != null)
            {
                var doubleentry = _timesheetContext.hrContactInformations.FirstOrDefault(a => a.Hr_Email_Id == table.Official_Email || a.Hr_Contact_No == table.Contact_No);

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
                        throw new HrContactException();
                    }
                    else
                    {
                        throw new HrMailExistException();
                        throw new HrContactException();
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
            var EmailCheck = _timesheetContext.hrContactInformations.FirstOrDefault(h => h.Hr_Contact_Id != editHrContactModel.Hr_Contact_Id && h.Hr_Email_Id == editHrContactModel.Hr_Email_Id);
            var PhoneCheck = _timesheetContext.hrContactInformations.FirstOrDefault(h => h.Hr_Contact_Id != editHrContactModel.Hr_Contact_Id && h.Hr_Contact_No == editHrContactModel.Hr_Contact_No);

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
                        throw new HrContactException();
                    }
                }
                else
                {
                    throw new HrMailExistException();
                }
            }
            else
            {
                throw new HrIdException();
            }
        }

        public void EditHrContactInfoIsActive(IsActiveModel HrContactInfoIsActiveModel, bool Is_Active)
        {
            var records = _timesheetContext.hrContactInformations.Where(a => HrContactInfoIsActiveModel.Id.Contains(a.Hr_Contact_Id));
            if (records.Count() != 0)
            {
                foreach (var r in records)
                {
                    r.Is_Active = Is_Active;
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

        public IEnumerable<HrContactInfoIsActiveModel> GetHrContactInfoIsActive(bool? isActive)
        {

            var data = from H in this._timesheetContext.hrContactInformations
                       orderby H.First_Name
                       select new HrContactInfoIsActiveModel
                       {
                           Hr_Contact_Id = H.Hr_Contact_Id,
                           Hr_Name = H.First_Name + " " + H.Last_Name,
                           Hr_Email_Id = H.Hr_Email_Id,
                           Hr_Contact_No = H.Hr_Contact_No,
                           Is_Active = H.Is_Active,
                       };

            if (isActive == true)
            {
                return data.Where(e => e.Is_Active).ToList();
            }
            else if (isActive == false)
            {
                return data.Where(e => !e.Is_Active).ToList();
            }
            else
            {
                return data.ToList();
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

        //Timesheet status

        public List<GetTimeSheetStatusModel> GetTimeSheetStatus()
        {
            var data = (from a in _timesheetContext.timeSheetSummarys
                        select new { a } into t1
                        group t1 by t1.a.Year into g
                        select new GetTimeSheetStatusModel
                        {
                            Year = g.Key
                        });
            return data.ToList();
        }

        public IEnumerable<TimeSheetStatusByYearModel> GetTimeSheetStatusByYear(int Year)
        {
            var data = from ts in _timesheetContext.timeSheetSummarys
                       join fis in _timesheetContext.Fiscal_Years
                       on ts.Fiscal_Year_ID equals fis.Fiscal_Year_ID
                       where ts.Year == Year
                       orderby fis.Month
                       select new { ts, fis } into t1
                       group t1 by new
                       {
                           t1.ts.Fiscal_Year_ID,
                           t1.fis.Month
                       } into g

                       select new TimeSheetStatusByYearModel
                       {
                           MonthID = g.Key.Fiscal_Year_ID,
                           Month = g.Key.Month,
                           TimeSheet_Count = g.Count(),
                           Pending = g.Where(t => t.ts.Status == "Pending").Count(),
                           Approved = g.Where(t => t.ts.Status == "Approved").Count(),
                           Rejected = g.Where(t => t.ts.Status == "Rejected").Count()
                       };
            return data.ToList();
        }

            public IEnumerable<EmployeeTimeSheetByMonthModel> GetTimeSheetStatusByMonth(int month_id, int year)                      //GETTIMESHEETBY MONTH
        {
            var data = from emp in _timesheetContext.employees
                       join ts in _timesheetContext.timeSheetSummarys
                       on emp.Employee_Id equals ts.Employee_Id
                       where (ts.Fiscal_Year_ID == month_id && ts.Year == year)
                       join emptyp in _timesheetContext.employeeTypes
                       on emp.Employee_Type_Id equals emptyp.Employee_Type_Id
                       select new EmployeeTimeSheetByMonthModel
                       {
                           Employee_Id = emp.Employee_Id,
                           Employee_Type = emptyp.Employee_Type,
                           Full_Name = emp.First_Name + " " + emp.Last_Name,
                           EmailId = emp.Official_Email,
                           NoOfDaysWorked = ts.No_Of_days_Worked,
                           NoOfLeaveTaken = ts.No_Of_Leave_Taken,
                           Total_Hours = ts.Total_Working_Hours,
                           Reporting_Manager = emp.Reporting_Manager1,
                           Status = ts.Status,
                           ImagePathUpload = ts.ImagePathUpload,
                           ImagePathTimesheet = ts.ImagePathTimesheet
                       };
            return data.ToList();
        }

        public IEnumerable<GetTimesheetSummaryMonthYearEmployeeModel> GetTimesheetSummaryMonthYearEmployee(int Month_id, int Year_id, int Employee_Id)
        {
            var data = from ts in _timesheetContext.timeSheetSummarys
                       join t in _timesheetContext.timeSheets
                       on ts.TimesheetSummary_Id equals t.TimesheetSummary_Id
                       join pro in _timesheetContext.projects
                       on t.Project_Id equals pro.Project_Id
                       where ts.Year == Year_id &&
                       ts.Fiscal_Year_ID == Month_id &&
                       ts.Employee_Id == Employee_Id

                       select new GetTimesheetSummaryMonthYearEmployeeModel
                       {
                           Day = t.Day,
                           Date = t.Date.ToString(),
                           Status = t.Leave.ToString(),
                           Project = pro.Project_Name,
                           Duration = t.Duration_in_Hours.ToString()
                       };
            return data.ToList();
        }

        //Edit Approve
        public void EditTimesheetStatus(EditTimeSheetStatusModel editTimeSheetStatusModel)
        {
            foreach (var b in editTimeSheetStatusModel.EditTimeSheetModelById)
            {
                var timeSlot = _timesheetContext.timeSheetSummarys.FirstOrDefault
                (e => (e.Employee_Id == b.Employee_id && e.Year == b.Year && e.Fiscal_Year_ID == b.Month_Id));
                //if (timeSlot != null)
                // {
                timeSlot.Status = editTimeSheetStatusModel.Timesheet_Status;
                _timesheetContext.SaveChanges();
                //}
            }
        }


            //View Previous changes

        public IEnumerable<ViewPreviousChangesModel> GetViewPreviousChanges()
        {
            var data = from emp in _timesheetContext.viewPreviousChanges
                       join des in _timesheetContext.designations
                       on emp.Designation_Id equals des.Designation_Id
                       join emptyp in _timesheetContext.employeeTypes
                       on emp.Employee_Type_Id equals emptyp.Employee_Type_Id
                       where (emp.Modified_Date != null
                       || des.Modified_Date != null
                       || emptyp.Modified_Date != null)
                       select new ViewPreviousChangesModel
                       {
                           Employee_Id = emp.Employee_Id,
                           Full_Name = emp.First_Name + " " + emp.Last_Name,
                           Employee_Type = emptyp.Employee_Type,
                           EmailId = emp.Email,
                           Joining_Date = emp.Joining_Date.ToString("dd/MM/yyyy"),
                           Modified_Date = emp.Modified_Date.HasValue ?
                            emp.Modified_Date.Value.ToString("dd/MM/yyyy") : string.Empty,
                           Designation = des.Designation,
                           Contact_No = emp.Contact_No,
                           Reporting_Manager = emp.Reporting_Manager1
                       };
            return data.ToList();
        }

        public IEnumerable<ViewPreviousChangesByIdModel> GetViewPreviousChangesById(int Id)
        {
            var view = from v in this._timesheetContext.viewPreviousChanges
                       join typ in this._timesheetContext.employeeTypes
                       on v.Employee_Type_Id equals typ.Employee_Type_Id
                       join des in this._timesheetContext.designations
                       on v.Designation_Id equals des.Designation_Id
                       where (v.Employee_Id == Id)
                       orderby v.Modified_Date

                       select new ViewPreviousChangesByIdModel
                       {
                           Full_Name = v.First_Name + " " + v.Last_Name,
                           EmailId = v.Email,
                           Employee_Id = v.Employee_Id,
                           Employee_Type = typ.Employee_Type,
                           Reporting_Manager1 = v.Reporting_Manager1,
                           Contact_No = v.Contact_No,
                           Designation = des.Designation,
                           Joining_Date = v.Joining_Date.Date,
                           Modified_Date = v.Modified_Date.HasValue ? v.Modified_Date.Value.Date : DateTime.MinValue,
                           End_Date = v.End_Date.HasValue ? v.End_Date.Value.Date : DateTime.MinValue
                       };
            return view.ToList();
        }

        //ExportExcelEditTimesheetStatusByMonthModel

        public string ExcelEditTimesheetStatusByMonth(int Year, int Month_id)
        {
            var data = from emp in _timesheetContext.employees
                       join ts in _timesheetContext.timeSheetSummarys
                       on emp.Employee_Id equals ts.Employee_Id
                       join emptyp in _timesheetContext.employeeTypes
                       on emp.Employee_Type_Id equals emptyp.Employee_Type_Id
                       join fis in _timesheetContext.Fiscal_Years
                       on ts.Fiscal_Year_ID equals fis.Fiscal_Year_ID
                       where ts.Fiscal_Year_ID == Month_id && ts.Year == Year
                       select new ExcelEditTimesheetStatusByMonthModel
                       {
                           EmployeeId = emp.Employee_Id,
                           EmployeeType = emptyp.Employee_Type,
                           EmployeeName = emp.First_Name + " " + emp.Last_Name,
                           EmailId = emp.Official_Email,
                           Month = fis.Month,
                           Year = ts.Year,
                           NoOfDaysWorked = ts.No_Of_days_Worked,
                           NoOfLeaveTaken = ts.No_Of_Leave_Taken,
                           TotalHours = ts.Total_Working_Hours,
                           Status = ts.Status,
                           ReportingManager = emp.Reporting_Manager1
                       };

            return CreateExcelDoc(data.ToList());

        }

        public string CreateExcelDoc(List<ExcelEditTimesheetStatusByMonthModel> timeSheetData)
        {
            string fileName = Path.Combine(Path.GetTempPath(), $"TimeSheet of {timeSheetData[0].Month + "-" + timeSheetData[0].Year}" + ".xlsx");
            //Directory.CreateDirectory(fileName);
            using (SpreadsheetDocument document = SpreadsheetDocument.Create(fileName, SpreadsheetDocumentType.Workbook))
            {
                WorkbookPart workbookPart = document.AddWorkbookPart();
                workbookPart.Workbook = new Workbook();

                WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                worksheetPart.Worksheet = new Worksheet();

                // Adding style
                WorkbookStylesPart stylePart = workbookPart.AddNewPart<WorkbookStylesPart>();
                stylePart.Stylesheet = GenerateStylesheet();
                stylePart.Stylesheet.Save();

                // Setting up columns
                Sheets sheets = workbookPart.Workbook.AppendChild(new Sheets());
                Sheet sheet = new Sheet() { Id = workbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = "excel_edit_timesheet_status_by_month" };
                sheets.Append(sheet);
                workbookPart.Workbook.Save();

                // Define column widths
                
                Columns columns = new Columns();
                
                Column column1 = new Column() { Min = 1, Max = 1, Width = 25}; // Set width of column A to 20
                columns.Append(column1);
                Column column2 = new Column() { Min = 2, Max = 2, Width = 14 }; // Set width of column A to 20
                columns.Append(column2);

                Column column3 = new Column() { Min = 3, Max = 3, Width = 27 }; // Set width of column A to 20
                columns.Append(column3);

                for (int i = 4; i <= 6; i++)
                {
                    Column column4_6 = new Column() { Min = (uint)i, Max = (uint)i, Width = 18 }; // Set width of column A to 20
                    columns.Append(column4_6);
                }

                for (int i = 7; i <= 9; i++)
                {
                    Column column7_9 = new Column() { Min = (uint)i, Max = (uint)i, Width = 11 }; // Set width of column A to 20
                    columns.Append(column7_9);
                }

                worksheetPart.Worksheet.Append(columns);


                SheetData sheetData = worksheetPart.Worksheet.AppendChild(new SheetData());

                var firstRow = new Row();
                firstRow.Append(
                     ConstructCell($"TimeSheet for {timeSheetData[0].Month + "-" + timeSheetData[0].Year}", CellValues.String, 1)
                      );

                sheetData.Append(firstRow);

                var secondRow = new Row();
                sheetData.Append(secondRow);

                var thirdRow = new Row();
                thirdRow.Append(
                    ConstructCell("Employee Name", CellValues.String, 1),                   
                    ConstructCell("Employee Type", CellValues.String, 1),
                    ConstructCell("Official Email", CellValues.String, 1),
                    ConstructCell("Reporting Manager", CellValues.String, 1),
                    ConstructCell("No Of Days Worked", CellValues.String, 1),
                    ConstructCell("No Of Leave Taken", CellValues.String, 1),
                    ConstructCell("Total Hours", CellValues.String, 1),
                    ConstructCell("Status", CellValues.String, 1)
                    );
                sheetData.AppendChild(thirdRow);

                foreach (var sheetdata in timeSheetData)
                {
                    var row = new Row();
                    row.Append(
                        ConstructCell(sheetdata.EmployeeName, CellValues.String, 1),
                        ConstructCell(sheetdata.EmployeeType, CellValues.String, 1),
                        ConstructCell(sheetdata.EmailId, CellValues.String, 1),
                        ConstructCell(sheetdata.ReportingManager, CellValues.String, 1),
                        ConstructCell(sheetdata.NoOfDaysWorked, CellValues.Number, 1),
                        ConstructCell(sheetdata.NoOfLeaveTaken, CellValues.Number, 1),
                        ConstructCell(sheetdata.TotalHours, CellValues.Number, 1),
                        ConstructCell(sheetdata.Status, CellValues.String, 1)
                        );

                    if (sheetdata.Status.ToLower() == "approved")
                    {
                        row.Append(
                        ConstructCell(sheetdata.Status, CellValues.String, 2));
                    }
                    else if (sheetdata.Status.ToLower() == "rejected")
                    {
                        row.Append(
                        ConstructCell(sheetdata.Status, CellValues.String, 3));
                    }
                    else
                    {
                        row.Append(
                        ConstructCell(sheetdata.Status, CellValues.String, 4));
                    }
                    sheetData.AppendChild(row);
                }

                worksheetPart.Worksheet.Save();
                return fileName;
            }
        }

        private Cell ConstructCell(dynamic value, CellValues dataType, uint styleIndex = 0)
        {
            return new Cell()
            {
                CellValue = new CellValue(value),
                DataType = new EnumValue<CellValues>(dataType),
                StyleIndex = styleIndex
            };
        }

        private Stylesheet GenerateStylesheet()
        {
            Stylesheet styleSheet = null;
            
            Fills fills = new Fills(
                    new Fill(new PatternFill() { PatternType = PatternValues.None }), // Index 0 - default
                    new Fill(new PatternFill() { PatternType = PatternValues.Gray125 }), // Index 1 - default
                    new Fill(new PatternFill(new ForegroundColor { Rgb = new HexBinaryValue() { Value = "99ff00" } })
                    { PatternType = PatternValues.Solid }),
                    new Fill(new PatternFill(new ForegroundColor { Rgb = new HexBinaryValue() { Value = "ff0900" } })
                    { PatternType = PatternValues.Solid }),
                    new Fill(new PatternFill(new ForegroundColor { Rgb = new HexBinaryValue() { Value = "00ffc4" } })
                    { PatternType = PatternValues.Solid })// Index 2 - header
                );

            Borders borders = new Borders(
                    new Border(), // index 0 default
                    new Border( // index 1 black border
                        new LeftBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                        new RightBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                        new TopBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                        new BottomBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                        new DiagonalBorder()),
                     new Border( // index 1 black border
                        new LeftBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                        new RightBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                        new TopBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                        new BottomBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                        new DiagonalBorder()),
                      new Border( // index 1 black border
                        new LeftBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                        new RightBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                        new TopBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                        new BottomBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                        new DiagonalBorder())
                );

            CellFormats cellFormats = new CellFormats(
                    new CellFormat(), // default
                    new CellFormat { FontId = 0, FillId = 0, BorderId = 1, ApplyBorder = true, Alignment = new Alignment() { Horizontal = HorizontalAlignmentValues.Center } }, // body
                    new CellFormat { FontId = 1, FillId = 2, BorderId = 1, ApplyFill = true, Alignment = new Alignment() { Horizontal = HorizontalAlignmentValues.Center } },
                    new CellFormat { FontId = 1, FillId = 3, BorderId = 1, ApplyFill = true, Alignment = new Alignment() { Horizontal = HorizontalAlignmentValues.Center } },
                    new CellFormat { FontId = 1, FillId = 4, BorderId = 1, ApplyFill = true, Alignment = new Alignment() { Horizontal = HorizontalAlignmentValues.Center } }// header
                );

            styleSheet = new Stylesheet( fills, borders, cellFormats);
            return styleSheet;
        }
    }
}
