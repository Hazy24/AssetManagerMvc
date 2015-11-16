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



            return View(beamers);
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
