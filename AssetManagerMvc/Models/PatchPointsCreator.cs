using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Excel = Microsoft.Office.Interop.Excel;
using System.Data.Entity;

namespace AssetManagerMvc.Models
{
    public static class PatchPointsCreator
    {
        public static List<PatchPoint> GetPatchPoints(AssetManagerContext db)
        {
            List<PatchPoint> patchPoints = new List<PatchPoint>();
            string excelFileName = "\\\\vmfile\\users\\sven\\My Documents\\patchpuntjes.xlsx";
            Excel.Application xlApp = new Excel.Application();
            object misValue = System.Reflection.Missing.Value;

            Excel.Workbook xlWorkBook = xlApp.Workbooks.Open(excelFileName);
            Excel.Worksheet xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

            int startRow = 2;
            if (xlWorkSheet != null)
            {
                Excel.Range usedRange = xlWorkSheet.UsedRange;
                PatchPoint pp;

                for (int rowCount = startRow; rowCount <= usedRange.Rows.Count; rowCount++)
                {
                    pp = GetPatchPointFromRange(usedRange, rowCount);
                    if (pp.Number > 0)
                    {
                        patchPoints.Add(pp);
                        db.PatchPoints.Add(pp);
                    }
                }
                db.SaveChanges();

            }

            xlWorkBook.Close(true, misValue, misValue);
            xlApp.Quit();

            releaseObject(xlWorkSheet);
            releaseObject(xlWorkBook);
            releaseObject(xlApp);

            return patchPoints;
        }

        private static PatchPoint GetPatchPointFromRange(Excel.Range usedRange, int rowCount)
        {
            PatchPoint pp = new PatchPoint();

            if (usedRange.Cells[rowCount, 1].Value2 is double)
            { pp.Number = (int)usedRange.Cells[rowCount, 1].Value2; }

            if (usedRange.Cells[rowCount, 2].Value2 is double)
            { pp.Floor = (int)usedRange.Cells[rowCount, 2].Value2; }

            pp.RoomName = usedRange.Cells[rowCount, 3].Value2 as string;

            if (usedRange.Cells[rowCount, 4].Value2 is double)
            { pp.RoomNumber = (int)usedRange.Cells[rowCount, 4].Value2; }

            pp.Tile = usedRange.Cells[rowCount, 5].Value2 as string;


            pp.Function = usedRange.Cells[rowCount, 6].Value2 as string;
            //if (usedRange.Cells[rowCount, 6].Value2 is string)
            //{
            //    Asset asset = FindAsset(usedRange.Cells[rowCount, 6].Value2 as string, db);
            //    if (asset.AssetId != 0) { pp.Asset = asset; }
            //}
            return pp;
        }

        // returns asset with AssetId == 0 if no matching asset is found
        private static Asset FindAsset(string str, AssetManagerContext db)
        {
            Asset asset = new Asset();

            var usePeriods = db.UsePeriods
                .Include(u => u.Asset)
                .Include(u => u.Status)
                .Include(u => u.UserAccount)
                ;

            string[] substrings = str.Split(' ');

            if (substrings.Length > 1)
            {
                string assetType = substrings[0];
                string accountFirstName = substrings[1];
                usePeriods = usePeriods.Where(u => u.UserAccount.Name.Contains(accountFirstName))
                .Where(u => u.EndDate == null || u.EndDate >= DateTime.Now) // current
                .Where(u => u.Status.UsePeriodStatusId != 4) // not "uit gebruik"                
                ;
                switch (substrings[0])
                {
                    case "D":
                        usePeriods = usePeriods.Where(u => u.Asset is Computer);
                        break;
                    case "T":
                        usePeriods = usePeriods.Where(u => u.Asset is Telephone);
                        break;
                    case "V":
                        usePeriods = usePeriods.Where(u => u.Asset is Telephone);
                        break;
                    case "P":
                        usePeriods = usePeriods.Where(u => u.Asset is Printer);
                        break;
                    default:
                        break;
                }
                // var upList = usePeriods.ToList();
                if (usePeriods.Count() == 1) { asset = usePeriods.First().Asset; }
            }
            return asset;
        }

        private static void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                throw ex;
            }
            finally
            {
                GC.Collect();
            }
        }
    }
}