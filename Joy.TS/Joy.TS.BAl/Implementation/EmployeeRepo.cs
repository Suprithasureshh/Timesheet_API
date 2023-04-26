using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Joy.TS.DAL.Data;
using Joy.TS.DAL.Model;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Joy.TS.BAL.DomainModel.AdminDomainModel;
using static Joy.TS.BAL.DomainModel.EmployeeDomainModel;

namespace Joy.TS.BAL.Implementation
{
    public class EmployeeRepo : ControllerBase, EmployeeInterface
    {
        private readonly TimeSheetContext _timesheetContext;
        private readonly IWebHostEnvironment _hostEnvironment;


        public EmployeeRepo(TimeSheetContext timesheetContext, IWebHostEnvironment hostEnvironment)
        {
            _timesheetContext = timesheetContext;
            _hostEnvironment = hostEnvironment;
        }

        //DashBoard
        public IEnumerable<DomainModel.EmployeeDomainModel.GetDashboardModel> GetByDashboard(int id)
        {
            var e1 = _timesheetContext.timeSheetSummarys.Max(s => s.Year);
            var t = DateTime.Now.Month - 1;

            var dash = from d in this._timesheetContext.timeSheetSummarys
                       join f in _timesheetContext.Fiscal_Years
                       on d.Fiscal_Year_ID equals f.Fiscal_Year_ID
                       where (d.Year == e1 && d.Employee_Id == id && d.Fiscal_Year_ID == t)
                       select new DomainModel.EmployeeDomainModel.GetDashboardModel
                       {
                           Status = d.Status,
                           Month = f.Month,
                           Year = e1
                       };
            return dash.ToList();
        }

        // Add Timesheetsummary
        public string AddTimeSheet_Summary(AddTimeSheet_SummaryModel AddTimeSheet_SummaryModel)
        {
            var TS = new TimeSheetSummary();
            TS.Employee_Id = AddTimeSheet_SummaryModel.Employee_Id;
            TS.Fiscal_Year_ID = DateTime.Now.Month - 1;
            TS.Year = DateTime.Now.Year;
            TS.No_Of_days_Worked = AddTimeSheet_SummaryModel.NoOfdays_Worked;
            TS.No_Of_Leave_Taken = AddTimeSheet_SummaryModel.NoOfLeave_Taken;
            TS.Total_Working_Hours = AddTimeSheet_SummaryModel.Total_Working_Hours;
            TS.ImagePathUpload = AddTimeSheet_SummaryModel.ImagePathUpload;
            TS.ImagePathTimesheet = AddTimeSheet_SummaryModel.ImagePathTimesheet;
            TS.Created_Date = DateTime.Now.Date;
            TS.Status = "Pending";

            _timesheetContext.timeSheetSummarys.Add(TS);
            _timesheetContext.SaveChanges();
            int lastsummaryid = _timesheetContext.timeSheetSummarys.Max(item => item.TimesheetSummary_Id);


            foreach (var s in AddTimeSheet_SummaryModel.addTimesheetDay)
            {
               // var M = _timesheetContext.projects.FirstOrDefault(i => i.Project_Name == s.Project_Id);

                var T = new TimeSheets();
                T.Project_Id = s.Project_Id;
                T.Leave = s.Leave;
                T.Date = s.Date;
                T.Day = s.Day;
                T.Employee_Id = AddTimeSheet_SummaryModel.Employee_Id;
                T.Duration_in_Hours = s.Duration_in_Hrs;
                T.TimesheetSummary_Id = lastsummaryid;
                _timesheetContext.timeSheets.Add(T);
                _timesheetContext.SaveChanges();
            }
            return "Employee TimeSheet Added...!";
        }


