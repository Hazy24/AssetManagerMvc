using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AssetManagerMvc.Models;
using static AssetManagerMvc.Models.CustomHelpers;
using System.Data.Entity.Validation;
using System.Diagnostics;

namespace AssetManagerMvc.Controllers
{
    [Authorize(Users = @"OWWOFT\sven, OWWOFT\miquel, OWWOFT\maurice, OWWOFT\kristof, OWWOFT\werner")]
    public class MiscellaneousController : Controller
    {
        private AssetManagerContext db = new AssetManagerContext();

        // GET: Miscellaneous
        public ActionResult Index(string sortOrder, string searchString)
        {
            var misc = from m in db.Miscellaneous
                       select m;

            if (!string.IsNullOrEmpty(searchString))
            {
                misc = misc.TextSearch(searchString);
            }
            ViewBag.CurrentFilter = searchString;

            ViewBag.CompoundIdSortParm = String.IsNullOrEmpty(sortOrder) ? "compoundId_desc" : "";
            ViewBag.MiscNameSortParm = sortOrder == "miscname" ? "miscname_desc" : "miscname";
            ViewBag.SerialNumberSortParm = sortOrder == "serialnumber" ? "serialnumber_desc" : "serialnumber";
            ViewBag.ManufacturerSortParm = sortOrder == "manufacturer" ? "manufacturer_desc" : "manufacturer";
            ViewBag.ModelNameSortParm = sortOrder == "modelname" ? "modelname_desc" : "modelname";
            ViewBag.PurchaseDateSortParm = sortOrder == "purchasedate" ? "purchasedate_desc" : "purchasedate";
            ViewBag.MiscTypeSortParm = sortOrder == "misctype" ? "misctype_desc" : "misctype";            

            switch (sortOrder)
            {
                case "compoundId_desc":
                    misc = misc.OrderByDescending(m => m.AssetId);
                    break;
                case "miscname":
                    misc = misc.OrderBy(m => m.MiscellaneousName);
                    break;
                case "miscname_desc":
                    misc = misc.OrderByDescending(m => m.MiscellaneousName);
                    break;
                case "serialnumber":
                    misc = misc.OrderBy(m => m.SerialNumber);
                    break;
                case "serialnumber_desc":
                    misc = misc.OrderByDescending(m => m.SerialNumber);
                    break;
                case "manufacturer":
                    misc = misc.OrderBy(m => m.Manufacturer);
                    break;
                case "manufacturer_desc":
                    misc = misc.OrderByDescending(m => m.Manufacturer);
                    break;
                case "modelname":
                    misc = misc.OrderBy(m => m.ModelName);
                    break;
                case "modelname_desc":
                    misc = misc.OrderByDescending(m => m.ModelName);
                    break;
                case "purchasedate":
                    misc = misc.OrderBy(m => m.PurchaseDate);
                    break;
                case "purchasedate_desc":
                    misc = misc.OrderByDescending(m => m.PurchaseDate);
                    break;
                case "misctype":
                    misc = misc.OrderBy(m => m.MiscellaneousType);
                    break;
                case "misctype_desc":
                    misc = misc.OrderByDescending(m => m.MiscellaneousType);
                    break;
               

                default:  // compoundId ascending 
                    misc = misc.OrderBy(m => m.AssetId);
                    break;
            }

            return View(misc);
        }

      

        //Print CompoundId to PDF
        public ActionResult PrintCompoundId(string compoundId)
        {
            return File(Util.CompoundIdtoPDFStream(compoundId), "application/pdf", compoundId + ".pdf");
        }

        // GET: Miscellaneous/Print/5
        public ActionResult Print(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Miscellaneous miscellaneous = db.Miscellaneous.Find(id);
            if (miscellaneous == null)
            {
                return HttpNotFound();
            }
            return View(miscellaneous);
        }

        // GET: Miscellaneous/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Miscellaneous miscellaneous = db.Miscellaneous.Find(id);
            if (miscellaneous == null)
            {
                return HttpNotFound();
            }
            return View(miscellaneous);
        }

        // GET: Miscellaneous/Create
        public ActionResult Create()
        {
            SetCreateAndEditViewbag(new Miscellaneous());
            return View();
        }

        // POST: Miscellaneous/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AssetId,SerialNumber,ModelName,PurchaseDate,PurchasePrice,Remark,Owner,Supplier,Manufacturer,MiscellaneousType,MiscellaneousName")] Miscellaneous miscellaneous)
        {
            if (ModelState.IsValid)
            {                
                db.Assets.Add(miscellaneous);
                UsePeriod up = new UsePeriod(miscellaneous, db.UsePeriodStatuses
                  .Where(x => x.Description == "nieuw toestel").First().UsePeriodStatusId);
                db.UsePeriods.Add(up);
                db.SaveChanges();
                miscellaneous.CompoundId = "A" + miscellaneous.AssetId;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            SetCreateAndEditViewbag(miscellaneous);
            return View(miscellaneous);
        }

        // GET: Miscellaneous/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Miscellaneous miscellaneous = db.Miscellaneous.Find(id);
            if (miscellaneous == null)
            {
                return HttpNotFound();
            }
            SetCreateAndEditViewbag(miscellaneous);
            return View(miscellaneous);
        }

        // POST: Miscellaneous/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AssetId,CompoundId,SerialNumber,ModelName,PurchaseDate,PurchasePrice,Remark,Owner,Supplier,Manufacturer,MiscellaneousType,MiscellaneousName")] Miscellaneous miscellaneous)
        {
            if (ModelState.IsValid)
            {
                db.Entry(miscellaneous).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            SetCreateAndEditViewbag(miscellaneous);
            return View(miscellaneous);
        }
        private void SetCreateAndEditViewbag(Miscellaneous misc)
        {
            ViewBag.MiscellaneousType = GenericSelectList(db, typeof(Miscellaneous), "MiscellaneousType", misc.MiscellaneousType);
            ViewBag.Supplier = AssetSelectList(db, "Supplier", misc.Supplier);
            ViewBag.Owner = AssetSelectList(db, "Owner", misc.Owner);
            ViewBag.Manufacturer = GenericSelectList(db, typeof(Miscellaneous), "Manufacturer", misc.Manufacturer);
            ViewBag.ModelName = GenericSelectList(db, typeof(Miscellaneous), "ModelName", misc.ModelName);
        }
        // GET: Miscellaneous/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Miscellaneous miscellaneous = db.Miscellaneous.Find(id);
            if (miscellaneous == null)
            {
                return HttpNotFound();
            }
            return View(miscellaneous);
        }

        // POST: Miscellaneous/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Miscellaneous miscellaneous = db.Miscellaneous.Find(id);
            db.Assets.Remove(miscellaneous);
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
