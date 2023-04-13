using Joy.TS.DAL.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Joy.TS.DAL.Data
{
    public class TimeSheetContext : DbContext
    {
        public TimeSheetContext(DbContextOptions<TimeSheetContext> options) : base(options) { }

        public DbSet<Client> clients { get; set; }
        public DbSet<Designations> designations { get; set; }
        public DbSet<Employee> employees { get; set; }
        public DbSet<EmployeeProject> employeeProject { get; set; }
        public DbSet<EmployeeType> employeeTypes { get; set; }
        public DbSet<HrContactInformation> hrContactInformations { get; set; }
        public DbSet<Projects> projects { get; set; }
        public DbSet<Roles> roles { get; set; }
        public DbSet<TimeSheets> timeSheets { get; set; }
        public DbSet<TimeSheetSummary> timeSheetSummarys { get; set; }
        public DbSet<ViewPreviousChanges> viewPreviousChanges { get; set; }
    }
}
