using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
    }
}