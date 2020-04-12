using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TimeCard.Repo;
using TimeCard.Helpers;
using OfficeOpenXml;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;

namespace TimeCard.Controllers
{
    public class WorkController : BaseController
    {
        private readonly WorkRepo _WorkRepo;

        public WorkController()
        {
            _WorkRepo = new WorkRepo(ConnString);
        }

        public ActionResult Index()
        {

            var vm = new ViewModels.WorkViewModel();
            prepWork(vm);
            return View(vm);
        }

        [HttpPost]
        public ActionResult Index(ViewModels.WorkViewModel vm, string buttonValue)
        {
            if (buttonValue == "Save")
            {
                if (ModelState.IsValid)
                {
                    var work = vm.EditWork;
                    _WorkRepo.SaveWork(work);
                }
            }
            else
            {
                if (buttonValue == "Delete")
                {
                    _WorkRepo.DeleteWork(vm.EditWork.WorkId);
                    vm.EditWork = null;
                }
                ModelState.Clear();
            }
            prepWork(vm);
            return View(vm);
        }

        private void prepWork(ViewModels.WorkViewModel vm)
        {
            Domain.Lookup contractor = Session["Contractor"] as Domain.Lookup;
            if (contractor == null)
            {
                contractor = LookupRepo.GetLookupByVal("Contractor", CurrentUsername);
                Session["Contractor"] = contractor;
            }

            var cycles = GetPayCycles();
            int cycle = int.Parse(cycles.First().Value);
            vm.Jobs = _WorkRepo.GetJobs().Select(x => new SelectListItem { Text = x.Descr, Value = x.Id.ToString() });
            vm.PayCycles = cycles;
            if (vm.SelectedCycle == 0)
            {
                vm.SelectedCycle = cycle;
            }
            vm.WorkEntries = _WorkRepo.GetWork((Session["Contractor"] as Domain.Lookup).Id, vm.SelectedCycle, true);
            if (vm.EditWork == null)
            {
                vm.EditWork = new Domain.Work { ContractorId = contractor.Id, WorkDay = DateRef.GetWorkDay(DateTime.Today) };
            }
            vm.EditDays = GetEditDays(vm.SelectedCycle);
        }
        private IEnumerable<SelectListItem> GetEditDays(int thisCycle)
        {
            return Enumerable.Range(0, 14).Select(x => new SelectListItem { Text = DateRef.GetWorkDate(thisCycle + (decimal)x / 100, false), Value = (thisCycle + (decimal)x / 100).ToString() });
        }

        private IEnumerable<SelectListItem> GetPayCycles()
        {
            var thisCycle = decimal.Floor(DateRef.GetWorkDay(DateTime.Today));
            return Enumerable.Range(0, 15).Select(x => new SelectListItem { Text = DateRef.GetWorkDate(thisCycle - x), Value = (thisCycle - x).ToString() });
        }

        [HttpPost]
        public ActionResult GenerateDocs(int contractorId, int cycle)
        {
            var templateFile = new FileInfo(@"c:\TEMP\TimecardTemplates.xlsx");

            try
            {
                string name = LookupRepo.GetLookups("Contractor").Where(x => x.Id == contractorId).FirstOrDefault()?.Descr;

                var fileList = new List<string>();
                //GenerateTimeCards(contractorId, name, templateFile, cycle, fileList);
                //GenerateTimeBooks(contractorId, name, templateFile, cycle, fileList);
                GenerateInvoices(contractorId, name, templateFile, cycle, fileList);
                if (!fileList.Any())
                {
                    return Json(new { success = false, message = "Nothing to generate." });
                }


                string zipFile = $"C:\\TEMP\\{name}.zip";
                TempData["Download"] = new Infrastructure.ZipDownload { FileName = zipFile, FileList = fileList };
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }

        }

