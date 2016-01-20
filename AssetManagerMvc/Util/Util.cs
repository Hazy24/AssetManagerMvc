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
using System.Data.SqlClient;
using System.Configuration;
using System.Data.Entity.Validation;
using System.Diagnostics;

namespace AssetManagerMvc.Models
{
    public static class Util
    {
        public static IQueryable<Asset> TextSearch(this IQueryable<Asset> assets, string searchString)
        {
            assets = assets.Where(a => a.AssetId.ToString().Contains(searchString)
              || a.CompoundId.Contains(searchString)
              || a.Manufacturer.Contains(searchString)
              || a.ModelName.Contains(searchString)
              || a.Owner.Contains(searchString)
              || a.SerialNumber.Contains(searchString)
              || a.Remark.Contains(searchString)
              || a.IpAddress.Contains(searchString)
              || a.Supplier.Contains(searchString)
              || a.LogItems.Any(l => l.Text.Contains(searchString))
              );


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
              || c.ImageVersion.Contains(searchString)
              || c.QualityCheck.Contains(searchString)
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
        public static IQueryable<Network> TextSearch(this IQueryable<Network> networks, string searchString)
        {
            IQueryable<Network> assetSearch = (networks as IQueryable<Asset>)
                .TextSearch(searchString)
                .Cast<Network>();

            networks = assetSearch.Concat(networks.Where
                (n => n.NetworkName.Contains(searchString)))
                .Distinct();
            return networks;
        }
        public static IQueryable<Miscellaneous> TextSearch(this IQueryable<Miscellaneous> misc, string searchString)
        {
            return (misc as IQueryable<Asset>)
                .TextSearch(searchString)
                .Cast<Miscellaneous>();
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
        public static IQueryable<PatchPoint> TextSearch(this IQueryable<PatchPoint> patchpoints, string searchString)

        {
            int number;
            if (Int32.TryParse(searchString, out number))
            {
                patchpoints = patchpoints.Where
                     (pp => pp.Floor == number
                     || pp.Number == number
                      || pp.RoomNumber == number
                      );
            }
            else
            {
                patchpoints = patchpoints.Where
                    (pp => pp.Function.Contains(searchString)
                    || pp.Remark.Contains(searchString)
                    || pp.RoomName.Contains(searchString)
                    || pp.Tile.Contains(searchString)
                    );
            }
            return patchpoints;
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

        public static List<AssetSelectListItem> CompoundIdAndUserAccountNameOrFunction(AssetManagerContext db)
        {

            var usePeriods = db.UsePeriods
              .Include(u => u.Asset)
              .Include(u => u.Status)
              .Include(u => u.UserAccount)
              .Where(u => u.EndDate == null || u.EndDate >= DateTime.Now) // current
              .Select(u => new { CompoundId = u.Asset.CompoundId, Name = u.UserAccount.Name, Function = u.Function })
              ;

            List<AssetSelectListItem> list = new List<AssetSelectListItem>();
            foreach (var item in usePeriods)
            {
                AssetSelectListItem asli = new AssetSelectListItem();
                asli.CompoundId = item.CompoundId;
                if (string.IsNullOrEmpty(item.Name)) { asli.Identifier = item.Function; }
                else { asli.Identifier = item.Name; }
            }


            return list;
        }

        public static void CopyTable(string sourceConnectionString,
            string destinationConnectionString, string tableName)
        {
            // Create source connection
            SqlConnection source = new SqlConnection(ConfigurationManager.ConnectionStrings[sourceConnectionString].ConnectionString);
            // Create destination connection
            SqlConnection destination = new SqlConnection(ConfigurationManager.ConnectionStrings[destinationConnectionString].ConnectionString);
            // Clean up destination table. Your destination database must have the 
            // table with schema which you are copying data to. 
            // Before executing this code, you must create a table BulkDataTable 
            // in your database where you are trying to copy data to.

            SqlCommand cmd = new SqlCommand("DELETE FROM " + tableName, destination);

            // Open source and destination connections.
            source.Open();
            destination.Open();
            cmd.ExecuteNonQuery();

            cmd = new SqlCommand("SELECT * FROM " + tableName, source);
            // Execute reader
            SqlDataReader reader = cmd.ExecuteReader();
            // Create SqlBulkCopy
            SqlBulkCopy bulkData = new SqlBulkCopy(destination,
                SqlBulkCopyOptions.KeepIdentity, null);

            // Set destination table name
            bulkData.DestinationTableName = tableName;
            // Write data
            bulkData.WriteToServer(reader);
            // Close objects
            bulkData.Close();
            destination.Close();
            source.Close();
        }
        public static void CopyDataFromServerDbToLocalDb()
        {
            if (!ConfigurationManager.ConnectionStrings["AssetManagerContextLocal"]
                .ConnectionString.Contains("Data Source=(LocalDb)"))
            { return; }
            ClearAllTablesLocalDb();
            CopyTable("AssetManagerContextServer", "AssetManagerContextLocal", "UseperiodStatus");
            CopyTable("AssetManagerContextServer", "AssetManagerContextLocal", "Assets");
            CopyTable("AssetManagerContextServer", "AssetManagerContextLocal", "UserAccounts");
            CopyTable("AssetManagerContextServer", "AssetManagerContextLocal", "Departments");
            CopyTable("AssetManagerContextServer", "AssetManagerContextLocal", "PatchPoints");
            CopyTable("AssetManagerContextServer", "AssetManagerContextLocal", "UsePeriods");

        }
        private static void ClearAllTablesLocalDb()
        {
            SqlConnection local = new SqlConnection(ConfigurationManager
                .ConnectionStrings["AssetManagerContextLocal"].ConnectionString);

            local.Open();
            SqlCommand cmd = new SqlCommand("DELETE FROM UsePeriods", local);
            cmd.ExecuteNonQuery();
            cmd = new SqlCommand("DELETE FROM UseperiodStatus", local);
            cmd.ExecuteNonQuery();
            cmd = new SqlCommand("DELETE FROM Assets", local);
            cmd.ExecuteNonQuery();
            cmd = new SqlCommand("DELETE FROM UserAccounts", local);
            cmd.ExecuteNonQuery();
            cmd = new SqlCommand("DELETE FROM Departments", local);
            cmd.ExecuteNonQuery();
            cmd = new SqlCommand("DELETE FROM PatchPoints", local);
            cmd.ExecuteNonQuery();

            local.Close();
        }

        /// <summary>
        /// returns a distinct and ordered SelectList
        /// </summary>

        public static IOrderedEnumerable<SelectListItem> DOSelectList(IEnumerable<object> items, string dataValueField,
    string dataTextField, object selectedValue = null)
        {
            IOrderedEnumerable<SelectListItem> selectList;

            if (selectedValue == null)
            {
                selectList = new SelectList(items, dataValueField, dataTextField)
                .GroupBy(f => f.Text).Select(f => f.First()) // == Distinct              
                .OrderBy(f => f.Text);
            }
            else
            {
                selectList = new SelectList(items, dataValueField, dataTextField, selectedValue)
                .GroupBy(f => f.Text).Select(f => f.First()) // == Distinct              
                .OrderBy(f => f.Text);
            }
            return selectList;
        }

        public static void RemoveSynonyms(AssetManagerContext db)
        {
            try
            {
                foreach (Asset a in db.Assets)
                {
                    CheckRequiredProps(a);
                }

                var assetsWithCvbaOwner = db.Assets
                       .Where(a => a.Owner == "cvba")
                       ;

                foreach (Asset a in assetsWithCvbaOwner)
                {
                    CheckRequiredProps(a);
                    a.Owner = "cv";
                }

                var assetsWithAltanHP = db.Assets
                    .Where(a => a.Supplier == "Altan/HP")
                    ;
                foreach (Asset a in assetsWithAltanHP)
                {
                    a.Supplier = "Altan";
                }

                var computersWin7 = db.Computers
                    .Where(c => c.OperatingSystem == "win 7 pro"
                    || c.OperatingSystem == "windows 7");

                foreach (Computer c in computersWin7)
                {
                    c.OperatingSystem = "Win 7";
                }

                var computersMSE = db.Computers
                   .Where(c => c.AntiVirus == "MS sec"
                   || c.AntiVirus == "mse");

                foreach (Computer c in computersMSE)
                {
                    c.AntiVirus = "MSE";
                }

                db.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Debug.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Debug.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
        }
        private static void CheckRequiredProps(Asset a)
        {
            if (string.IsNullOrEmpty(a.SerialNumber)) a.SerialNumber = "?";
            if (a is Computer)
            {
                Computer c = (Computer)a;
                if (string.IsNullOrEmpty(c.ComputerType)) c.ComputerType = "?";
            }
        }
        public static void InsertCompoundIdsInDb(AssetManagerContext db)
        {
            foreach (var c in db.Computers)
            {
                c.CompoundId = "C" + c.AssetId;
            }
            foreach (var p in db.Printers)
            {
                p.CompoundId = "P" + p.AssetId;
            }
            foreach (var b in db.Beamers)
            {
                b.CompoundId = "B" + b.AssetId;
            }
            foreach (var m in db.Monitors)
            {
                m.CompoundId = "M" + m.AssetId;
            }
            foreach (var t in db.Telephones)
            {
                t.CompoundId = "T" + t.AssetId;
            }
            foreach (var n in db.Networks)
            {
                n.CompoundId = "N" + n.AssetId;
            }
            foreach (var a in db.Miscellaneous)
            {
                a.CompoundId = "A" + a.AssetId;
            }
            db.SaveChanges();
        }
    }
}