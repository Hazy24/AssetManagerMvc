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
    public class PatchPointsController : Controller
    {
        private AssetManagerContext db = new AssetManagerContext();

        // GET: PatchPoints
        public ActionResult Index()
        {
            return View(db.PatchPoints.ToList());
        }

        // GET: PatchPoints/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PatchPoint patchPoint = db.PatchPoints.Find(id);
            if (patchPoint == null)
            {
                return HttpNotFound();
            }
            return View(patchPoint);
        }

        // GET: PatchPoints/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PatchPoints/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PatchPointId,Number,Floor,RoomName,RoomNumber,Tile,Remark,Function")] PatchPoint patchPoint)
        {
            if (ModelState.IsValid)
            {
                db.PatchPoints.Add(patchPoint);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(patchPoint);
        }

        // GET: PatchPoints/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PatchPoint patchPoint = db.PatchPoints.Find(id);
            if (patchPoint == null)
            {
                return HttpNotFound();
            }
            return View(patchPoint);
        }

        // POST: PatchPoints/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PatchPointId,Number,Floor,RoomName,RoomNumber,Tile,Remark,Function")] PatchPoint patchPoint)
        {
            if (ModelState.IsValid)
            {
                db.Entry(patchPoint).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(patchPoint);
        }

        // GET: PatchPoints/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PatchPoint patchPoint = db.PatchPoints.Find(id);
            if (patchPoint == null)
            {
                return HttpNotFound();
            }
            return View(patchPoint);
        }

        // POST: PatchPoints/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PatchPoint patchPoint = db.PatchPoints.Find(id);
            db.PatchPoints.Remove(patchPoint);
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