        //Get timesheetsummary
        public IEnumerable<GetTimeSheetByIdModel> GetTimeSheetById(int id)
        {
            var res = new TimeSheets();
            var data = from a in _timesheetContext.timeSheetSummarys
                       join b in _timesheetContext.timeSheets
                       on a.TimesheetSummary_Id equals b.TimesheetSummary_Id
                       where (b.Employee_Id == id)
                       select new GetTimeSheetByIdModel
                       {
                           Date = b.Date,
                           Fiscal_Year_ID = a.Fiscal_Year_ID,
                           Year = a.Year,
                           Day = b.Day,
                           Employee_Id = b.Employee_Id,
                           Project_Id = b.Project_Id,
                           Leave = b.Leave,
                           NoOfdays_Worked = a.No_Of_days_Worked,
                           NoOfLeave_Taken = a.No_Of_days_Worked,
                           Total_Working_Hours = a.Total_Working_Hours
                       };
            return data.ToList();
        }
        //Add Fiscal_Years
        public IActionResult Fiscal_Years(Fiscal_Year fiscal_Year)
        {
            _timesheetContext.Fiscal_Years.Add(fiscal_Year);
            _timesheetContext.SaveChanges();
            return Ok("Fiscal Years Added");
        }
        //Get All timesheetsummary
        public List<TimesheetsummaryModel> GetAllTimeSheet_Summary(int Employee_Id, int year)
        {
            var data = from ts in this._timesheetContext.timeSheetSummarys
                       join fis in this._timesheetContext.Fiscal_Years
                       on ts.Fiscal_Year_ID equals fis.Fiscal_Year_ID
                       where ts.Employee_Id == Employee_Id && ts.Year == year

                       select new TimesheetsummaryModel
                       {
                           TimesheetSummary_Id = ts.TimesheetSummary_Id,
                           Employee_Id = ts.Employee_Id,
                           Fiscal_Year_ID = ts.Fiscal_Year_ID,
                           Month = fis.Month,
                           Year = ts.Year,
                           ImagePathTimesheet = ts.ImagePathTimesheet,
                           ImagePathUpload = ts.ImagePathUpload,
                           NoOfdays_Worked = ts.No_Of_days_Worked,
                           NoOfLeave_Taken = ts.No_Of_Leave_Taken,
                           Total_Working_Hours = ts.Total_Working_Hours,
                           Status = ts.Status,

                       };
            return data.ToList();
        }


        //Userprofile
        public IEnumerable<UserProfileModel> GetUserProfile(string mail_id)
        {

            var query = from hr in this._timesheetContext.employees
                        join des in this._timesheetContext.designations
                        on hr.Designation_Id equals des.Designation_Id
                        where hr.Official_Email == mail_id
                        select new UserProfileModel
                        {
                            Employee_ID = hr.Employee_Id,
                            Employee_Name = hr.First_Name + " " + hr.Last_Name,
                            Designation = des.Designation,
                            Email = hr.Official_Email,
                            Mobile_No = hr.Contact_No,
                            Joining_Date = hr.Joining_Date.ToString("dd/MM/yyyy")

                        };
            return query;
        }

        //Image Update in TimesheetSummary
        public IActionResult ImageUpdate(ImageUpdate imageUpdate)
        {
            var TS = new TimeSheetSummary();
            var lastsummaryid = _timesheetContext.timeSheetSummarys.FirstOrDefault(item => item.Fiscal_Year_ID == DateTime.Now.Month - 1);
            if(lastsummaryid != null)
            {
                TS.Status = "Pending";
                TS.ImagePathUpload = imageUpdate.ImagePathUpload;
                TS.ImagePathTimesheet = imageUpdate.ImagePathTimesheet;
                _timesheetContext.timeSheetSummarys.Update(TS);
                _timesheetContext.SaveChanges();
                return Ok("Updated");
            }
            return NotFound();
           
        }
        //Upload Images
        public async Task<IActionResult> UploadImage(IFormFile image)
        {
            if (image == null || image.Length == 0)
            {
                return BadRequest("Image file is not selected");
            }
            var folderPath = Path.Combine(_hostEnvironment.ContentRootPath, "Images");
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
            var filePath = Path.Combine(folderPath, fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }

            var imagePath = Path.Combine("Images", fileName);
            return Ok(new { imagePath });
        }


        //Upload Images Path
        public IActionResult GetImage(string imagePath)
        {
            var fullPath = Path.Combine(_hostEnvironment.ContentRootPath, imagePath);
            var imageBytes = System.IO.File.ReadAllBytes(fullPath);
            return File(imageBytes, "image/jpeg");
        }


