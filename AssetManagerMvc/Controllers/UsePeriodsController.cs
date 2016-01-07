using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AssetManagerMvc.Models;
using System.Text.RegularExpressions;
using static AssetManagerMvc.Models.CustomHelpers;

namespace AssetManagerMvc.Controllers
{
    [Authorize(Users = @"OWWOFT\sven, OWWOFT\miquel, OWWOFT\maurice, OWWOFT\kristof, OWWOFT\werner")]
    public class UsePeriodsController : Controller
    {
        private AssetManagerContext db = new AssetManagerContext();

        // GET: UsePeriods
        public ActionResult Index(string sortOrder, string searchString, bool? current,
            bool? hideUitGebruik, string category, bool? repair, int? assetId)
        {
            var usePeriods = db.UsePeriods
                .Include(u => u.Asset)
                .Include(u => u.Status)
                .Include(u => u.UserAccount)
                ;
            // set category to computers if we come from a computer page etc.
            if ((string.IsNullOrEmpty(category)) && (Request.UrlReferrer != null))
            {
                // "/AssetManager/" is in the path when running on server
                Match match = Regex.Match(Request.UrlReferrer.AbsolutePath, @"/AssetManager/([A-Za-z]+)");
                if (match.Success && match.Groups.Count > 1) { category = match.Groups[1].Value; }
                else
                {
                    // only"/" is in the path when running locally
                    match = Regex.Match(Request.UrlReferrer.AbsolutePath, @"/([A-Za-z]+)");
                    if (match.Success && match.Groups.Count > 1) { category = match.Groups[1].Value; }
                }
            }

            // filters
            switch (category)
            {
                case "Computers":
                    usePeriods = usePeriods.Where(u => u.Asset is Computer);
                    break;
                case "Printers":
                    usePeriods = usePeriods.Where(u => u.Asset is Printer);
                    break;
                case "Beamers":
                    usePeriods = usePeriods.Where(u => u.Asset is Beamer);
                    break;
                case "Monitors":
                    usePeriods = usePeriods.Where(u => u.Asset is Monitor);
                    break;
                case "Telephones":
                    usePeriods = usePeriods.Where(u => u.Asset is Telephone);
                    break;
                case "Network":
                    usePeriods = usePeriods.Where(u => u.Asset is Network);
                    break;
                case "Miscellaneous":
                    usePeriods = usePeriods.Where(u => u.Asset is Miscellaneous);
                    break;
                default:
                    // set category to computers if we come from e.g. RepairInfo
                    usePeriods = usePeriods.Where(u => u.Asset is Computer);
                    category = "Computers";
                    break;
            }
            // hideUitGebruik
            if ((hideUitGebruik == null) || (hideUitGebruik == true))
            { usePeriods = usePeriods.Where(up => up.Status.UsePeriodStatusId != 4); } // "uit gebruik"
            // current
            if ((current == null) || (current == true))
            {
                usePeriods = usePeriods.Where(up => up.EndDate == null ||
                up.EndDate >= DateTime.Now);
            }
            if (assetId.HasValue)
            {
                usePeriods = usePeriods.Where(u => u.AssetId == assetId.Value);
            }

            if (!String.IsNullOrEmpty(searchString))
            {
                usePeriods = usePeriods.TextSearch(searchString);
            }
            var categories = new SelectList
                (new string[] { "Computers", "Printers", "Beamers", "Monitors", "Telephones", "Network", "Miscellaneous" }, category);

            ViewBag.Filter = searchString;
            ViewBag.Current = current;
            ViewBag.HideUitGebruik = hideUitGebruik;
            ViewBag.CategorySelectList = categories;
            ViewBag.SelectedCategory = category;

            // sorting
            ViewBag.CompoundIdSortParm = String.IsNullOrEmpty(sortOrder) ? "compoundId_desc" : "";
            ViewBag.ComputerNameSortParm = sortOrder == "computername" ? "computername_desc" : "computername";
            ViewBag.PrinterNameSortParm = sortOrder == "printername" ? "printername_desc" : "printername";
            ViewBag.BeamerNameSortParm = sortOrder == "beamername" ? "beamername_desc" : "beamername";
            ViewBag.SerialNumberSortParm = sortOrder == "serialnumber" ? "serialnumber_desc" : "serialnumber";
            ViewBag.NumberSortParm = sortOrder == "number" ? "number_desc" : "number";
            ViewBag.TelephoneTypeSortParm = sortOrder == "telephonetype" ? "telephonetype_desc" : "telephonetype";
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
                case "printername":
                    usePeriods = usePeriods.OrderBy(u => (u.Asset as Printer).PrinterName);
                    break;
                case "printername_desc":
                    usePeriods = usePeriods.OrderByDescending(u => (u.Asset as Printer).PrinterName);
                    break;
                case "beamername":
                    usePeriods = usePeriods.OrderBy(u => (u.Asset as Beamer).BeamerName);
                    break;
                case "beamername_desc":
                    usePeriods = usePeriods.OrderByDescending(u => (u.Asset as Beamer).BeamerName);
                    break;
                case "serialnumber":
                    usePeriods = usePeriods.OrderBy(u => u.Asset.SerialNumber);
                    break;
                case "serialnumber_desc":
                    usePeriods = usePeriods.OrderByDescending(u => u.Asset.SerialNumber);
                    break;
                case "number":
                    usePeriods = usePeriods.OrderBy(u => (u.Asset as Telephone).Number);
                    break;
                case "number_desc":
                    usePeriods = usePeriods.OrderByDescending(u => (u.Asset as Telephone).Number);
                    break;
                case "telephonetype":
                    usePeriods = usePeriods.OrderBy(u => (u.Asset as Telephone).TelephoneType);
                    break;
                case "telephonetype_desc":
                    usePeriods = usePeriods.OrderByDescending(u => (u.Asset as Telephone).TelephoneType);
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

            // repair doc
            if (repair == true)
            {
                ViewBag.RepairInfo = true;
                TempData.Keep("repairDoc");
            }
            List<UsePeriod> usePeriodsList = usePeriods.ToList();
            if (assetId.HasValue && String.IsNullOrEmpty(sortOrder)) { usePeriodsList.Sort(); }            
            return View(usePeriodsList);
        }

