using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Data;
using System.Data.Entity;

namespace AssetManagerMvc.Models
{
    public static class Util
    {
        public static IQueryable<Asset> TextSearch(this IQueryable<Asset> assets, string searchString)
        {
            assets = assets.Where(a => a.AssetId.ToString().Contains(searchString)
              || a.Manufacturer.Contains(searchString)
              || a.ModelName.Contains(searchString)
              || a.Owner.Contains(searchString)
              || a.SerialNumber.Contains(searchString)
              || a.Supplier.Contains(searchString));

            return assets;
        }
        public static IQueryable<Computer> TextSearch(this IQueryable<Computer> computers, string searchString)
        {
            // search asset properties
            IQueryable<Computer> assetSearch = (computers as IQueryable<Asset>)
                .TextSearch(searchString)
                .Cast<Computer>();

            // search computer specific properties and concat with assetSearch
            computers = assetSearch.Concat(computers.Where
              (c => c.AntiVirus.Contains(searchString)
              || c.Browser.Contains(searchString)
              || c.ComputerName.Contains(searchString)
              || c.ComputerType.Contains(searchString)
              || c.OfficeVersion.Contains(searchString)
              || c.OperatingSystem.Contains(searchString)))
              .Distinct();

            return computers;
        }
        public static IQueryable<Printer> TextSearch(this IQueryable<Printer> printers, string searchString)
        {
            IQueryable<Printer> assetSearch = (printers as IQueryable<Asset>)
                .TextSearch(searchString)
                .Cast<Printer>();

            printers = assetSearch.Concat(printers.Where
                (p => p.DrumModel.Contains(searchString)
                || p.IpAddress.Contains(searchString)
                || p.PrinterName.Contains(searchString)
                || p.TonerModel.Contains(searchString)))
                .Distinct();

            return printers;
        }
        public static IQueryable<Beamer> TextSearch(this IQueryable<Beamer> beamers, string searchString)
        {
            IQueryable<Beamer> assetSearch = (beamers as IQueryable<Asset>)
                .TextSearch(searchString)
                .Cast<Beamer>();

            beamers = assetSearch.Concat(beamers.Where
                (p => p.BeamerName.Contains(searchString)))
                .Distinct();

            return beamers;
        }
        public static IQueryable<Monitor> TextSearch(this IQueryable<Monitor> monitors, string searchString)
        {
            IQueryable<Monitor> assetSearch = (monitors as IQueryable<Asset>)
                .TextSearch(searchString)
                .Cast<Monitor>();

            monitors = assetSearch.Concat(monitors.Where
                (p => p.MaxResolution.Contains(searchString)
                || p.Size.ToString().Contains(searchString)))
                .Distinct();

            return monitors;
        }
        public static IQueryable<Telephone> TextSearch(this IQueryable<Telephone> telephones, string searchString)
        {
            IQueryable<Telephone> assetSearch = (telephones as IQueryable<Asset>)
                .TextSearch(searchString)
                .Cast<Telephone>();

            telephones = assetSearch.Concat(telephones.Where
                (p => p.TelephoneType.Contains(searchString)
                || p.NumberIntern.Contains(searchString)
                || p.Number.Contains(searchString)))
                .Distinct();

            return telephones;
        }

