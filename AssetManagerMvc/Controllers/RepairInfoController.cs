using AssetManagerMvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using Novacode;
using System.IO;

namespace AssetManagerMvc.Controllers
{
    public class RepairInfoController : Controller
    {
        private AssetManagerContext db = new AssetManagerContext();
        // GET: RepairInfo
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.CompoundId = new SelectList(db.Assets.Where(x => x is Computer), "CompoundId", "CompoundIdAndSerialNumber");
            return View(new RepairInfo());
        }
        [HttpPost]
        public ActionResult Index(RepairInfo repairInfo)
        {
            int assetId = int.Parse(repairInfo.CompoundId.Substring(1));
            var usePeriods = db.UsePeriods
               .Include(u => u.Asset)
               .Where(u => u.Asset.AssetId == assetId)
               .Where(up => up.EndDate == null || up.EndDate >= DateTime.Now)
               .ToList();

            UsePeriod newUp;
            if (usePeriods.Count > 0)
            {
                newUp = usePeriods[0].Copy();
                newUp.UsePeriodStatusId = 3;    // statusId 3 = binnen ter herstelling
                foreach (UsePeriod up in usePeriods)
                {
                    up.EndDate = DateTime.Now;
                }
            }
            else
            {
                newUp = new UsePeriod(db.Assets.Find(assetId), 3); // statusId 3 = binnen ter herstelling
            }
            newUp.StartDate = DateTime.Now;
            if (string.IsNullOrWhiteSpace(newUp.Remark))
            {
                newUp.Remark = repairInfo.Remark;
            }
            else
            {
                newUp.Remark += Environment.NewLine +  repairInfo.Remark;
            }

            db.UsePeriods.Add(newUp);
            db.SaveChanges();


            string fileName = @"S:\My Documents\deliverInfo.docx";
            MemoryStream ms = new MemoryStream();
            
            using (DocX document = DocX.Load(fileName))
            {               
                document.ReplaceText("%Date%", string.Format("{0:d}", repairInfo.Date));
                document.ReplaceText("%CompoundId%", repairInfo.CompoundId);
                document.ReplaceText("%Remark%", repairInfo.Remark);
                document.ReplaceText("%Reason%", repairInfo.Reason);
                

                // document.SaveAs(@"S:\My Documents\delivered.docx");

                document.SaveAs(ms);
                ms.Flush();
                ms.Position = 0;

                return File(ms, "application/msword", repairInfo.CompoundId + ".docx");                
            }       
            
        }
    }
}
