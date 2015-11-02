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
    public class UsePeriodsController : Controller
    {
        private AssetManagerContext db = new AssetManagerContext();

        // GET: UsePeriods
        public ActionResult Index(string sortOrder, string searchString)
        {
            var usePeriods = db.UsePeriods
                .Include(u => u.Asset)
                .Include(u => u.Status)
                .Include(u => u.UserAccount)
                // .Where(u => u.Asset is Computer)
                // .Where(u => u.StartDate <= DateTime.Now || u.StartDate == null)
                // .Where(u => u.EndDate > DateTime.Now || u.EndDate == null)                
                ;
            if (!String.IsNullOrEmpty(searchString))
            {
                usePeriods = usePeriods.Where(u => u.UserAccount.Name.Contains(searchString)
                || (u.Asset as Computer).ComputerName.Contains(searchString)
                || (u.Asset as Computer).ComputerType.Contains(searchString)
                || u.Asset.Manufacturer.Contains(searchString)
                || u.Asset.ModelName.Contains(searchString)
                || u.Asset.SerialNumber.Contains(searchString)
                || u.Asset.Supplier.Contains(searchString)
                || u.Asset.AssetId.ToString().Contains(searchString)
                || u.Function.Contains(searchString)
                || u.Remark.Contains(searchString)
                || u.Status.Description.Contains(searchString)
                    );
            }
            ViewBag.CurrentFilter = searchString;

            ViewBag.CompoundIdSortParm = String.IsNullOrEmpty(sortOrder) ? "compoundId_desc" : "";
            ViewBag.ComputerNameSortParm = sortOrder == "computername" ? "computername_desc" : "computername";
            ViewBag.SerialNumberSortParm = sortOrder == "serialnumber" ? "serialnumber_desc" : "serialnumber";
            ViewBag.DescriptionSortParm = sortOrder == "description" ? "description_desc" : "description";
            ViewBag.FullNameSortParm = sortOrder == "fullname" ? "fullname_desc" : "fullname";
            ViewBag.FunctionSortParm = sortOrder == "function" ? "function_desc" : "function";



            switch (sortOrder)
            {
                case "compoundId_desc":
                    usePeriods = usePeriods.OrderByDescending(u => u.AssetId);
                    break;
                case "computername":
                    usePeriods = usePeriods.OrderBy(u => (u.Asset as Computer).ComputerName);
                    break;
                case "computername_desc":
                    usePeriods = usePeriods.OrderByDescending(u => (u.Asset as Computer).ComputerName);
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

        // GET: UsePeriods/Details/5
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

        // GET: UsePeriods/Create
        public ActionResult Create()
        {
            UsePeriod up = new UsePeriod();
            up.StartDate = DateTime.Now;

            ViewBag.AssetId = new SelectList(db.Assets, "AssetId", "CompoundIdAndSerialNumber");
            ViewBag.UsePeriodStatusId = new SelectList(db.UsePeriodStatuses, "UsePeriodStatusId", "Description");
            ViewBag.UserAccountId = new SelectList(db.UserAccounts, "UserAccountId", "Name").ToList();            
            ViewBag.UserAccountId.Insert(0, new SelectListItem { Text = "", Value = "" });

            return View(up);
        }

        // POST: UsePeriods/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UsePeriodId,UserAccountId,StartDate,EndDate,Remark,Function,UsePeriodStatusId,AssetId,UserIsAdmin")] UsePeriod usePeriod)
        {
            if (ModelState.IsValid)
            {
                db.UsePeriods.Add(usePeriod);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AssetId = new SelectList(db.Assets, "AssetId", "CompoundIdAndSerialNumber", usePeriod.AssetId);
            ViewBag.UsePeriodStatusId = new SelectList(db.UsePeriodStatuses, "UsePeriodStatusId", "Description", usePeriod.UsePeriodStatusId);
            ViewBag.UserAccountId = new SelectList(db.UserAccounts, "UserAccountId", "Name", usePeriod.UserAccountId).ToList();
            ViewBag.UserAccountId.Insert(0, new SelectListItem { Text = "", Value = "" });
            return View(usePeriod);
        }

        // GET: UsePeriods/Edit/5
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
            ViewBag.AssetId = new SelectList(db.Assets, "AssetId", "CompoundIdAndSerialNumber", usePeriod.AssetId);
            ViewBag.UsePeriodStatusId = new SelectList(db.UsePeriodStatuses, "UsePeriodStatusId", "Description", usePeriod.UsePeriodStatusId);

            ViewBag.UserAccountId = new SelectList(db.UserAccounts, "UserAccountId", "Name", usePeriod.UserAccountId).ToList();
            ViewBag.UserAccountId.Insert(0, new SelectListItem { Text = "", Value = "" });

            return View(usePeriod);
        }

        // POST: UsePeriods/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UsePeriodId,UserAccountId,StartDate,EndDate,Remark,Function,UsePeriodStatusId,AssetId,UserIsAdmin")] UsePeriod usePeriod)
        {
            if (ModelState.IsValid)
            {
                db.Entry(usePeriod).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AssetId = new SelectList(db.Assets, "AssetId", "CompoundIdAndSerialNumber", usePeriod.AssetId);
            ViewBag.UsePeriodStatusId = new SelectList(db.UsePeriodStatuses, "UsePeriodStatusId", "Description", usePeriod.UsePeriodStatusId);
            ViewBag.UserAccountId = new SelectList(db.UserAccounts, "UserAccountId", "Name", usePeriod.UserAccountId).ToList();
            ViewBag.UserAccountId.Insert(0, new SelectListItem { Text = "", Value = "" });
            return View(usePeriod);
        }

        // GET: UsePeriods/Delete/5
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

        // POST: UsePeriods/Delete/5
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
