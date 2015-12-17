using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AssetManagerMvc.Models;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace AssetManagerMvc.Controllers
{
    public class TelephoneListController : Controller
    {
        private AssetManagerContext db = new AssetManagerContext();

        // GET: TelephoneList
        public ActionResult Index()
        {                       
            return View(internWithoutLog());
        }

        public ActionResult Print()
        {
            return File(TelephoneListPDFStream(), "application/pdf", "test.pdf");
        }
        private List<TelephoneListItem> internWithoutLog()
        {
            List<TelephoneListItem> list = db.UsePeriods
               .Include(u => u.Asset)
               .Include(u => u.Status)
               .Include(u => u.UserAccount)
               .Where(u => u.Asset is Telephone)
               .Where(u => (u.Asset as Telephone).TelephoneType != "GSM")
               .Where(u => u.EndDate == null || u.EndDate >= DateTime.Now)
               .Where(u => u.UserAccount != null)
               .Where(u => u.UserAccount.DepartmentString != "log")
               .OrderBy( u => u.UserAccount.Name)
               .Select(u => new TelephoneListItem
               {
                   Department = u.UserAccount.DepartmentString,
                   Name = u.UserAccount.Name,
                   NumberIntern = (u.Asset as Telephone).NumberIntern
               })
               .ToList();


            return list;
        }
        private MemoryStream TelephoneListPDFStream()
        {
            MemoryStream stream = new MemoryStream();
            Document document = new Document();
            List<Group<string, TelephoneListItem>> listByDep = TelephoneListByDepartment();
            List<TelephoneListItem> listLog = TelephoneListLog(db);
            List<TelephoneListItem> listGSM = TelephoneListGSM(db);
            // Font font = FontFactory.GetFont("Arial", 28, Color.BLACK);          

            try
            {
                PdfWriter pdfWriter = PdfWriter.GetInstance(document, stream);
                pdfWriter.CloseStream = false;
                document.Open();


                PdfPTable layoutTable = new PdfPTable(2);
                layoutTable.WidthPercentage = 100f;
                layoutTable.SplitLate = false;

                PdfPCell cellTitle = CreateCell(5f, "Telefoonlijst");
                cellTitle.Colspan = 2;
                cellTitle.HorizontalAlignment = 1; // centered
                layoutTable.AddCell(cellTitle);

                PdfPTable tblLeft = new PdfPTable(1);

                PdfPCell cellIntern = CreateCellWithParaTitle(5f, "Interne nummers");

                foreach (var dep in listByDep)
                {
                    Paragraph para = new Paragraph();
                    para.Add(dep.Key);
                    para.SpacingAfter = 3f;
                    cellIntern.AddElement(para);

                    PdfPTable table = depTable();

                    foreach (var number in dep.Values)
                    {
                        PdfPCell cell = new PdfPCell(new Phrase(number.Name));
                        // cell.Border = Rectangle.BOTTOM_BORDER | Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER;
                        table.AddCell(cell);
                        cell = new PdfPCell(new Phrase(number.NumberIntern));
                        // cell.Border = Rectangle.BOTTOM_BORDER | Rectangle.TOP_BORDER | Rectangle.RIGHT_BORDER;
                        table.AddCell(cell);
                    }
                    cellIntern.AddElement(table);
                }

                PdfPCell cellLog = CreateCellWithParaTitle(5f, "log");
                PdfPTable tableLog = depTable();

                foreach (var tel in listLog)
                {
                    PdfPCell cell = new PdfPCell(new Phrase(tel.Name));
                    tableLog.AddCell(cell);
                    cell = new PdfPCell(new Phrase(tel.NumberIntern));
                    tableLog.AddCell(cell);
                }
                cellLog.AddElement(tableLog);

                PdfPCell cellGSM = CreateCellWithParaTitle(5f, "GSM");
                PdfPTable tableGSM = depTable();

                foreach (var tel in listGSM)
                {
                    PdfPCell cell = new PdfPCell(new Phrase(tel.Name));
                    tableGSM.AddCell(cell);
                    cell = new PdfPCell(new Phrase(tel.Number));
                    tableGSM.AddCell(cell);
                }
                cellGSM.AddElement(tableGSM);


                tblLeft.AddCell(cellIntern);
                tblLeft.AddCell(cellLog);
                PdfPCell nesthousing = new PdfPCell(tblLeft);
                nesthousing.Padding = 0f;
                layoutTable.AddCell(nesthousing);
                layoutTable.AddCell(cellGSM);

                document.Add(layoutTable);

                document.Close();
            }
            catch (DocumentException de)
            {
                Console.Error.WriteLine(de.Message + "My DocumentException");
            }
            catch (IOException ioe)
            {
                Console.Error.WriteLine(ioe.Message + "My IOException");
            }
            document.Close();

            stream.Flush();
            stream.Position = 0;

            return stream;
        }
        private PdfPCell CreateCell(float padding = 5f, string str = null)
        {
            var c = new PdfPCell();
            if (!string.IsNullOrEmpty(str))
            { c.Phrase = new Phrase(str); }
            c.Padding = padding;
            return c;
        }
        private PdfPCell CreateCellWithParaTitle(float padding, string str)
        {
            var c = CreateCell(padding);
            if (!string.IsNullOrEmpty(str))
            {
                Paragraph para = new Paragraph();
                para.Add(str);
                para.SpacingAfter = 10f;
                c.AddElement(para);
            }
            return c;
        }
        private PdfPTable depTable()
        {
            PdfPTable table = new PdfPTable(2);
            table.WidthPercentage = 95f;
            // table.SpacingBefore = 20f;
            table.SpacingAfter = 10f;
            return table;
        }
        // list of intern telephone numbers, no gsm's, without log department
        private List<Group<string, TelephoneListItem>> TelephoneListByDepartment()
        {
            var currentPhones = db.UsePeriods
           .Include(u => u.Asset)
           .Include(u => u.Status)
           .Include(u => u.UserAccount)
           .Where(u => u.Asset is Telephone)
           .Where(u => (u.Asset as Telephone).TelephoneType != "GSM")
           .Where(u => u.EndDate == null || u.EndDate >= DateTime.Now)
           .Where(u => u.UserAccount != null)
           .Where(u => u.UserAccount.DepartmentString != "log")
            .Select(u => new TelephoneListItem
            {
                Department = u.UserAccount.DepartmentString,
                Name = u.UserAccount.Name,
                NumberIntern = (u.Asset as Telephone).NumberIntern
            });

            var grouped1 = from phone in currentPhones
                           group phone by phone.Department into groupedPhones
                           orderby groupedPhones.Key
                           select new Group<string, TelephoneListItem>
                           { Key = groupedPhones.Key, Values = groupedPhones.OrderBy(t => t.Name) };
            return grouped1.ToList();
        }
        private List<TelephoneListItem> TelephoneListLog(AssetManagerContext db)
        {
            var logPhones = db.UsePeriods
           .Include(u => u.Asset)
           .Include(u => u.Status)
           .Include(u => u.UserAccount)
           .Where(u => u.Asset is Telephone)
           .Where(u => (u.Asset as Telephone).TelephoneType != "GSM")
           .Where(u => u.EndDate == null || u.EndDate >= DateTime.Now)
           .Where(u => u.UserAccount != null)
           .Where(u => u.UserAccount.DepartmentString == "log")
            .Select(u => new TelephoneListItem
            {
                Department = u.UserAccount.DepartmentString,
                Name = u.UserAccount.Name,
                NumberIntern = (u.Asset as Telephone).NumberIntern
            });

            return logPhones.ToList();
        }
        private List<TelephoneListItem> TelephoneListGSM(AssetManagerContext db)
        {
            var gsmPhones = db.UsePeriods
           .Include(u => u.Asset)
           .Include(u => u.Status)
           .Include(u => u.UserAccount)
           .Where(u => u.Asset is Telephone)
           .Where(u => (u.Asset as Telephone).TelephoneType == "GSM")
           .Where(u => u.EndDate == null || u.EndDate >= DateTime.Now)
           .Where(u => u.UserAccount != null)
            .Select(u => new TelephoneListItem
            {
                Department = u.UserAccount.DepartmentString,
                Name = u.UserAccount.Name,
                Number = (u.Asset as Telephone).Number
            });

            return gsmPhones.ToList();
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
