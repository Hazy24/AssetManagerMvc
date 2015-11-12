using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AssetManagerMvc.Models;

namespace AssetManagerMvc.Controllers
{
    public class UsePeriodsPrintersController : Controller
    {
        private AssetManagerContext db = new AssetManagerContext();

        // GET: UsePeriodsPrinters
        public ActionResult Index(string sortOrder, string searchString, bool? current,
            bool? hideUitGebruik)
        {
            var usePeriods = db.UsePeriods
                .Include(u => u.Asset)
                .Include(u => u.Status)
                .Include(u => u.UserAccount)
                .Where(u => u.Asset is Printer)
                ;
            if ((hideUitGebruik == null) || (hideUitGebruik == true))
            { usePeriods = usePeriods.Where(up => up.Status.UsePeriodStatusId != 4); } // "uit gebruik"

            if (!String.IsNullOrEmpty(searchString))
            {
                usePeriods = usePeriods.TextSearch(searchString);                  
            }
            if ((current == null) || (current == true))
            {
                usePeriods = usePeriods.Where(up => up.EndDate == null ||
                up.EndDate >= DateTime.Now);
            }
            ViewBag.Filter = searchString;
            ViewBag.Current = current;
            ViewBag.HideUitGebruik = hideUitGebruik;

            ViewBag.CompoundIdSortParm = String.IsNullOrEmpty(sortOrder) ? "compoundId_desc" : "";
            ViewBag.PrinterNameSortParm = sortOrder == "printername" ? "printername_desc" : "printername";
            ViewBag.SerialNumberSortParm = sortOrder == "serialnumber" ? "serialnumber_desc" : "serialnumber";
            ViewBag.DescriptionSortParm = sortOrder == "description" ? "description_desc" : "description";
            ViewBag.FullNameSortParm = sortOrder == "fullname" ? "fullname_desc" : "fullname";
            ViewBag.FunctionSortParm = sortOrder == "function" ? "function_desc" : "function";

            switch (sortOrder)
            {
                case "compoundId_desc":
                    usePeriods = usePeriods.OrderByDescending(u => u.AssetId);
                    break;
                case "printername":
                    usePeriods = usePeriods.OrderBy(u => (u.Asset as Printer).PrinterName);
                    break;
                case "printername_desc":
                    usePeriods = usePeriods.OrderByDescending(u => (u.Asset as Printer).PrinterName);
                    break;
                case "serialnumber":
                    usePeriods = usePeriods.OrderBy(u => u.Asset.SerialNumber);
                    break;
                case "serialnumber_desc":
                    usePeriods = usePeriods.OrderByDescending(u => u.Asset.SerialNumber);
                    break;
                case "description":
                    usePeriods = usePeriods.OrderBy(u => u.Status.Description);
                    break;
                case "description_desc":
                    usePeriods = usePeriods.OrderByDescending(u => u.Status.Description);
                    break;
                case "fullname":
                    usePeriods = usePeriods.OrderBy(u => u.UserAccount.Name);
                    break;
                case "fullname_desc":
                    usePeriods = usePeriods.OrderByDescending(u => u.UserAccount.Name);
                    break;
                case "function":
                    usePeriods = usePeriods.OrderBy(u => u.Function);
                    break;
                case "function_desc":
                    usePeriods = usePeriods.OrderByDescending(u => u.Function);
                    break;
                default:  // compoundId ascending 
                    usePeriods = usePeriods.OrderBy(u => u.AssetId);
                    break;
            }


            return View(usePeriods.ToList());
        }

        // GET: UsePeriodsPrinters/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UsePeriod usePeriod = db.UsePeriods.Find(id);
            if (usePeriod == null)
            {
                return HttpNotFound();
            }
            return View(usePeriod);
        }

        // GET: UsePeriodsPrinters/Create
        public ActionResult Create()
        {
            ViewBag.AssetId = new SelectList(db.Assets, "AssetId", "SerialNumber");
            ViewBag.UsePeriodStatusId = new SelectList(db.UsePeriodStatuses, "UsePeriodStatusId", "Description");
            ViewBag.UserAccountId = new SelectList(db.UserAccounts, "UserAccountId", "Name");
            return View();
        }

        // POST: UsePeriodsPrinters/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UsePeriodId,UserAccountId,StartDate,EndDate,Remark,Function,UserIsAdmin,UsePeriodStatusId,AssetId")] UsePeriod usePeriod)
        {
            if (ModelState.IsValid)
            {
                db.UsePeriods.Add(usePeriod);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AssetId = new SelectList(db.Assets, "AssetId", "SerialNumber", usePeriod.AssetId);
            ViewBag.UsePeriodStatusId = new SelectList(db.UsePeriodStatuses, "UsePeriodStatusId", "Description", usePeriod.UsePeriodStatusId);
            ViewBag.UserAccountId = new SelectList(db.UserAccounts, "UserAccountId", "Name", usePeriod.UserAccountId);
            return View(usePeriod);
        }

        // GET: UsePeriodsPrinters/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UsePeriod usePeriod = db.UsePeriods.Find(id);
            if (usePeriod == null)
            {
                return HttpNotFound();
            }
            ViewBag.AssetId = new SelectList(db.Assets, "AssetId", "SerialNumber", usePeriod.AssetId);
            ViewBag.UsePeriodStatusId = new SelectList(db.UsePeriodStatuses, "UsePeriodStatusId", "Description", usePeriod.UsePeriodStatusId);
            ViewBag.UserAccountId = new SelectList(db.UserAccounts, "UserAccountId", "Name", usePeriod.UserAccountId);
            return View(usePeriod);
        }

        // POST: UsePeriodsPrinters/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UsePeriodId,UserAccountId,StartDate,EndDate,Remark,Function,UserIsAdmin,UsePeriodStatusId,AssetId")] UsePeriod usePeriod)
        {
            if (ModelState.IsValid)
            {
                db.Entry(usePeriod).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AssetId = new SelectList(db.Assets, "AssetId", "SerialNumber", usePeriod.AssetId);
            ViewBag.UsePeriodStatusId = new SelectList(db.UsePeriodStatuses, "UsePeriodStatusId", "Description", usePeriod.UsePeriodStatusId);
            ViewBag.UserAccountId = new SelectList(db.UserAccounts, "UserAccountId", "Name", usePeriod.UserAccountId);
            return View(usePeriod);
        }

        // GET: UsePeriodsPrinters/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UsePeriod usePeriod = db.UsePeriods.Find(id);
            if (usePeriod == null)
            {
                return HttpNotFound();
            }
            return View(usePeriod);
        }

        // POST: UsePeriodsPrinters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            UsePeriod usePeriod = db.UsePeriods.Find(id);
            db.UsePeriods.Remove(usePeriod);
            db.SaveChanges();
            return RedirectToAction("Index");
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