        private void GenerateTimeCards(int contractorId, string name, FileInfo templateFile, int cycle, List<string> fileList)
        {
            const int blankRow = 11;

            var workEntries = _WorkRepo.GetWorkExtended(contractorId, cycle, true, 1).Where(x => x.BillType == "TC")
                .GroupBy(g => new { g.ClientId, g.ProjectId });


            using (var templatePackage = new ExcelPackage(templateFile))
            {
                var templateSheet = templatePackage.Workbook.Worksheets["TimeCard"];

                foreach (var tc in workEntries)
                {
                    //create a new time card and populate it

                    string endDate = new Domain.WorkExtended { WorkDay = (decimal)cycle }.CycleEndDate;

                    var file = new FileInfo($"C:\\TEMP\\FWSI_TC_{endDate.Replace("/", "")}_{name}_{tc.First().Client}_{tc.First().Project}.xlsx");
                    System.IO.File.Delete(file.FullName);
                    ExcelWorksheet sheet = null;
                    using (var package = new ExcelPackage(file))
                    {
                        var workBook = package.Workbook;
                        var first = tc.First();
                        int[] currentRow = { blankRow + 1, blankRow + 1 };
                        var tcWeek = tc.GroupBy(w => new { tabName = $"{ w.Project} Week {w.WorkWeek + 1}", weekDate = w.WorkWeekDate, week = w.WorkWeek });
                        foreach (var week in tcWeek)
                        {
                            sheet = workBook.Worksheets.Add(week.Key.tabName, templateSheet);
                            sheet.Cells[7, 9].Value = week.Key.weekDate;
                            sheet.Cells[7, 5].Value = first.Client;
                            sheet.Cells[7, 1].Value = first.Contractor;
                            sheet.Cells[14, 5].Value = first.Contractor;
                            sheet.Cells[14, 10].Value = DateTime.Today;
                        }
                        foreach (var entry in tc)
                        {
                            var w = entry.WorkWeek;
                            sheet = workBook.Worksheets[$"{ entry.Project} Week {entry.WorkWeek + 1}"];
                            sheet.InsertRow(currentRow[w], 1);
                            sheet.Cells[blankRow, 1, blankRow, 20].Copy(sheet.Cells[currentRow[w], 1]);

                            sheet.Cells[currentRow[w], 3].Value = entry.WorkType;
                            sheet.Cells[currentRow[w], 2].Value = entry.Descr;
                            sheet.Cells[currentRow[w], 4].Value = entry.Project;
                            sheet.Cells[currentRow[w], 5 + entry.WorkWeekDay].Value = entry.Hours;
                            sheet.Cells[currentRow[w], 13].Formula = $"= SUM(E{ currentRow[w]}:K{ currentRow[w]})";
                            currentRow[w]++;
                        }
                        foreach (var week in tcWeek)
                        {
                            var w = week.Key.week;
                            sheet = workBook.Worksheets[week.Key.tabName];
                            for (int i = 0; i < 9; i++)
                            {
                                var column = "EFGHIJKLM".Substring(i, 1);
                                sheet.Cells[currentRow[w], i + 5].Formula = $"= SUM({column}{ blankRow}:{column}{ currentRow[w] - 1})";
                            }
                            sheet.Calculate();
                        }
                        package.Save();
                        fileList.Add(package.File.FullName);
                    }
                }
            }
        }

        public void GenerateTimeBooks(int contractorId, string name, FileInfo templateFile, int cycle, List<string> fileList)
        {
            var workEntries = _WorkRepo.GetWorkExtended(contractorId, cycle, true, 10).Where(x => "SOW TB".Contains(x.BillType))
                .GroupBy(g => new { g.ClientId, g.ProjectId });

            using (var templatePackage = new ExcelPackage(templateFile))
            {
                var templateSheet = templatePackage.Workbook.Worksheets["TimeBook"];
                var file = new FileInfo($"C:\\TEMP\\FWSI_TimeBooks.xlsx");
                System.IO.File.Delete(file.FullName);

                using (var package = new ExcelPackage(file))
                {
                    foreach (var tb in workEntries)
                    {
                        var first = tb.First();
                        var workBook = package.Workbook;
                        int currentRow = 2;
                        ExcelWorksheet sheet = workBook.Worksheets.Add($"{first.Client} {first.Project}",templateSheet);
                        foreach (var entry in tb)
                        {
                            sheet.Cells[currentRow, 1].Value = $"{entry.WorkDate:MM/dd/yyyy}";
                            sheet.Cells[currentRow, 3].Value = entry.Hours;
                            sheet.Cells[currentRow, 3].Value = entry.Descr;
                            currentRow++;
                        }
                    }
                    package.Save();
                    fileList.Add(package.File.FullName);
                }
            }
        }

