using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Joy.TS.DAL.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Joy.TS.BAL.DomainModel.AdminDomainModel;
using static Joy.TS.BAL.DomainModel.EmployeeDomainModel;

namespace Joy.TS.BAL.Implementation
{
    public interface EmployeeInterface
    {
        public IEnumerable<GetTimeSheetByIdModel> GetTimeSheetById(int id);
        public IEnumerable<DomainModel.EmployeeDomainModel.GetDashboardModel> GetByDashboard(int id);
        public IActionResult AddTimeSheet_Summary(AddTimeSheet_SummaryModel AddTimeSheet_SummaryModel);
        
        public Task<IActionResult> UploadImage(IFormFile image); 
        public string ExportExcel(int id, int monthid, int year, int project_id);
        public IActionResult GetImage(string imagePath);
         public List<TimesheetsummaryModel> GetAllTimeSheet_Summary(int Employee_Id, int year);
        public IEnumerable<UserProfileModel> GetUserProfile(string mail_id);
        public IActionResult Fiscal_Years(Fiscal_Year fiscal_Year);
        public IActionResult ImageUpdate(ImageUpdate imageUpdate);
    }
}
