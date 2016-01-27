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
    [Authorize(Users = @"OWWOFT\sven, OWWOFT\miquel, OWWOFT\maurice, OWWOFT\kristof, OWWOFT\werner, Hazy-G2358\Hazy, OFT-IT-ENVY\Miquel")]
    public class LogItemsController : Controller
    {
        private AssetManagerContext db = new AssetManagerContext();

        // GET: LogItems
        public ActionResult Index()
        {
            var logItems = db.LogItems.Include(l => l.Asset);
            return View(logItems.ToList());
        }

        // GET: LogItems/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LogItem logItem = db.LogItems.Find(id);
            if (logItem == null)
            {
                return HttpNotFound();
            }
            return View(logItem);
        }

        // GET: LogItems/Create
        public ActionResult Create(int? assetId)
        {
            LogItem logItem = new LogItem();
            logItem.DateCreated = DateTime.Now;
            if (assetId.HasValue) logItem.AssetId = assetId.Value;           
            
            TempData["referringController"] = GetReferringControllerName(Request.UrlReferrer);

            SetCreateAndEditViewbag(logItem);
            return View(logItem);
        }

        // POST: LogItems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "LogItemId,Text,DateCreated,AssetId")] LogItem logItem)
        {
            if (ModelState.IsValid)
            {
                db.LogItems.Add(logItem);
                db.SaveChanges();
                return RedirectToAction("Details", (string)TempData["referringController"], new  { id = logItem.AssetId });
            }

            SetCreateAndEditViewbag(logItem);
            return View(logItem);
        }

        // GET: LogItems/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LogItem logItem = db.LogItems.Find(id);
            if (logItem == null)
            {
                return HttpNotFound();
            }
            TempData["referringController"] = GetReferringControllerName(Request.UrlReferrer);

            SetCreateAndEditViewbag(logItem);
            return View(logItem);
        }

        // POST: LogItems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "LogItemId,Text,DateCreated,AssetId")] LogItem logItem)
        {
            if (ModelState.IsValid)
            {
                db.Entry(logItem).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", (string)TempData["referringController"], new { id = logItem.AssetId });
            }
            SetCreateAndEditViewbag(logItem);
            return View(logItem);
        }
        private void SetCreateAndEditViewbag(LogItem logItem)
        {
            ViewBag.AssetId = GenericSelectList(db, typeof(Asset), "AssetId", logItem.AssetId);
        }
        // GET: LogItems/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LogItem logItem = db.LogItems.Find(id);
            if (logItem == null)
            {
                return HttpNotFound();
            }
            TempData["referringController"] = GetReferringControllerName(Request.UrlReferrer);
            return View(logItem);
        }

        // POST: LogItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            LogItem logItem = db.LogItems.Find(id);
            db.LogItems.Remove(logItem);
            db.SaveChanges();
            return RedirectToAction("Details", (string)TempData["referringController"], new { id = logItem.AssetId });
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
