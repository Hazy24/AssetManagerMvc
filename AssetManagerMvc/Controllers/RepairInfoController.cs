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
            
            var useperiods = db.UsePeriods
                .Where(up => up.EndDate == null || up.EndDate >= DateTime.Now)
                .Where(up => up.Asset is Computer)
                .Select(up => new { AssetId = up.AssetId, UserName = up.UserAccount.Name, Function = up.Function })
                .ToList()
                ;

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

                if (usePeriods[0].UserAccount != null)
                {
                    repairInfo.UserNameFunction = usePeriods[0].UserAccount.Name;
                    if (!string.IsNullOrWhiteSpace(usePeriods[0].Function))
                    { repairInfo.UserNameFunction += " - " + usePeriods[0].Function; }
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(usePeriods[0].Function))
                    { repairInfo.UserNameFunction = usePeriods[0].Function; }                   
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
                newUp.Remark += Environment.NewLine + repairInfo.Remark;
            }

            db.UsePeriods.Add(newUp);
            db.SaveChanges();


            string fileName = Server.MapPath("~/Docs/deliverInfo.docx");
            MemoryStream ms = new MemoryStream();

            using (DocX document = DocX.Load(fileName))
            {
                // set spaces for strings or ReplaceText will complain if they're empty
                if (string.IsNullOrEmpty(repairInfo.UserNameFunction)) { repairInfo.UserNameFunction = " "; }
                if (string.IsNullOrEmpty(repairInfo.Remark)) { repairInfo.Remark = " "; }
                if (string.IsNullOrEmpty(repairInfo.Reason)) { repairInfo.Reason = " "; }
                // ReplaceText
                document.ReplaceText("%Date%", string.Format("{0:d}", repairInfo.Date));                
                document.ReplaceText("%UserNameFunction%",  repairInfo.UserNameFunction);
                document.ReplaceText("%CompoundId%", repairInfo.CompoundId);                
                document.ReplaceText("%Remark%", repairInfo.Remark);                
                document.ReplaceText("%Reason%", repairInfo.Reason);              
                // prepare stream
                document.SaveAs(ms);
                ms.Flush();
                ms.Position = 0;
               // save filestream for next requests
                TempData["repairDoc"] = File(ms, "application/msword", repairInfo.CompoundId + ".docx"); 

            }

            return RedirectToAction("Index", "UsePeriods", new
            {
                searchString = assetId.ToString(),
                current = false,
                hideUitGebruik = false,
                category = "Computer", 
                repair = true
            });
        }
        public ActionResult Download()
        {         
            return (FileStreamResult)TempData["repairDoc"];
        }
    }
}
