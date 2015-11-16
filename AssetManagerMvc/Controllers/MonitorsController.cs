﻿using System;
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
    public class MonitorsController : Controller
    {
        private AssetManagerContext db = new AssetManagerContext();

        // GET: Monitors
        public ActionResult Index(string sortOrder, string searchString)
        {
            var monitors = from m in db.Monitors
                           select m;
            if (!String.IsNullOrEmpty(searchString))
            {
                monitors = monitors.TextSearch(searchString);
            }
            ViewBag.CurrentFilter = searchString;

            ViewBag.CompoundIdSortParm = String.IsNullOrEmpty(sortOrder) ? "compoundId_desc" : "";
            ViewBag.SerialNumberSortParm = sortOrder == "serialnumber" ? "serialnumber_desc" : "serialnumber";
            ViewBag.ManufacturerSortParm = sortOrder == "manufacturer" ? "manufacturer_desc" : "manufacturer";
            ViewBag.ModelNameSortParm = sortOrder == "modelname" ? "modelname_desc" : "modelname";
            ViewBag.PurchaseDateSortParm = sortOrder == "purchasedate" ? "purchasedate_desc" : "purchasedate";
            ViewBag.SizeSortParm = sortOrder == "size" ? "size_desc" : "size";

            switch (sortOrder)
            {
                case "compoundId_desc":
                    monitors = monitors.OrderByDescending(m => m.AssetId);
                    break;
                case "serialnumber":
                    monitors = monitors.OrderBy(m => m.SerialNumber);
                    break;
                case "serialnumber_desc":
                    monitors = monitors.OrderByDescending(m => m.SerialNumber);
                    break;
                case "manufacturer":
                    monitors = monitors.OrderBy(m => m.Manufacturer);
                    break;
                case "manufacturer_desc":
                    monitors = monitors.OrderByDescending(m => m.Manufacturer);
                    break;
                case "modelname":
                    monitors = monitors.OrderBy(m => m.ModelName);
                    break;
                case "modelname_desc":
                    monitors = monitors.OrderByDescending(m => m.ModelName);
                    break;
                case "purchasedate":
                    monitors = monitors.OrderBy(m => m.PurchaseDate);
                    break;
                case "purchasedate_desc":
                    monitors = monitors.OrderByDescending(m => m.PurchaseDate);
                    break;
                case "size":
                    monitors = monitors.OrderBy(m => m.Size);
                    break;
                case "size_desc":
                    monitors = monitors.OrderByDescending(m => m.Size);
                    break;

                default:  // compoundId ascending 
                    monitors = monitors.OrderBy(m => m.AssetId);
                    break;
            }
            return View(monitors);
        }

        // GET: Monitors/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Monitor monitor = db.Monitors.Find(id);
            if (monitor == null)
            {
                return HttpNotFound();
            }
            return View(monitor);
        }

        // GET: Monitors/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Monitors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AssetId,SerialNumber,ModelName,PurchaseDate,PurchasePrice,Owner,Supplier,Manufacturer,Size,MaxResolution")] Monitor monitor)
        {
            if (ModelState.IsValid)
            {
                db.Assets.Add(monitor);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(monitor);
        }

        // GET: Monitors/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Monitor monitor = db.Monitors.Find(id);
            if (monitor == null)
            {
                return HttpNotFound();
            }
            return View(monitor);
        }

        // POST: Monitors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AssetId,SerialNumber,ModelName,PurchaseDate,PurchasePrice,Owner,Supplier,Manufacturer,Size,MaxResolution")] Monitor monitor)
        {
            if (ModelState.IsValid)
            {
                db.Entry(monitor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(monitor);
        }

        // GET: Monitors/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Monitor monitor = db.Monitors.Find(id);
            if (monitor == null)
            {
                return HttpNotFound();
            }
            return View(monitor);
        }

        // POST: Monitors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Monitor monitor = db.Monitors.Find(id);
            db.Assets.Remove(monitor);
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