        // GET: UsePeriods/Details/5
        public ActionResult Details(int? id, string category)
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
        public ActionResult Create(string category, int? oldUsePeriodId)
        {
            UsePeriod up = new UsePeriod();
            up.StartDate = DateTime.Now;

            if (oldUsePeriodId != null)
            {
                UsePeriod oldUsePeriod = db.UsePeriods.Find(oldUsePeriodId);
                oldUsePeriod.EndDate = DateTime.Today;
                db.SaveChanges();
                SetCreateAndEditViewbag(category, oldUsePeriod.AssetId, null,
                    oldUsePeriod.UserAccountId, oldUsePeriod.Function);
            }
            else
            {
                SetCreateAndEditViewbag(category);
            }
            ViewBag.SelectedCategory = category;
            return View(up);
        }


        // POST: UsePeriods/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(string category, [Bind(Include = "UsePeriodId,UserAccountId,StartDate,EndDate,Remark,Function,UsePeriodStatusId,AssetId,UserIsAdmin")] UsePeriod usePeriod)
        {
            if (ModelState.IsValid)
            {
                db.UsePeriods.Add(usePeriod);
                db.SaveChanges();
                return RedirectToAction("Index", new { category = category });
            }

            SetCreateAndEditViewbag(category, usePeriod.AssetId, usePeriod.UsePeriodStatusId,
                usePeriod.UserAccountId, usePeriod.Function);
            ViewBag.SelectedCategory = category;
            return View(usePeriod);
        }

        // GET: UsePeriods/Edit/5
        public ActionResult Edit(int? id, string category)
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
            SetCreateAndEditViewbag(category, usePeriod.AssetId, usePeriod.UsePeriodStatusId,
                usePeriod.UserAccountId, usePeriod.Function);
            ViewBag.SelectedCategory = category;
            return View(usePeriod);
        }

