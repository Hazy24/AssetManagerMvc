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
    public class TelephonesController : Controller
    {
        private AssetManagerContext db = new AssetManagerContext();

        // GET: Telephones
        public ActionResult Index()
        {
            var telephones = from t in db.Telephones
                             select t;

            return View(telephones);
        }

        // GET: Telephones/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Telephone telephone = db.Telephones.Find(id);
            if (telephone == null)
            {
                return HttpNotFound();
            }
            return View(telephone);
        }

        // GET: Telephones/Create
        public ActionResult Create()
        {
            SetCreateAndEditViewbag();
            return View();
        }

        // POST: Telephones/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AssetId,SerialNumber,ModelName,PurchaseDate,PurchasePrice,Owner,Supplier,Manufacturer,TelephoneType,Number,NumberIntern")] Telephone telephone)
        {
            if (ModelState.IsValid)
            {
                db.Assets.Add(telephone);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            SetCreateAndEditViewbag(telephone.TelephoneType);
            return View(telephone);
        }

        // GET: Telephones/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Telephone telephone = db.Telephones.Find(id);
            if (telephone == null)
            {
                return HttpNotFound();
            }
            SetCreateAndEditViewbag(telephone.TelephoneType);
            return View(telephone);
        }

        // POST: Telephones/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AssetId,SerialNumber,ModelName,PurchaseDate,PurchasePrice,Owner,Supplier,Manufacturer,TelephoneType,Number,NumberIntern")] Telephone telephone)
        {
            if (ModelState.IsValid)
            {
                db.Entry(telephone).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            SetCreateAndEditViewbag(telephone.TelephoneType);
            return View(telephone);
        }
        private void SetCreateAndEditViewbag(string telephoneType = null)
        {
            if (string.IsNullOrEmpty(telephoneType))
            {
                ViewBag.TelephoneType = new SelectList(db.Telephones, "TelephoneType", "TelephoneType")
                .GroupBy(f => f.Text).Select(f => f.First()) // == Distinct              
                .OrderBy(f => f.Text);
            }
            else
            {
                ViewBag.TelephoneType = new SelectList(db.Telephones, "TelephoneType", "TelephoneType", telephoneType)
                .GroupBy(f => f.Text).Select(f => f.First()) // == Distinct              
                .OrderBy(f => f.Text);
            }
        }

        // GET: Telephones/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Telephone telephone = db.Telephones.Find(id);
            if (telephone == null)
            {
                return HttpNotFound();
            }
            return View(telephone);
        }

        // POST: Telephones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Telephone telephone = db.Telephones.Find(id);
            db.Assets.Remove(telephone);
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