        public void GenerateInvoices(int contractorId, string name, FileInfo templateFile, int cycle, List<string> fileList)
        {
            int blankRow = 14;
            var workEntries = _WorkRepo.GetWorkExtended(contractorId, cycle, true, 1).Where(x => "TB".Contains(x.BillType))
                .GroupBy(g => new { g.ClientId, g.ProjectId });

            using (var templatePackage = new ExcelPackage(templateFile))
            {
                var templateSheet = templatePackage.Workbook.Worksheets["Invoice"];
                var file = new FileInfo($"C:\\TEMP\\FWSI_Invoices.xlsx");
                System.IO.File.Delete(file.FullName);

                using (var package = new ExcelPackage(file))
                {
                    foreach (var inv in workEntries)
                    {
                        var first = inv.First();
                        var workBook = package.Workbook;
                        ExcelWorksheet sheet = workBook.Worksheets.Add($"{first.Client} {first.Project}", templateSheet);
                        sheet.Cells[2, 6].Value = $"{DateTime.Today: M/d/yyyy}";
                        sheet.Cells[9, 3].Value = first.Client;
                        sheet.Cells[10, 3].Value = first.Project;
                        int currentRow = blankRow + 1;

                        foreach (var entry in inv)
                        {
                            sheet.InsertRow(currentRow, 1);
                            sheet.Cells[blankRow, 1, blankRow, 10].Copy(sheet.Cells[currentRow, 1]);

                            sheet.Cells[currentRow, 2].Value = $"{entry.WorkDate : M/d}";
                            sheet.Cells[currentRow, 3].Value = $"{entry.WorkType} : {entry.Descr}";
                            sheet.Cells[currentRow, 4].Value = entry.Hours;
                            sheet.Cells[currentRow, 6].Formula = $"=D{currentRow} * C11";
                            currentRow++;
                        }
                        sheet.Cells[currentRow, 4].Formula = $"=SUM(D{blankRow} : D{currentRow-1})";
                        sheet.Cells[currentRow, 6].Formula = $"=SUM(F{blankRow} : F{currentRow-1})";
                        sheet.Calculate();
                    }
                    
                    package.Save();
                    fileList.Add(package.File.FullName);
                }
            }
        }



        [HttpGet]
        public void DownloadTimeDocs()
        {
            var download = TempData["Download"] as Infrastructure.ZipDownload;

            Response.ContentType = "application/zip";
            // If the browser is receiving a mangled zipfile, IIS Compression may cause
            // this problem. Some members have found that 
            //Response.ContentType = "application/octet-stream" 
            // has solved this. May be specific to Internet Explorer.

            Response.AppendHeader("Content-Disposition",
                $"attachment; filename=\"{Path.GetFileName(download.FileName)}\"");
            Response.CacheControl = "Private";

            Response.Cache.SetExpires(DateTime.Now.AddMinutes(3));
            // or put a timestamp in the filename in the content-disposition

            var buffer = new byte[4096];

            using (var zipOutputStream = new ZipOutputStream(Response.OutputStream))
            {

                // 0-9, 9 being the highest level of compression
                zipOutputStream.SetLevel(3);

                foreach (string fileName in download.FileList)
                {

                    using (Stream fs = System.IO.File.OpenRead(fileName))
                    {

                        var entry = new ZipEntry(ZipEntry.CleanName(Path.GetFileName(fileName)));
                        entry.Size = fs.Length;
                        // Setting the Size provides WinXP built-in extractor 
                        // compatibility, but if not available, you can instead set 
                        //zipOutputStream.UseZip64 = UseZip64.Off

                        zipOutputStream.PutNextEntry(entry);

                        int count = fs.Read(buffer, 0, buffer.Length);
                        while (count > 0)
                        {
                            zipOutputStream.Write(buffer, 0, count);
                            count = fs.Read(buffer, 0, buffer.Length);
                            if (!Response.IsClientConnected)
                            {
                                break;
                            }
                            Response.Flush();
                        }
                    }
                }
            }
            Response.Flush();
            Response.End();
        }
    }
}