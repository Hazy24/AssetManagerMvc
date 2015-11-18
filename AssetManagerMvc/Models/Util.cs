﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iTextSharp.text;
using iTextSharp.text.pdf;

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
                || (u.Asset as Printer).TonerModel.Contains(searchString)
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
    }
}