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
    public class BeamersController : Controller
    {
        private AssetManagerContext db = new AssetManagerContext();

        // GET: Beamers
        public ActionResult Index(string sortOrder, string searchString)
        {
            var beamers = from b in db.Beamers
                           select b;
            if (!String.IsNullOrEmpty(searchString))
            {
                beamers = beamers.TextSearch(searchString);
            }
            ViewBag.CurrentFilter = searchString;

            ViewBag.CompoundIdSortParm = String.IsNullOrEmpty(sortOrder) ? "compoundId_desc" : "";
            ViewBag.BeamerNameSortParm = sortOrder == "beamername" ? "beamername_desc" : "beamername";
            ViewBag.SerialNumberSortParm = sortOrder == "serialnumber" ? "serialnumber_desc" : "serialnumber";
            ViewBag.ManufacturerSortParm = sortOrder == "manufacturer" ? "manufacturer_desc" : "manufacturer";
            ViewBag.ModelNameSortParm = sortOrder == "modelname" ? "modelname_desc" : "modelname";
            ViewBag.PurchaseDateSortParm = sortOrder == "purchasedate" ? "purchasedate_desc" : "purchasedate";

            switch (sortOrder)
            {
                case "compoundId_desc":
                    beamers = beamers.OrderByDescending(b => b.AssetId);
                    break;
                case "beamername":
                    beamers = beamers.OrderBy(b => b.BeamerName);
                    break;
                case "beamername_desc":
                    beamers = beamers.OrderByDescending(b => b.BeamerName);
                    break;
                case "serialnumber":
                    beamers = beamers.OrderBy(b => b.SerialNumber);
                    break;
                case "serialnumber_desc":
                    beamers = beamers.OrderByDescending(b => b.SerialNumber);
                    break;
                case "manufacturer":
                    beamers = beamers.OrderBy(b => b.Manufacturer);
                    break;
                case "manufacturer_desc":
                    beamers = beamers.OrderByDescending(b => b.Manufacturer);
                    break;
                case "modelname":
                    beamers = beamers.OrderBy(b => b.ModelName);
                    break;
                case "modelname_desc":
                    beamers = beamers.OrderByDescending(b => b.ModelName);
                    break;
                case "purchasedate":
                    beamers = beamers.OrderBy(b => b.PurchaseDate);
                    break;
                case "purchasedate_desc":
                    beamers = beamers.OrderByDescending(b => b.PurchaseDate);
                    break;               

                default:  // compoundId ascending 
                    beamers = beamers.OrderBy(b => b.AssetId);
                    break;
            }

            return View(beamers);
        }

        //Print CompoundId to PDF
        public ActionResult Print(string compoundId)
        {
            return File(Util.CompoundIdtoPDFStream(compoundId), "application/pdf", compoundId + ".pdf");
        }

        // GET: Beamers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Beamer beamer = db.Beamers.Find(id);
            if (beamer == null)
            {
                return HttpNotFound();
            }
            return View(beamer);
        }

        // GET: Beamers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Beamers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AssetId,SerialNumber,ModelName,PurchaseDate,PurchasePrice,Owner,Supplier,Manufacturer,BeamerName")] Beamer beamer)
        {
            if (ModelState.IsValid)
            {
                db.Assets.Add(beamer);
                UsePeriod up = new UsePeriod(beamer, db.UsePeriodStatuses
                    .Where(x => x.Description == "nieuw toestel").First().UsePeriodStatusId);
                db.UsePeriods.Add(up);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(beamer);
        }

        // GET: Beamers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Beamer beamer = db.Beamers.Find(id);
            if (beamer == null)
            {
                return HttpNotFound();
            }
            return View(beamer);
        }

        // POST: Beamers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AssetId,SerialNumber,ModelName,PurchaseDate,PurchasePrice,Owner,Supplier,Manufacturer,BeamerName")] Beamer beamer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(beamer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(beamer);
        }

        // GET: Beamers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Beamer beamer = db.Beamers.Find(id);
            if (beamer == null)
            {
                return HttpNotFound();
            }
            return View(beamer);
        }

        // POST: Beamers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Beamer beamer = db.Beamers.Find(id);
            db.Assets.Remove(beamer);
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
