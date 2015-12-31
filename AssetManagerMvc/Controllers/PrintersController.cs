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

namespace AssetManagerMvc.Controllers
{
    [Authorize(Users = @"OWWOFT\sven, OWWOFT\miquel, OWWOFT\maurice, OWWOFT\kristof, OWWOFT\werner")]
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
                printers = printers.TextSearch(searchString);
            }
            ViewBag.CurrentFilter = searchString;

            ViewBag.CompoundIdSortParm = String.IsNullOrEmpty(sortOrder) ? "compoundId_desc" : "";
            ViewBag.PrinterNameSortParm = sortOrder == "printername" ? "printername_desc" : "printername";
            ViewBag.SerialNumberSortParm = sortOrder == "serialnumber" ? "serialnumber_desc" : "serialnumber";
            ViewBag.ManufacturerSortParm = sortOrder == "manufacturer" ? "manufacturer_desc" : "manufacturer";
            ViewBag.ModelNameSortParm = sortOrder == "modelname" ? "modelname_desc" : "modelname";
            ViewBag.PurchaseDateSortParm = sortOrder == "purchasedate" ? "purchasedate_desc" : "purchasedate";
            ViewBag.LocationSortParm = sortOrder == "location" ? "location_desc" : "location";
            ViewBag.IpAddressSortParm = sortOrder == "ipaddress" ? "ipaddress_desc" : "ipaddress";

            switch (sortOrder)
            {
                case "compoundId_desc":
                    printers = printers.OrderByDescending(p => p.AssetId);
                    break;
                case "printername":
                    printers = printers.OrderBy(p => p.PrinterName);
                    break;
                case "printername_desc":
                    printers = printers.OrderByDescending(p => p.PrinterName);
                    break;
                case "serialnumber":
                    printers = printers.OrderBy(p => p.SerialNumber);
                    break;
                case "serialnumber_desc":
                    printers = printers.OrderByDescending(p => p.SerialNumber);
                    break;
                case "manufacturer":
                    printers = printers.OrderBy(p => p.Manufacturer);
                    break;
                case "manufacturer_desc":
                    printers = printers.OrderByDescending(p => p.Manufacturer);
                    break;
                case "modelname":
                    printers = printers.OrderBy(p => p.ModelName);
                    break;
                case "modelname_desc":
                    printers = printers.OrderByDescending(p => p.ModelName);
                    break;
                case "purchasedate":
                    printers = printers.OrderBy(p => p.PurchaseDate);
                    break;
                case "purchasedate_desc":
                    printers = printers.OrderByDescending(p => p.PurchaseDate);
                    break;
                case "location":
                    printers = printers.OrderBy(p => p.Location);
                    break;
                case "location_desc":
                    printers = printers.OrderByDescending(p => p.Location);
                    break;
                case "ipaddress":
                    printers = printers.OrderBy(p => p.IpAddress);
                    break;
                case "ipaddress_desc":
                    printers = printers.OrderByDescending(p => p.IpAddress);
                    break;

                default:  // compoundId ascending 
                    printers = printers.OrderBy(p => p.AssetId);
                    break;
            }


            return View(printers);
        }

        //Print CompoundId to PDF
        public ActionResult Print(string compoundId)
        {
            return File(Util.CompoundIdtoPDFStream(compoundId), "application/pdf", compoundId + ".pdf");
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
            SetCreateAndEditViewbag(new Printer());
            return View();
        }

        // POST: Printers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AssetId,SerialNumber,ModelName,PurchaseDate,PurchasePrice,Owner,Supplier,Manufacturer,PrinterName,TonerModel,DrumModel,IpAddress,Location")] Printer printer)
        {
            if (ModelState.IsValid)
            {
                db.Assets.Add(printer);
                UsePeriod up = new UsePeriod(printer, db.UsePeriodStatuses
                   .Where(x => x.Description == "nieuw toestel").First().UsePeriodStatusId);
                db.UsePeriods.Add(up);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            SetCreateAndEditViewbag(printer);
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
            SetCreateAndEditViewbag(printer);
            return View(printer);
        }

        // POST: Printers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AssetId,SerialNumber,ModelName,PurchaseDate,PurchasePrice,Owner,Supplier,Manufacturer,PrinterName,TonerModel,DrumModel,IpAddress,Location")] Printer printer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(printer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            SetCreateAndEditViewbag(printer);
            return View(printer);
        }

        private void SetCreateAndEditViewbag(Printer printer)
        {            
            ViewBag.Supplier = AssetSelectList(db, "Supplier", printer.Supplier);
            ViewBag.Owner = AssetSelectList(db, "Owner", printer.Owner);
            ViewBag.Manufacturer = GenericSelectList(db, typeof(Printer), "Manufacturer", printer.Manufacturer);
            ViewBag.ModelName = GenericSelectList(db, typeof(Printer), "ModelName", printer.ModelName);
            ViewBag.TonerModel = GenericSelectList(db, typeof(Printer), "TonerModel", printer.TonerModel);
            ViewBag.DrumModel = GenericSelectList(db, typeof(Printer), "DrumModel", printer.DrumModel);
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