        public static IQueryable<UsePeriod> TextSearch(this IQueryable<UsePeriod> useperiods, string searchString)
        {
            useperiods = useperiods.Where
                (u => u.UserAccount.Name.Contains(searchString)
                || u.Asset.Manufacturer.Contains(searchString)
                || u.Asset.ModelName.Contains(searchString)
                || u.Asset.SerialNumber.Contains(searchString)
                || u.Asset.Supplier.Contains(searchString)
                || u.Asset.AssetId.ToString().Contains(searchString)
                || u.Function.Contains(searchString)
                || u.Remark.Contains(searchString)
                || u.Status.Description.Contains(searchString)
                || (u.Asset as Computer).AntiVirus.Contains(searchString)
                || (u.Asset as Computer).Browser.Contains(searchString)
                || (u.Asset as Computer).ComputerName.Contains(searchString)
                || (u.Asset as Computer).ComputerType.Contains(searchString)
                || (u.Asset as Computer).OfficeVersion.Contains(searchString)
                || (u.Asset as Computer).OperatingSystem.Contains(searchString)
                || (u.Asset as Printer).PrinterName.Contains(searchString)
                || (u.Asset as Printer).DrumModel.Contains(searchString)
                || (u.Asset as Printer).IpAddress.Contains(searchString)
                || (u.Asset as Beamer).BeamerName.Contains(searchString)
                || (u.Asset as Monitor).MaxResolution.Contains(searchString)
                || (u.Asset as Monitor).Size.ToString().Contains(searchString)
                || (u.Asset as Telephone).TelephoneType.Contains(searchString)
                || (u.Asset as Telephone).Number.Contains(searchString)
                || (u.Asset as Telephone).NumberIntern.Contains(searchString)                
                    );
            return useperiods;
        }
        public static MemoryStream CompoundIdtoPDFStream(string compoundId)
        {
            Document document = new Document(new Rectangle(147f, 68f));
            document.SetMargins(0f, 0f, 0f, 0f);
            // string fontsfolder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Fonts);
            Font font = FontFactory.GetFont("Arial", 28, Color.BLACK);
            MemoryStream stream = new MemoryStream();

            try
            {
                PdfWriter pdfWriter = PdfWriter.GetInstance(document, stream);
                pdfWriter.CloseStream = false;
                Paragraph para = new Paragraph(compoundId, font);                            
                para.Alignment = Element.ALIGN_CENTER;
                document.Open();
                document.Add(para);
            }
            catch (DocumentException de)
            {
                Console.Error.WriteLine(de.Message);
            }
            catch (IOException ioe)
            {
                Console.Error.WriteLine(ioe.Message);
            }

            document.Close();

            stream.Flush();
            stream.Position = 0;
            return stream;
        }
        public static MemoryStream TelephoneListPDFStream(AssetManagerContext db)
        {
            MemoryStream stream = new MemoryStream();
            Document document = new Document();
            List<Group<string, Telephonelist>> listByDep = TelephoneListByDepartment(db);
            // Font font = FontFactory.GetFont("Arial", 28, Color.BLACK);          

            try
            {
                PdfWriter pdfWriter = PdfWriter.GetInstance(document, stream);
                pdfWriter.CloseStream = false;
                document.Open();            

                
                PdfPTable layoutTable = new PdfPTable(2);
                layoutTable.WidthPercentage = 100f;                
                
                PdfPCell cellTitle = CreateCell(5f, "Telefoonlijst");
                cellTitle.Colspan = 2;
                cellTitle.HorizontalAlignment = 1;
                layoutTable.AddCell(cellTitle);

                PdfPTable tblLeft = new PdfPTable(1);


                PdfPCell cellIntern = CreateCell(5f);
                Paragraph parInternTitle = new Paragraph();
                parInternTitle.Add("Interne nummers");
                parInternTitle.SpacingAfter = 10f;
                cellIntern.AddElement(parInternTitle);             

                // Paragraph paraIntern = new Paragraph();
                foreach (var dep in listByDep)
                {
                    cellIntern.AddElement(new Phrase(dep.Key));

                    PdfPTable table = new PdfPTable(2);
                    // table.SpacingBefore = 20f;
                    table.SpacingAfter = 10f;

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
                // cellIntern.AddElement(paraIntern);

                tblLeft.AddCell(cellIntern);
                tblLeft.AddCell(CreateCell(5f, "LOG"));
                PdfPCell nesthousing = new PdfPCell(tblLeft);
                nesthousing.Padding = 0f;
                layoutTable.AddCell(nesthousing);
                layoutTable.AddCell(CreateCell(5f, "GSM"));

                document.Add(layoutTable);
                // document.Add(paraIntern);

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
        private static PdfPCell CreateCell(float padding = 5f, string str = null)
        {
            var c = new PdfPCell();
            if (!string.IsNullOrEmpty(str))
            { c.Phrase = new Phrase(str); }
            c.Padding = padding;
            return c;
        }
        public static List<Group<string, Telephonelist>> TelephoneListByDepartment(AssetManagerContext db)
        {
            var currentPhones = db.UsePeriods
           .Include(u => u.Asset)
           .Include(u => u.Status)
           .Include(u => u.UserAccount)
           .Where(u => u.Asset is Telephone)
           .Where(u => u.EndDate == null || u.EndDate >= DateTime.Now)
           .Where(u => u.UserAccount != null)
            .Select(u => new Telephonelist
            {
                Department = u.UserAccount.Department,
                Name = u.UserAccount.Name,
                NumberIntern = (u.Asset as Telephone).NumberIntern
            });

            var grouped1 = from phone in currentPhones
                           group phone by phone.Department into groupedPhones
                           orderby groupedPhones.Key
                           select new Group<string, Telephonelist>
                           { Key = groupedPhones.Key, Values = groupedPhones.OrderBy(t => t.Name) };
            return grouped1.ToList();
        }
    }   
}