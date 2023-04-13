using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Joy.TS.DAL.Model;
using Microsoft.AspNetCore.Mvc;
using static Joy.TS.BAL.DomainModel.EmployeeDomainModel;

namespace Joy.TS.BAL.Implementation
{
    public interface EmployeeInterface
    {
        public IEnumerable<GetTimeSheetByIdModel> GetTimeSheetById(int id);
        public IEnumerable<GetDashboardModel> GetByDashboard(int id);
        public string AddTimeSheet_Summary(AddTimeSheet_SummaryModel AddTimeSheet_SummaryModel);
    }
}