        // POST: UsePeriods/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string category, [Bind(Include = "UsePeriodId,UserAccountId,StartDate,EndDate,Remark,Function,UsePeriodStatusId,AssetId,UserIsAdmin")] UsePeriod usePeriod)
        {
            if (ModelState.IsValid)
            {
                db.Entry(usePeriod).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { category = category });
            }
            SetCreateAndEditViewbag(category, usePeriod.AssetId, usePeriod.UsePeriodStatusId,
                  usePeriod.UserAccountId, usePeriod.Function);
            ViewBag.SelectedCategory = category;
            return View(usePeriod);
        }
        private void SetCreateAndEditViewbag(string category = null, int? assetId = null, int? usePeriodStatusId = null,
            int? userAccountId = null, string function = null)
        {
            // ViewBag.AssetId = GenericSelectList(db, Type.GetType(category))
            switch (category)
            {
                case "Computers":
                    if (assetId == null)
                    { ViewBag.AssetId = new SelectList(db.Assets.Where(x => x is Computer), "AssetId", "CompoundIdAndSerialNumber"); }
                    else
                    { ViewBag.AssetId = new SelectList(db.Assets.Where(x => x is Computer), "AssetId", "CompoundIdAndSerialNumber", assetId); }
                    break;
                case "Printers":
                    if (assetId == null)
                    { ViewBag.AssetId = new SelectList(db.Assets.Where(x => x is Printer), "AssetId", "CompoundIdAndSerialNumber"); }
                    else
                    { ViewBag.AssetId = new SelectList(db.Assets.Where(x => x is Printer), "AssetId", "CompoundIdAndSerialNumber", assetId); }
                    break;
                case "Beamers":
                    if (assetId == null)
                    { ViewBag.AssetId = new SelectList(db.Assets.Where(x => x is Beamer), "AssetId", "CompoundIdAndSerialNumber"); }
                    else
                    { ViewBag.AssetId = new SelectList(db.Assets.Where(x => x is Beamer), "AssetId", "CompoundIdAndSerialNumber", assetId); }
                    break;
                case "Monitors":
                    if (assetId == null)
                    { ViewBag.AssetId = new SelectList(db.Assets.Where(x => x is Monitor), "AssetId", "CompoundIdAndSerialNumber"); }
                    else
                    { ViewBag.AssetId = new SelectList(db.Assets.Where(x => x is Monitor), "AssetId", "CompoundIdAndSerialNumber", assetId); }
                    break;
                case "Telephones":
                    if (assetId == null)
                    { ViewBag.AssetId = new SelectList(db.Assets.Where(x => x is Telephone), "AssetId", "CompoundIdAndNumberIntern"); }
                    else
                    { ViewBag.AssetId = new SelectList(db.Assets.Where(x => x is Telephone), "AssetId", "CompoundIdAndNumberIntern", assetId); }
                    break;
                case "Network":
                    if (assetId == null)
                    { ViewBag.AssetId = new SelectList(db.Assets.Where(x => x is Network), "AssetId", "CompoundIdAndNumberIntern"); }
                    else
                    { ViewBag.AssetId = new SelectList(db.Assets.Where(x => x is Network), "AssetId", "CompoundIdAndNumberIntern", assetId); }
                    break;
                case "Miscellaneous":
                    if (assetId == null)
                    { ViewBag.AssetId = new SelectList(db.Assets.Where(x => x is Miscellaneous), "AssetId", "CompoundIdAndNumberIntern"); }
                    else
                    { ViewBag.AssetId = new SelectList(db.Assets.Where(x => x is Miscellaneous), "AssetId", "CompoundIdAndNumberIntern", assetId); }
                    break;
                default:
                    ViewBag.AssetId = new SelectList(db.Assets, "AssetId", "CompoundIdAndSerialNumber");
                    break;
            }
            if (usePeriodStatusId == null)
            {
                ViewBag.UsePeriodStatusId = new SelectList(db.UsePeriodStatuses, "UsePeriodStatusId", "Description");
            }
            else
            {
                ViewBag.UsePeriodStatusId = new SelectList(db.UsePeriodStatuses, "UsePeriodStatusId", "Description", usePeriodStatusId);
            }
            if (userAccountId == null)
            {
                ViewBag.UserAccountId = new SelectList(db.UserAccounts.OrderBy(x => x.Name), "UserAccountId", "Name").ToList();
            }
            else
            {
                ViewBag.UserAccountId = new SelectList(db.UserAccounts.OrderBy(x => x.Name), "UserAccountId", "Name", userAccountId).ToList();
            }
            ViewBag.UserAccountId.Insert(0, new SelectListItem { Text = "", Value = "" });

            if (string.IsNullOrEmpty(function))
            {
                ViewBag.Function = new SelectList(db.UsePeriods, "Function", "Function")
                .GroupBy(f => f.Text).Select(f => f.First()) // == Distinct              
                .OrderBy(f => f.Text);
            }
            else
            {
                ViewBag.Function = new SelectList(db.UsePeriods, "Function", "Function", function)
                .GroupBy(f => f.Text).Select(f => f.First()) // == Distinct              
                .OrderBy(f => f.Text);
            }
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
