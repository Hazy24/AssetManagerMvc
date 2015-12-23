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
using System.Diagnostics;

namespace AssetManagerMvc.Controllers
{
    public class TelephoneListController : Controller
    {
        private AssetManagerContext db = new AssetManagerContext();

        // GET: TelephoneList
        public ActionResult Index(string sortOrder, string searchString)
        {

            List<TelephoneListItem> telephoneList = InternWithoutLog().ToList();
            telephoneList.AddRange(ExternLog());
            telephoneList.AddRange(GSM());
            telephoneList.AddRange(WithFunction());            

            if (!String.IsNullOrWhiteSpace(searchString))
            {
                telephoneList = telephoneList.Where
                    (t => (!string.IsNullOrEmpty(t.Name) 
                      && t.Name.ToLower().Contains(searchString.ToLower()))
                      || (!string.IsNullOrEmpty(t.Department)
                      && t.Department.ToLower().Contains(searchString.ToLower()))
                      || (!string.IsNullOrEmpty(t.Number)
                      && t.Number.Contains(searchString.ToLower()))
                      || (!string.IsNullOrEmpty(t.NumberIntern)
                      && t.NumberIntern.Contains(searchString.ToLower()))
                    )
                .ToList();
            }
            return View(telephoneList);
        }

        public ActionResult Print()
        {
            return File(TelephoneListPDFStream(), "application/pdf", "telefoonlijst.pdf");
        }
        private IQueryable<TelephoneListItem> InternWithoutLog()
        {
            var query = db.UsePeriods
               .Include(u => u.Asset)
               .Include(u => u.Status)
               .Include(u => u.UserAccount)              
               .Where(u => u.Asset is Telephone)
               .Where(u => (u.Asset as Telephone).TelephoneType != "GSM")
               .Where(u => u.EndDate == null || u.EndDate >= DateTime.Now)
               .Where(u => u.UserAccount != null)
               .Where(u => u.UserAccount.Department.LdapName != "log")
               .Where(u => u.Status.UsePeriodStatusId == 1) 
               .OrderBy( u => u.UserAccount.Name)
               .Select(u => new TelephoneListItem
               {
                   Department = u.UserAccount.Department.Name,
                   Name = u.UserAccount.Name,
                   Number = (u.Asset as Telephone).Number,
                   NumberIntern = (u.Asset as Telephone).NumberIntern
               })
               ;
            return query;
        }
        private IQueryable<TelephoneListItem> ExternLog()
        {
            IQueryable<TelephoneListItem> query = db.UsePeriods
               .Include(u => u.Asset)
               .Include(u => u.Status)
               .Include(u => u.UserAccount)               
               .Where(u => u.Asset is Telephone)
               .Where(u => (u.Asset as Telephone).TelephoneType != "GSM")
               .Where(u => u.EndDate == null || u.EndDate >= DateTime.Now)
               .Where(u => u.UserAccount != null)
               .Where(u => u.UserAccount.Department.LdapName == "log")
               .Where(u => u.Status.UsePeriodStatusId == 1) // afgeleverd en in werking  
               .OrderBy(u => u.UserAccount.Name)
               .Select(u => new TelephoneListItem
               {
                   Department = u.UserAccount.Department.Name,
                   Name = u.UserAccount.Name,
                   Number = (u.Asset as Telephone).Number,
                   NumberIntern = (u.Asset as Telephone).NumberIntern
               })
               ;
            return query;
        }
        private IQueryable<TelephoneListItem> GSM()
        {
            IQueryable<TelephoneListItem> query = db.UsePeriods
               .Include(u => u.Asset)
               .Include(u => u.Status)
               .Include(u => u.UserAccount)               
               .Where(u => u.Asset is Telephone)
               .Where(u => (u.Asset as Telephone).TelephoneType == "GSM")
               .Where(u => u.EndDate == null || u.EndDate >= DateTime.Now)
               .Where(u => u.UserAccount != null)
               .Where(u => u.Status.UsePeriodStatusId == 1) // afgeleverd en in werking        
               .OrderBy(u => u.UserAccount.Name)
               .Select(u => new TelephoneListItem
               {                   
                   Name = u.UserAccount.Name,
                   Department = u.UserAccount.Department.Name,
                   Number = (u.Asset as Telephone).Number,
                   NumberIntern = (u.Asset as Telephone).NumberIntern
               })
               ;
            return query;
        }
        private IQueryable<TelephoneListItem> WithFunction()
        {
            IQueryable<TelephoneListItem> query = db.UsePeriods
               .Include(u => u.Asset)
               .Include(u => u.Status)
               .Include(u => u.UserAccount)               
               .Where(u => u.Asset is Telephone)               
               .Where(u => u.EndDate == null || u.EndDate >= DateTime.Now)
               .Where(u => u.UserAccount == null)
               .Where(u => u.Status.UsePeriodStatusId == 1) // afgeleverd en in werking  
               .Where(u => !string.IsNullOrEmpty((u.Asset as Telephone).NumberIntern))         
               .OrderBy(u => u.Function)
               .Select(u => new TelephoneListItem
               {                   
                   Name = u.Function,
                   Department = u.UserAccount.Department.Name,
                   Number = (u.Asset as Telephone).Number,
                   NumberIntern = (u.Asset as Telephone).NumberIntern
               })
               ;

            return query;
        }
        private MemoryStream TelephoneListPDFStream()
        {
            MemoryStream stream = new MemoryStream();
            Document document = new Document();

            try
            {
                PdfWriter pdfWriter = PdfWriter.GetInstance(document, stream);
                // pdfWriter.PageEvent = new ITextEvents();
                pdfWriter.CloseStream = false;
                document.Open();

                string fontpath = Server.MapPath(@"~/fonts/");
                BaseFont OxfamGlobalHeadline = BaseFont.CreateFont(fontpath + "OxfamGlobalHeadline.ttf", 
                    BaseFont.CP1252, BaseFont.EMBEDDED);
                //Font font = new Font(customfont, 12);
                //string s = "My expensive custom font.";
                //document.Add(new Paragraph(s, font));

                Paragraph heading = new Paragraph("TELEFOONLIJST", new Font(OxfamGlobalHeadline, 28f, Font.BOLD));
                heading.SpacingAfter = 40f;
                heading.Alignment = Element.ALIGN_CENTER;
                document.Add(heading);

                int rowsPerPage = 43;             

                MultiColumnText columns = new MultiColumnText();
                //float left, float right, float gutterwidth, int numcolumns
                columns.AddRegularColumns(36f, document.PageSize.Width - 36f, 24f, 2);                            
                columns.AddElement(GetPdfPtableInternWithoutLog(rowsPerPage));
                columns.AddElement(new Paragraph("OFTL", 
                    new Font(Font.HELVETICA, 9f, Font.BOLD)));
                columns.AddElement(GetPdfPtableExternLog());

                document.Add(columns);

                document.NewPage();
                document.Add(heading);

                columns = new MultiColumnText();
                //float left, float right, float gutterwidth, int numcolumns
                columns.AddRegularColumns(36f, document.PageSize.Width - 36f, 24f, 2);
                columns.AddElement(GetPdfPtableGSMAndFunction(rowsPerPage));               

                document.Add(columns);
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
        public class ITextEvents : PdfPageEventHelper
        {

            // This is the contentbyte object of the writer
            PdfContentByte cb;

            // we will put the final number of pages in a template
            PdfTemplate headerTemplate, footerTemplate;

            // this is the BaseFont we are going to use for the header / footer
            BaseFont bf = null;

            // This keeps track of the creation time
            DateTime PrintTime = DateTime.Now;


            #region Fields
            private string _header;
            #endregion

            #region Properties
            public string Header
            {
                get { return _header; }
                set { _header = value; }
            }
            #endregion


            public override void OnOpenDocument(PdfWriter writer, Document document)
            {
                try
                {
                    PrintTime = DateTime.Now;
                    bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                    cb = writer.DirectContent;
                    headerTemplate = cb.CreateTemplate(100, 100);
                    footerTemplate = cb.CreateTemplate(50, 50);
                }
                catch (DocumentException de)
                {

                }
                catch (System.IO.IOException ioe)
                {

                }
            }

            public override void OnEndPage(iTextSharp.text.pdf.PdfWriter writer, iTextSharp.text.Document document)
            {
                base.OnEndPage(writer, document);

                iTextSharp.text.Font baseFontNormal = new iTextSharp.text.Font(iTextSharp.text.Font.HELVETICA, 12f, iTextSharp.text.Font.NORMAL);

                iTextSharp.text.Font baseFontBig = new iTextSharp.text.Font(iTextSharp.text.Font.HELVETICA, 12f, iTextSharp.text.Font.BOLD);

                Phrase p1Header = new Phrase("Sample Header Here", baseFontNormal);

                //Create PdfTable object
                PdfPTable pdfTab = new PdfPTable(3);

                //We will have to create separate cells to include image logo and 2 separate strings
                //Row 1
                PdfPCell pdfCell1 = new PdfPCell();
                PdfPCell pdfCell2 = new PdfPCell(p1Header);
                PdfPCell pdfCell3 = new PdfPCell();
                String text = "Page " + writer.PageNumber + " of ";


                //Add paging to header
                {
                    cb.BeginText();
                    cb.SetFontAndSize(bf, 12);
                    cb.SetTextMatrix(document.PageSize.GetRight(200), document.PageSize.GetTop(45));
                    cb.ShowText(text);
                    cb.EndText();
                    float len = bf.GetWidthPoint(text, 12);
                    //Adds "12" in Page 1 of 12
                    cb.AddTemplate(headerTemplate, document.PageSize.GetRight(200) + len, document.PageSize.GetTop(45));
                }
                //Add paging to footer
                {
                    cb.BeginText();
                    cb.SetFontAndSize(bf, 12);
                    cb.SetTextMatrix(document.PageSize.GetRight(180), document.PageSize.GetBottom(30));
                    cb.ShowText(text);
                    cb.EndText();
                    float len = bf.GetWidthPoint(text, 12);
                    cb.AddTemplate(footerTemplate, document.PageSize.GetRight(180) + len, document.PageSize.GetBottom(30));
                }
                //Row 2
                PdfPCell pdfCell4 = new PdfPCell(new Phrase("Sub Header Description", baseFontNormal));
                //Row 3


                PdfPCell pdfCell5 = new PdfPCell(new Phrase("Date:" + PrintTime.ToShortDateString(), baseFontBig));
                PdfPCell pdfCell6 = new PdfPCell();
                PdfPCell pdfCell7 = new PdfPCell(new Phrase("TIME:" + string.Format("{0:t}", DateTime.Now), baseFontBig));


                //set the alignment of all three cells and set border to 0
                pdfCell1.HorizontalAlignment = Element.ALIGN_CENTER;
                pdfCell2.HorizontalAlignment = Element.ALIGN_CENTER;
                pdfCell3.HorizontalAlignment = Element.ALIGN_CENTER;
                pdfCell4.HorizontalAlignment = Element.ALIGN_CENTER;
                pdfCell5.HorizontalAlignment = Element.ALIGN_CENTER;
                pdfCell6.HorizontalAlignment = Element.ALIGN_CENTER;
                pdfCell7.HorizontalAlignment = Element.ALIGN_CENTER;


                pdfCell2.VerticalAlignment = Element.ALIGN_BOTTOM;
                pdfCell3.VerticalAlignment = Element.ALIGN_MIDDLE;
                pdfCell4.VerticalAlignment = Element.ALIGN_TOP;
                pdfCell5.VerticalAlignment = Element.ALIGN_MIDDLE;
                pdfCell6.VerticalAlignment = Element.ALIGN_MIDDLE;
                pdfCell7.VerticalAlignment = Element.ALIGN_MIDDLE;


                pdfCell4.Colspan = 3;



                pdfCell1.Border = 0;
                pdfCell2.Border = 0;
                pdfCell3.Border = 0;
                pdfCell4.Border = 0;
                pdfCell5.Border = 0;
                pdfCell6.Border = 0;
                pdfCell7.Border = 0;


                //add all three cells into PdfTable
                pdfTab.AddCell(pdfCell1);
                pdfTab.AddCell(pdfCell2);
                pdfTab.AddCell(pdfCell3);
                pdfTab.AddCell(pdfCell4);
                pdfTab.AddCell(pdfCell5);
                pdfTab.AddCell(pdfCell6);
                pdfTab.AddCell(pdfCell7);

                pdfTab.TotalWidth = document.PageSize.Width - 80f;
                pdfTab.WidthPercentage = 70;
                //pdfTab.HorizontalAlignment = Element.ALIGN_CENTER;


                //call WriteSelectedRows of PdfTable. This writes rows from PdfWriter in PdfTable
                //first param is start row. -1 indicates there is no end row and all the rows to be included to write
                //Third and fourth param is x and y position to start writing
                pdfTab.WriteSelectedRows(0, -1, 40, document.PageSize.Height - 30, writer.DirectContent);
                //set pdfContent value

                //Move the pointer and draw line to separate header section from rest of page
                cb.MoveTo(40, document.PageSize.Height - 100);
                cb.LineTo(document.PageSize.Width - 40, document.PageSize.Height - 100);
                cb.Stroke();

                //Move the pointer and draw line to separate footer section from rest of page
                cb.MoveTo(40, document.PageSize.GetBottom(50));
                cb.LineTo(document.PageSize.Width - 40, document.PageSize.GetBottom(50));
                cb.Stroke();
            }

            public override void OnCloseDocument(PdfWriter writer, Document document)
            {
                base.OnCloseDocument(writer, document);

                headerTemplate.BeginText();
                headerTemplate.SetFontAndSize(bf, 12);
                headerTemplate.SetTextMatrix(0, 0);
                headerTemplate.ShowText((writer.PageNumber - 1).ToString());
                headerTemplate.EndText();

                footerTemplate.BeginText();
                footerTemplate.SetFontAndSize(bf, 12);
                footerTemplate.SetTextMatrix(0, 0);
                footerTemplate.ShowText((writer.PageNumber - 1).ToString());
                footerTemplate.EndText();


            }
        }
        private IElement GetPdfPtableGSMAndFunction(int rowsPerPage)
        {
            List<TelephoneListItem> lstGSM = GSM().ToList();
            // lstGSM.Add(new TelephoneListItem { Name = "Extra GSM", Number = "0456 78 90 12" });
            PdfPTable tableGSMAndFunction = new PdfPTable(2);            
            tableGSMAndFunction.WidthPercentage = 100f;
            PdfPCell cell;

            for (int i = 0; i < lstGSM.Count; i++)
            {
                TelephoneListItem number = lstGSM[i];
                cell = new PdfPCell(new Phrase(number.Name,
                  new Font(Font.HELVETICA, 9f, Font.NORMAL)));
                cell.Border = (i == 0 || i == rowsPerPage) ? Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER :
                    Rectangle.TOP_BORDER | Rectangle.RIGHT_BORDER;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.FixedHeight = 16f;
                tableGSMAndFunction.AddCell(cell);
                cell = new PdfPCell(new Phrase(number.Number,
                    new Font(Font.HELVETICA, 9f, Font.BOLD)));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.Border = (i == 0 || i == rowsPerPage) ? Rectangle.BOTTOM_BORDER : Rectangle.TOP_BORDER;
                tableGSMAndFunction.AddCell(cell);
            }
            cell = new PdfPCell(new Phrase(" ", new Font(Font.HELVETICA, 9f, Font.NORMAL)));
            cell.Border = Rectangle.TOP_BORDER | Rectangle.RIGHT_BORDER;
            tableGSMAndFunction.AddCell(cell);
            cell = new PdfPCell(new Phrase(" ", new Font(Font.HELVETICA, 9f, Font.NORMAL)));
            cell.Border = Rectangle.TOP_BORDER;
            tableGSMAndFunction.AddCell(cell);

            List<TelephoneListItem> lstFunction = WithFunction().ToList();
            for (int i = 0; i < lstFunction.Count; i++)
            {
                TelephoneListItem number = lstFunction[i];
                cell = new PdfPCell(new Phrase(number.Name,
                  new Font(Font.HELVETICA, 9f, Font.NORMAL)));
                cell.Border = (i + lstGSM.Count == rowsPerPage - 1) ? Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER :
                    Rectangle.TOP_BORDER | Rectangle.RIGHT_BORDER;
                if (i + lstGSM.Count == rowsPerPage - 1)
                {
                    if (i == lstFunction.Count - 1) { cell.Border = Rectangle.RIGHT_BORDER; }
                    else { cell.Border = Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER; }
                }
                else { cell.Border = Rectangle.TOP_BORDER | Rectangle.RIGHT_BORDER; }
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.FixedHeight = 16f;
                tableGSMAndFunction.AddCell(cell);
                cell = new PdfPCell(new Phrase(number.NumberIntern,
                    new Font(Font.HELVETICA, 9f, Font.BOLD)));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.Border = (i + lstGSM.Count == rowsPerPage - 1) ? Rectangle.BOTTOM_BORDER : Rectangle.TOP_BORDER;
                if (i + lstGSM.Count == rowsPerPage - 1)
                {
                    if (i == lstFunction.Count - 1) { cell.Border = Rectangle.NO_BORDER; }
                    else { cell.Border = Rectangle.BOTTOM_BORDER; }
                }
                else { cell.Border = Rectangle.TOP_BORDER ; }
                tableGSMAndFunction.AddCell(cell);
            }


            return tableGSMAndFunction;
        }

        private PdfPTable GetPdfPtableExternLog()
        {
            List<TelephoneListItem> lstExternLog = ExternLog().ToList();
            PdfPTable tblExternLog = new PdfPTable(2);
            tblExternLog.SpacingBefore = 15f;
            //float[] widths = new float[] { 3f, 1f, 3f };
            //tblInternWithoutLog.SetWidths(widths);
            tblExternLog.WidthPercentage = 100f;

            for (int i = 0; i < lstExternLog.Count; i++)
            {
                TelephoneListItem number = lstExternLog[i];
                PdfPCell cell = new PdfPCell(new Phrase(number.Name,
                  new Font(Font.HELVETICA, 9f, Font.NORMAL)));
                cell.Border = (i == 0 || i == 44) ? Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER :
                    Rectangle.TOP_BORDER | Rectangle.RIGHT_BORDER;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.FixedHeight = 16f;
                tblExternLog.AddCell(cell);
                cell = new PdfPCell(new Phrase(number.Number,
                    new Font(Font.HELVETICA, 9f, Font.BOLD)));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.Border = (i == 0 || i == 44) ? Rectangle.BOTTOM_BORDER : Rectangle.TOP_BORDER;
                tblExternLog.AddCell(cell);               
            }
            return tblExternLog;
        }

        private PdfPTable GetPdfPtableInternWithoutLog(int rowsPerPage)
        {
            List<TelephoneListItem> lstInternWithoutLog = InternWithoutLog().ToList();
            PdfPTable tblInternWithoutLog = new PdfPTable(3);
            tblInternWithoutLog.SpacingAfter = 15f;
            float[] widths = new float[] { 3f, 1f, 3f };
            tblInternWithoutLog.SetWidths(widths);
            tblInternWithoutLog.WidthPercentage = 100f;

            for (int i = 0; i < lstInternWithoutLog.Count; i++)
            {
                TelephoneListItem number = lstInternWithoutLog[i];
                PdfPCell cell = new PdfPCell(new Phrase(number.Name,
                  new Font(Font.HELVETICA, 9f, Font.NORMAL)));
                cell.Border = (i == 0 || i == rowsPerPage) ? Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER :
                    Rectangle.TOP_BORDER | Rectangle.RIGHT_BORDER;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.FixedHeight = 16f;
                tblInternWithoutLog.AddCell(cell);
                cell = new PdfPCell(new Phrase(number.NumberIntern,
                    new Font(Font.HELVETICA, 9f, Font.BOLD)));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.Border = (i == 0 || i == rowsPerPage) ? Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER :
                    Rectangle.TOP_BORDER | Rectangle.RIGHT_BORDER;
                tblInternWithoutLog.AddCell(cell);
                cell = new PdfPCell(new Phrase(number.Department,
                    new Font(Font.HELVETICA, 9f, Font.NORMAL)));
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.Border = (i == 0 || i == rowsPerPage) ? Rectangle.BOTTOM_BORDER : Rectangle.TOP_BORDER;
                tblInternWithoutLog.AddCell(cell);
            }
            return tblInternWithoutLog;
        }

        private MemoryStream TelephoneListPDFStreamOld()
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