        //Export to excel
        public string ExportExcel(int id, int monthid, int year, int project_id)
        {
            var data = from tsum in this._timesheetContext.timeSheetSummarys
                       join t in this._timesheetContext.timeSheets
                       on tsum.TimesheetSummary_Id equals t.TimesheetSummary_Id
                       join emp in this._timesheetContext.employees
                       on t.Employee_Id equals emp.Employee_Id
                       where (emp.Employee_Id == 92)
                       join pro in this._timesheetContext.projects
                       on t.Project_Id equals pro.Project_Id
                       join des in this._timesheetContext.designations
                       on emp.Designation_Id equals des.Designation_Id

                       select new ExcelTimeSheetModel
                       {
                           Employee_Id = emp.Employee_Id,
                           Employee_Name = emp.First_Name + " " + emp.Last_Name,
                           Reporting_Manager1 = emp.Reporting_Manager1,
                           Project_Name = pro.Project_Name,
                           NoOfdays_Worked = tsum.No_Of_days_Worked.ToString(),
                           NoOfLeave_Taken = tsum.No_Of_Leave_Taken.ToString(),
                           Total_Working_Hours = tsum.Total_Working_Hours.ToString(),
                           Date = t.Date.HasValue ?
                                   t.Date.Value.ToString("dd") : ToString(),
                           Day = t.Day,
                           Duration_in_Hrs = t.Duration_in_Hours.ToString(),
                           Year = tsum.Year,
                           TimesheetSummary_Id = t.TimesheetSummary_Id,
                           Leave = t.Leave,
                           Designation_Name = des.Designation,
                           status = t.Leave == true ? "L" : "P"
                       };

            if (data.ToList().Count > 0)
            {
                return CreateExcelDoc(data.ToList());
            }
            else
            {
                throw new Exception($"there is no data available in {id} ");
            }

        }
        public string CreateExcelDoc(List<ExcelTimeSheetModel> timeSheetData)
        {
            var months = _timesheetContext.Fiscal_Years.FirstOrDefault(e => e.Fiscal_Year_ID == DateTime.UtcNow.Month);
            var y1 = timeSheetData[0].Project_Name;
            string fileName = Path.Combine(Path.GetTempPath(), "Timesheet" + " " + months.Month + " " + timeSheetData[0].Employee_Name + ".xlsx");
            //Directory.CreateDirectory(fileName);
            using (SpreadsheetDocument document = SpreadsheetDocument.Create(fileName, SpreadsheetDocumentType.Workbook))
            {
                MergeCells mergeCells = new MergeCells();
                Border border = new Border();
                //border.Append(new Border() { BottomBorder=false});
                WorkbookPart workbookPart = document.AddWorkbookPart();
                workbookPart.Workbook = new Workbook();

                WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                worksheetPart.Worksheet = new Worksheet();

                // Adding style
                WorkbookStylesPart stylePart = workbookPart.AddNewPart<WorkbookStylesPart>();
                stylePart.Stylesheet = GenerateStylesheet();
                stylePart.Stylesheet.Save();

                Columns lstColumns = worksheetPart.Worksheet.GetFirstChild<Columns>();

                Boolean needToInsertColumns = false;
                if (lstColumns == null)
                {
                    lstColumns = new Columns();
                    needToInsertColumns = true;
                }

                lstColumns.Append(new Column() { Min = 1, Max = 1, Width = 15, CustomWidth = true });
                lstColumns.Append(new Column() { Min = 2, Max = 2, Width = 10, CustomWidth = true });
                lstColumns.Append(new Column() { Min = 3, Max = 3, Width = 10, CustomWidth = true });
                lstColumns.Append(new Column() { Min = 4, Max = 4, Width = 10, CustomWidth = true });
                lstColumns.Append(new Column() { Min = 5, Max = 5, Width = 10, CustomWidth = true });
                lstColumns.Append(new Column() { Min = 6, Max = 6, Width = 10, CustomWidth = true });
                lstColumns.Append(new Column() { Min = 7, Max = 7, Width = 10, CustomWidth = true });
                lstColumns.Append(new Column() { Min = 8, Max = 8, Width = 10, CustomWidth = true });
                lstColumns.Append(new Column() { Min = 9, Max = 9, Width = 10, CustomWidth = true });
                lstColumns.Append(new Column() { Min = 10, Max = 10, Width = 10, CustomWidth = true });
                lstColumns.Append(new Column() { Min = 11, Max = 11, Width = 10, CustomWidth = true });
                lstColumns.Append(new Column() { Min = 12, Max = 12, Width = 10, CustomWidth = true });
                lstColumns.Append(new Column() { Min = 13, Max = 13, Width = 10, CustomWidth = true });
                lstColumns.Append(new Column() { Min = 14, Max = 14, Width = 10, CustomWidth = true });
                lstColumns.Append(new Column() { Min = 15, Max = 15, Width = 10, CustomWidth = true });
                lstColumns.Append(new Column() { Min = 16, Max = 16, Width = 10, CustomWidth = true });
                lstColumns.Append(new Column() { Min = 17, Max = 17, Width = 10, CustomWidth = true });
                lstColumns.Append(new Column() { Min = 18, Max = 18, Width = 10, CustomWidth = true });
                lstColumns.Append(new Column() { Min = 19, Max = 19, Width = 10, CustomWidth = true });
                lstColumns.Append(new Column() { Min = 20, Max = 20, Width = 10, CustomWidth = true });
                lstColumns.Append(new Column() { Min = 21, Max = 21, Width = 10, CustomWidth = true });

                if (needToInsertColumns)
                    worksheetPart.Worksheet.InsertAt(lstColumns, 0);


                SheetData sheetData1 = worksheetPart.Worksheet.GetFirstChild<SheetData>();



                // Setting up columns
                Sheets sheets = workbookPart.Workbook.AppendChild(new Sheets());
                Sheet sheet = new Sheet() { Id = workbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = "Time Sheet" };
                sheets.Append(sheet);
                workbookPart.Workbook.Save();
                SheetData sheetData = worksheetPart.Worksheet.AppendChild(new SheetData());


                mergeCells.Append(new MergeCell() { Reference = new StringValue("A1:R1") });
                var firstRow = new Row();
                firstRow.Append(



                    ConstructCell($" TimeSheet for {DateTime.Today.ToString("MMM - yy")}", CellValues.String, 1),
                    ConstructCell(string.Empty, CellValues.String, 1),
                    ConstructCell(string.Empty, CellValues.String, 1),
                    ConstructCell(string.Empty, CellValues.String, 1),
                    ConstructCell(string.Empty, CellValues.String, 1),
                    ConstructCell(string.Empty, CellValues.String, 1),
                    ConstructCell(string.Empty, CellValues.String, 1),
                    ConstructCell(string.Empty, CellValues.String, 1),
                    ConstructCell(string.Empty, CellValues.String, 1),
                    ConstructCell(string.Empty, CellValues.String, 1),
                    ConstructCell(string.Empty, CellValues.String, 1),
                    ConstructCell(string.Empty, CellValues.String, 1),
                    ConstructCell(string.Empty, CellValues.String, 1),
                    ConstructCell(string.Empty, CellValues.String, 1),
                    ConstructCell(string.Empty, CellValues.String, 1),
                    ConstructCell(string.Empty, CellValues.String, 1),
                    ConstructCell(string.Empty, CellValues.String, 1),
                    ConstructCell(string.Empty, CellValues.String, 1)

                    );

                sheetData.AppendChild(firstRow);

                var secondRow = new Row();
                sheetData.AppendChild(secondRow);
                Width w = new Width();

                mergeCells.Append(new MergeCell() { Reference = new StringValue("A3:C3") });
                mergeCells.Append(new MergeCell() { Reference = new StringValue("D3:H3") });
                mergeCells.Append(new MergeCell() { Reference = new StringValue("J3:L3") });
                mergeCells.Append(new MergeCell() { Reference = new StringValue("M3:Q3") });
                var thirdRow = new Row();
                thirdRow.Append(
                ConstructCell("Date", CellValues.String, 1),
                    ConstructCell(string.Empty, CellValues.String, 1),
                    ConstructCell(string.Empty, CellValues.String, 1),
                    ConstructCell(DateTime.Today.ToString("dd-MMM-yyyy"), CellValues.String, 1),
                    ConstructCell(string.Empty, CellValues.String, 1),
                    ConstructCell(string.Empty, CellValues.String, 1),
                    ConstructCell(string.Empty, CellValues.String, 1),
                    ConstructCell(string.Empty, CellValues.String, 1),
                    ConstructCell(string.Empty, CellValues.String, 0),
                    ConstructCell("project_Name", CellValues.String, 1),
                    ConstructCell(string.Empty, CellValues.String, 1),
                    ConstructCell(string.Empty, CellValues.String, 1),
                    ConstructCell(timeSheetData[0].Project_Name, CellValues.String, 1),
                    ConstructCell(string.Empty, CellValues.String, 1),
                    ConstructCell(string.Empty, CellValues.String, 1),
                    ConstructCell(string.Empty, CellValues.String, 1),
                    ConstructCell(string.Empty, CellValues.String, 1)

                    );
                sheetData.AppendChild(thirdRow);

                mergeCells.Append(new MergeCell() { Reference = new StringValue("A4:C4") });
                mergeCells.Append(new MergeCell() { Reference = new StringValue("D4:H4") });
                mergeCells.Append(new MergeCell() { Reference = new StringValue("J4:L4") });
                mergeCells.Append(new MergeCell() { Reference = new StringValue("M4:Q4") });
                var fourthRow = new Row();
                fourthRow.Append(
                    ConstructCell("Employee_Name", CellValues.String, 1),
                    ConstructCell(string.Empty, CellValues.String, 1),
                    ConstructCell(string.Empty, CellValues.String, 1),
                    ConstructCell(timeSheetData[0].Employee_Name, CellValues.String, 1),
                    ConstructCell(string.Empty, CellValues.String, 1),
                    ConstructCell(string.Empty, CellValues.String, 1),
                    ConstructCell(string.Empty, CellValues.String, 1),
                    ConstructCell(string.Empty, CellValues.String, 1),
                    ConstructCell(string.Empty, CellValues.String, 0),
                    ConstructCell("Reporting_Manager1", CellValues.String, 1),
                    ConstructCell(string.Empty, CellValues.String, 1),
                    ConstructCell(string.Empty, CellValues.String, 1),
                    ConstructCell(timeSheetData[0].Reporting_Manager1, CellValues.String, 1),
                    ConstructCell(string.Empty, CellValues.String, 1),
                    ConstructCell(string.Empty, CellValues.String, 1),
                    ConstructCell(string.Empty, CellValues.String, 1),
                    ConstructCell(string.Empty, CellValues.String, 1)

                    );
                sheetData.AppendChild(fourthRow);

                mergeCells.Append(new MergeCell() { Reference = new StringValue("A5:C5") });
                mergeCells.Append(new MergeCell() { Reference = new StringValue("D5:H5") });
                mergeCells.Append(new MergeCell() { Reference = new StringValue("J5:L5") });
                mergeCells.Append(new MergeCell() { Reference = new StringValue("M5:Q5") });
                var fifthRow = new Row();
                fifthRow.Append(

                    ConstructCell("Time Sheet for Month ", CellValues.String, 1),
                    ConstructCell(string.Empty, CellValues.String, 1),
                    ConstructCell(string.Empty, CellValues.String, 1),
                    ConstructCell(DateTime.Today.ToString("MMM-yy"), CellValues.String, 1),
                    ConstructCell(string.Empty, CellValues.String, 1),
                    ConstructCell(string.Empty, CellValues.String, 1),
                    ConstructCell(string.Empty, CellValues.String, 1),
                    ConstructCell(string.Empty, CellValues.String, 1),
                    ConstructCell(string.Empty, CellValues.String, 0),
                    ConstructCell("NoOfLeave_Taken", CellValues.String, 1),
                    ConstructCell(string.Empty, CellValues.String, 1),
                    ConstructCell(string.Empty, CellValues.String, 1),
                    ConstructCell(timeSheetData[0].NoOfLeave_Taken.ToString(), CellValues.Number, 1),
                    ConstructCell(string.Empty, CellValues.String, 1),
                    ConstructCell(string.Empty, CellValues.String, 1),
                    ConstructCell(string.Empty, CellValues.String, 1),
                    ConstructCell(string.Empty, CellValues.String, 1)
                    );
                sheetData.AppendChild(fifthRow);
                mergeCells.Append(new MergeCell() { Reference = new StringValue("A6:C6") });
                mergeCells.Append(new MergeCell() { Reference = new StringValue("D6:H6") });
                mergeCells.Append(new MergeCell() { Reference = new StringValue("J6:L6") });
                mergeCells.Append(new MergeCell() { Reference = new StringValue("M6:Q6") });



                var sixthRow = new Row();
                sixthRow.Append(
                    ConstructCell("NoOfdays_Worked", CellValues.String, 1),
                    ConstructCell(string.Empty, CellValues.String, 1),
                    ConstructCell(string.Empty, CellValues.String, 1),
                    ConstructCell(timeSheetData[0].NoOfdays_Worked.ToString(), CellValues.Number, 1),
                    ConstructCell(string.Empty, CellValues.String, 1),
                    ConstructCell(string.Empty, CellValues.String, 1),
                    ConstructCell(string.Empty, CellValues.String, 1),
                    ConstructCell(string.Empty, CellValues.String, 1),
                    ConstructCell(string.Empty, CellValues.String, 0),
                    ConstructCell("location", CellValues.String, 1),
                    ConstructCell(string.Empty, CellValues.String, 1),
                    ConstructCell(string.Empty, CellValues.String, 1),
                    ConstructCell(" ", CellValues.String, 1),
                    ConstructCell(string.Empty, CellValues.String, 1),
                     ConstructCell(string.Empty, CellValues.String, 1),
                    ConstructCell(string.Empty, CellValues.String, 1),
                    ConstructCell(string.Empty, CellValues.String, 1)

                    );
                sheetData.AppendChild(sixthRow);

                var seventhRow = new Row();
                sheetData.AppendChild(seventhRow);

                var eighthRow = new Row();
                var ninethRow = new Row();
                var tenthRow = new Row();
                var eleventhRow = new Row();

                eighthRow.Append(
                    ConstructCell("Day", CellValues.String, 1));
                ninethRow.Append(
                    ConstructCell("Date", CellValues.String, 1));
                tenthRow.Append(
                    ConstructCell("Status", CellValues.String, 1));
                eleventhRow.Append(
                    ConstructCell("Hrs Worked", CellValues.String, 1));





                for (var i = 0; i <= 13; i++)
                {
                    eighthRow.Append(
                    ConstructCell(timeSheetData[i].Day, CellValues.String, 1));
                    ninethRow.Append(
                        ConstructCell(timeSheetData[i].Date, CellValues.String, 1));
                    if (timeSheetData[i].Day.ToLower() == "sunday" || timeSheetData[i].Day.ToLower() == "saturday")
                    {
                        tenthRow.Append(
                            ConstructCell(timeSheetData[i].status, CellValues.Boolean, 2));
                        eleventhRow.Append(
                            ConstructCell(timeSheetData[i].Duration_in_Hrs, CellValues.Number, 2));
                    }
                    else
                    {
                        tenthRow.Append(
                            ConstructCell(timeSheetData[i].status, CellValues.Boolean, 1));
                        eleventhRow.Append(
                            ConstructCell(timeSheetData[i].Duration_in_Hrs, CellValues.Number, 1));
                    }
                }

                sheetData.AppendChild(eighthRow);
                sheetData.AppendChild(ninethRow);
                sheetData.AppendChild(tenthRow);
                sheetData.AppendChild(eleventhRow);


                var twelvethRow = new Row();
                sheetData.AppendChild(twelvethRow);

                var thirteenthRow = new Row();
                var fourteenthRow = new Row();
                var fifteenthRow = new Row();
                var sixteenthRow = new Row();

                thirteenthRow.Append(
                    ConstructCell("Day", CellValues.String, 1));
                fourteenthRow.Append(
                    ConstructCell("Date", CellValues.String, 1));
                fifteenthRow.Append(
                    ConstructCell("Status", CellValues.String, 1));
                sixteenthRow.Append(
                    ConstructCell("Hrs Worked", CellValues.String, 1));

                for (var i = 14; i <= timeSheetData.Count - 1; i++)
                {
                    thirteenthRow.Append(
                        ConstructCell(timeSheetData[i].Day, CellValues.String, 1));
                    fourteenthRow.Append(
                        ConstructCell(timeSheetData[i].Date, CellValues.String, 1));
                    if (timeSheetData[i].Day.ToLower() == "sunday" || timeSheetData[i].Day.ToLower() == "saturday")
                    {
                        fifteenthRow.Append(
                        ConstructCell(timeSheetData[i].status, CellValues.Boolean, 2));
                        sixteenthRow.Append(
                        ConstructCell(timeSheetData[i].Duration_in_Hrs, CellValues.Number, 2));
                    }
                    else
                    {
                        fifteenthRow.Append(
                        ConstructCell(timeSheetData[i].status, CellValues.Boolean, 1));
                        sixteenthRow.Append(
                        ConstructCell(timeSheetData[i].Duration_in_Hrs, CellValues.Number, 1));
                    }
                }

                sheetData.AppendChild(thirteenthRow);
                sheetData.AppendChild(fourteenthRow);
                sheetData.AppendChild(fifteenthRow);
                sheetData.AppendChild(sixteenthRow);

                var seventeenthRow = new Row();
                sheetData.AppendChild(seventeenthRow);

                mergeCells.Append(new MergeCell() { Reference = new StringValue("A18:F18") });

                var eighteenthRow = new Row();
                eighteenthRow.Append(
                      ConstructCell("* ( P = Present, L = Leave, H = Holiday, C=Comp off ,WFH = Work from Home)", CellValues.String, 0),
                     ConstructCell(string.Empty, CellValues.String, 0),
                     ConstructCell(string.Empty, CellValues.String, 0),
                     ConstructCell(string.Empty, CellValues.String, 0),
                     ConstructCell(string.Empty, CellValues.String, 0),
                     ConstructCell(string.Empty, CellValues.String, 0)

    );
                sheetData.AppendChild(eighteenthRow);

                var nineteenthRow = new Row();
                sheetData.AppendChild(nineteenthRow);

                mergeCells.Append(new MergeCell() { Reference = new StringValue("A20:F20") });
                mergeCells.Append(new MergeCell() { Reference = new StringValue("K20:N20") });
                mergeCells.Append(new MergeCell() { Reference = new StringValue("O20:P20") });


                var twentythRow = new Row();
                twentythRow.Append(
                    ConstructCell("The above stated details are correct as per the records with us.", CellValues.String, 0),
                     ConstructCell(string.Empty, CellValues.String, 0),
                     ConstructCell(string.Empty, CellValues.String, 0),
                     ConstructCell(string.Empty, CellValues.String, 0),
                     ConstructCell(string.Empty, CellValues.String, 0),
                     ConstructCell(string.Empty, CellValues.String, 0),
                     ConstructCell(string.Empty, CellValues.String, 0),
                     ConstructCell(string.Empty, CellValues.String, 0),
                     ConstructCell(string.Empty, CellValues.String, 0),
                     ConstructCell(string.Empty, CellValues.String, 0),
                     ConstructCell("Comp-Off Earned", CellValues.String, 1),
                     ConstructCell(string.Empty, CellValues.String, 1),
                     ConstructCell(string.Empty, CellValues.String, 1),
                     ConstructCell(string.Empty, CellValues.String, 1),
                     ConstructCell("Comp-Off Utilized", CellValues.String, 1),
                     ConstructCell(string.Empty, CellValues.String, 1)


                    );


                sheetData.AppendChild(twentythRow);

                mergeCells.Append(new MergeCell() { Reference = new StringValue("K21:N21") });
                mergeCells.Append(new MergeCell() { Reference = new StringValue("O21:P21") });

                var rowTwentyone = new Row();
                rowTwentyone.Append(
                     ConstructCell(string.Empty, CellValues.String, 0),
                     ConstructCell(string.Empty, CellValues.String, 0),
                     ConstructCell(string.Empty, CellValues.String, 0),
                     ConstructCell(string.Empty, CellValues.String, 0),
                     ConstructCell(string.Empty, CellValues.String, 0),
                     ConstructCell(string.Empty, CellValues.String, 0),
                     ConstructCell(string.Empty, CellValues.String, 0),
                     ConstructCell(string.Empty, CellValues.String, 0),
                     ConstructCell(string.Empty, CellValues.String, 0),
                     ConstructCell(string.Empty, CellValues.String, 0),
                     ConstructCell(string.Empty, CellValues.String, 1),
                     ConstructCell(string.Empty, CellValues.String, 1),
                     ConstructCell(string.Empty, CellValues.String, 1),
                     ConstructCell(string.Empty, CellValues.String, 1),
                     ConstructCell(string.Empty, CellValues.String, 1),
                     ConstructCell(string.Empty, CellValues.String, 1)


                    );
                sheetData.AppendChild(rowTwentyone);

                mergeCells.Append(new MergeCell() { Reference = new StringValue("K22:N22") });
                mergeCells.Append(new MergeCell() { Reference = new StringValue("O22:P22") });

                var rowTwentytwo = new Row();
                rowTwentytwo.Append(
                     ConstructCell(string.Empty, CellValues.String, 0),
                     ConstructCell(string.Empty, CellValues.String, 0),
                     ConstructCell(string.Empty, CellValues.String, 0),
                     ConstructCell(string.Empty, CellValues.String, 0),
                     ConstructCell(string.Empty, CellValues.String, 0),
                     ConstructCell(string.Empty, CellValues.String, 0),
                     ConstructCell(string.Empty, CellValues.String, 0),
                     ConstructCell(string.Empty, CellValues.String, 0),
                     ConstructCell(string.Empty, CellValues.String, 0),
                     ConstructCell(string.Empty, CellValues.String, 0),
                     ConstructCell(string.Empty, CellValues.String, 1),
                     ConstructCell(string.Empty, CellValues.String, 1),
                     ConstructCell(string.Empty, CellValues.String, 1),
                     ConstructCell(string.Empty, CellValues.String, 1),
                     ConstructCell(string.Empty, CellValues.String, 1),
                     ConstructCell(string.Empty, CellValues.String, 1)


                    );
                sheetData.AppendChild(rowTwentytwo);

                mergeCells.Append(new MergeCell() { Reference = new StringValue("A23:B23") });
                mergeCells.Append(new MergeCell() { Reference = new StringValue("K23:N23") });
                mergeCells.Append(new MergeCell() { Reference = new StringValue("O23:P23") });

                var rowTwentythree = new Row();
                rowTwentythree.Append(
                    ConstructCell("Authorized Signatory", CellValues.String, 0),
                    ConstructCell(string.Empty, CellValues.String, 0),
                    ConstructCell(string.Empty, CellValues.String, 0),
                     ConstructCell(string.Empty, CellValues.String, 0),
                     ConstructCell(string.Empty, CellValues.String, 0),
                     ConstructCell(string.Empty, CellValues.String, 0),
                     ConstructCell(string.Empty, CellValues.String, 0),
                     ConstructCell(string.Empty, CellValues.String, 0),
                     ConstructCell(string.Empty, CellValues.String, 0),
                     ConstructCell(string.Empty, CellValues.String, 0),
                     ConstructCell(string.Empty, CellValues.String, 1),
                     ConstructCell(string.Empty, CellValues.String, 1),
                     ConstructCell(string.Empty, CellValues.String, 1),
                     ConstructCell(string.Empty, CellValues.String, 1),
                     ConstructCell(string.Empty, CellValues.String, 1),
                     ConstructCell(string.Empty, CellValues.String, 1)
                    );
                sheetData.AppendChild(rowTwentythree);

                mergeCells.Append(new MergeCell() { Reference = new StringValue("A25:B25") });
                mergeCells.Append(new MergeCell() { Reference = new StringValue("C25:F25") });

                mergeCells.Append(new MergeCell() { Reference = new StringValue("C26:F26") });
                mergeCells.Append(new MergeCell() { Reference = new StringValue("A26:B26") });
                mergeCells.Append(new MergeCell() { Reference = new StringValue("G25:I26") });

                mergeCells.Append(new MergeCell() { Reference = new StringValue("K25:N25") });
                mergeCells.Append(new MergeCell() { Reference = new StringValue("O25:P25") });

                mergeCells.Append(new MergeCell() { Reference = new StringValue("K24:N24") });
                mergeCells.Append(new MergeCell() { Reference = new StringValue("O24:P24") });

                var rowTwentyfour = new Row();
                rowTwentyfour.Append(
                    ConstructCell(string.Empty, CellValues.String, 0),
                    ConstructCell(string.Empty, CellValues.String, 0),
                    ConstructCell(string.Empty, CellValues.String, 0),
                    ConstructCell(string.Empty, CellValues.String, 0),
                    ConstructCell(string.Empty, CellValues.String, 0),
                    ConstructCell(string.Empty, CellValues.String, 0),
                    ConstructCell(string.Empty, CellValues.String, 0),
                    ConstructCell(string.Empty, CellValues.String, 0),
                    ConstructCell(string.Empty, CellValues.String, 0),
                    ConstructCell(string.Empty, CellValues.String, 0),
                    ConstructCell(string.Empty, CellValues.String, 1),
                    ConstructCell(string.Empty, CellValues.String, 1),
                    ConstructCell(string.Empty, CellValues.String, 1),
                    ConstructCell(string.Empty, CellValues.String, 1),
                    ConstructCell(string.Empty, CellValues.String, 1),
                    ConstructCell(string.Empty, CellValues.String, 1)


                   );
                sheetData.AppendChild(rowTwentyfour);

                var rowTwentyfive = new Row();
                rowTwentyfive.Append(
                     ConstructCell("Name", CellValues.String, 1),
                     ConstructCell(string.Empty, CellValues.String, 1),
                     ConstructCell(timeSheetData[0].Reporting_Manager1, CellValues.String, 1),
                     ConstructCell(string.Empty, CellValues.String, 1),
                     ConstructCell(string.Empty, CellValues.String, 1),
                     ConstructCell(string.Empty, CellValues.String, 1),
                     ConstructCell("Signature :", CellValues.String, 1),
                     ConstructCell(string.Empty, CellValues.String, 1),
                     ConstructCell(string.Empty, CellValues.String, 1),
                     ConstructCell(string.Empty, CellValues.String, 0)





                    );
                sheetData.AppendChild(rowTwentyfive);

                mergeCells.Append(new MergeCell() { Reference = new StringValue("C21:G21") });
                mergeCells.Append(new MergeCell() { Reference = new StringValue("A21:B21") });

                var rowTwentysix = new Row();
                rowTwentysix.Append(
                    ConstructCell("Designation", CellValues.String, 1),
                    ConstructCell(string.Empty, CellValues.String, 1),
                    ConstructCell(timeSheetData[0].Designation_Name, CellValues.String, 1),
                    ConstructCell(string.Empty, CellValues.String, 1),
                    ConstructCell(string.Empty, CellValues.String, 1),

                    ConstructCell(string.Empty, CellValues.String, 1),

                    ConstructCell(string.Empty, CellValues.String, 1),
                     ConstructCell("Signature :", CellValues.String, 1),

                    ConstructCell(string.Empty, CellValues.String, 1)




                    );
                sheetData.AppendChild(rowTwentysix);

                worksheetPart.Worksheet.InsertAfter(mergeCells, worksheetPart.Worksheet.Elements<SheetData>().First());

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
            Fonts fonts = new Fonts(
                new Font( // Index 0 - default
                    new FontSize() { Val = 10 }
                ),
                new Font( // Index 1 - header
                    new FontSize() { Val = 10 },
                    new Bold(),
                    new Color() { Rgb = "#FFFFFF" }

                ));


            Fills fills = new Fills(
                    new Fill(new PatternFill() { PatternType = PatternValues.None }), // Index 0 - default
                    new Fill(new PatternFill() { PatternType = PatternValues.Gray125 }), // Index 1 - default
                    new Fill(new PatternFill(new ForegroundColor { Rgb = new HexBinaryValue() { Value = "FFFFFF00" } })
                    { PatternType = PatternValues.Solid }) // Index 2 - header
                );
            Borders borders = new Borders(
                    new Border(), // index 0 default
                    new Border( // index 1 black border
                        new LeftBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Medium },
                        new RightBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Medium },
                        new TopBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Medium },
                        new BottomBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Medium },

                        new DiagonalBorder())
                );
            Alignment alignment = new Alignment() { Horizontal = HorizontalAlignmentValues.Justify, Vertical = VerticalAlignmentValues.Justify };

            CellFormats cellFormats = new CellFormats(
                    new CellFormat(), // default
                    new CellFormat { FontId = 0, FillId = 0, BorderId = 1, ApplyBorder = true }, // body
                    new CellFormat { FontId = 1, FillId = 2, BorderId = 1, ApplyFill = true } // header
                );


            styleSheet = new Stylesheet(fonts, fills, borders, cellFormats, alignment);
            return styleSheet;
        }
    }
}
