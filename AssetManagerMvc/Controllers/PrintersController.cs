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
    public class PrintersController : Controller
    {
        private AssetManagerContext db = new AssetManagerContext();

        // GET: Printers
        public ActionResult Index(string sortOrder, string searchString)
        {
            var printers = from p in db.Printers
                           select p;
            if (!String.IsNullOrEmpty(searchString))
            {
                printers = printers.Where(p => p.AssetId.ToString().Contains(searchString)
                
                || p.DrumModel.Contains(searchString)
                || p.IpAddress.Contains(searchString)
                || p.Manufacturer.Contains(searchString)
                || p.ModelName.Contains(searchString)
                || p.Owner.Contains(searchString)
                || p.PrinterName.Contains(searchString)
                || p.SerialNumber.Contains(searchString)
                || p.Supplier.Contains(searchString)
                || p.TonerModel.Contains(searchString)           

                    );
            }
            ViewBag.CurrentFilter = searchString;

            ViewBag.CompoundIdSortParm = String.IsNullOrEmpty(sortOrder) ? "compoundId_desc" : "";
            ViewBag.PrinterNameSortParm = sortOrder == "printername" ? "printername_desc" : "printername";
            ViewBag.SerialNumberSortParm = sortOrder == "serialnumber" ? "serialnumber_desc" : "serialnumber";
            ViewBag.ManufacturerSortParm = sortOrder == "manufacturer" ? "manufacturer_desc" : "manufacturer";
            ViewBag.ModelNameSortParm = sortOrder == "modelname" ? "modelname_desc" : "modelname";
            ViewBag.PurchaseDateSortParm = sortOrder == "purchasedate" ? "purchasedate_desc" : "purchasedate";
            ViewBag.IpAddressSortParm = sortOrder == "ipaddress" ? "ipaddress_desc" : "ipaddress";

            switch (sortOrder)
            {
                case "compoundId_desc":
                    printers = printers.OrderByDescending(p => p.AssetId);
                    break;
                case "printerrname":
                    printers = printers.OrderBy(p => p.PrinterName);
                    break;
                case "printername_desc":
                    printers = printers.OrderByDescending(p => p.PrinterName);
                    break;
                case "serialnumber":
                    printers = printers.OrderBy(c => c.SerialNumber);
                    break;
                case "serialnumber_desc":
                    printers = printers.OrderByDescending(c => c.SerialNumber);
                    break;
                case "manufacturer":
                    printers = printers.OrderBy(c => c.Manufacturer);
                    break;
                case "manufacturer_desc":
                    printers = printers.OrderByDescending(c => c.Manufacturer);
                    break;
                case "modelname":
                    printers = printers.OrderBy(c => c.ModelName);
                    break;
                case "modelname_desc":
                    printers = printers.OrderByDescending(c => c.ModelName);
                    break;
                case "purchasedate":
                    printers = printers.OrderBy(c => c.PurchaseDate);
                    break;
                case "purchasedate_desc":
                    printers = printers.OrderByDescending(c => c.PurchaseDate);
                    break;
                case "ipaddress":
                    printers = printers.OrderBy(c => c.IpAddress);
                    break;
                case "ipaddress_desc":
                    printers = printers.OrderByDescending(c => c.IpAddress);
                    break;

                default:  // compoundId ascending 
                    printers = printers.OrderBy(c => c.AssetId);
                    break;
            }


            return View(printers);
        }

        // GET: Printers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Printer printer = db.Printers.Find(id);
            if (printer == null)
            {
                return HttpNotFound();
            }
            return View(printer);
        }

        // GET: Printers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Printers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AssetId,SerialNumber,ModelName,PurchaseDate,PurchasePrice,Owner,Supplier,Manufacturer,PrinterName,TonerModel,DrumModel,IpAddress")] Printer printer)
        {
            if (ModelState.IsValid)
            {
                db.Assets.Add(printer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(printer);
        }

        // GET: Printers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Printer printer = db.Printers.Find(id);
            if (printer == null)
            {
                return HttpNotFound();
            }
            return View(printer);
        }

        // POST: Printers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AssetId,SerialNumber,ModelName,PurchaseDate,PurchasePrice,Owner,Supplier,Manufacturer,PrinterName,TonerModel,DrumModel,IpAddress")] Printer printer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(printer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(printer);
        }

        // GET: Printers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Printer printer = db.Printers.Find(id);
            if (printer == null)
            {
                return HttpNotFound();
            }
            return View(printer);
        }

        // POST: Printers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Printer printer = db.Printers.Find(id);
            db.Assets.Remove(printer);
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
