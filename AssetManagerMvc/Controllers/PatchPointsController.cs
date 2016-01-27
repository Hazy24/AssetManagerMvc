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
    [Authorize(Users = @"OWWOFT\sven, OWWOFT\miquel, OWWOFT\maurice, OWWOFT\kristof, OWWOFT\werner, Hazy-G2358\Hazy, OFT-IT-ENVY\Miquel")]
    public class PatchPointsController : Controller
    {
        private AssetManagerContext db = new AssetManagerContext();

        // GET: PatchPoints
        public ActionResult Index(string sortOrder, string searchString)
        {
            var patchpoints = from pp in db.PatchPoints
                              select pp;

            if (!String.IsNullOrWhiteSpace(searchString))
            {
                patchpoints = patchpoints.TextSearch(searchString);
            }

            ViewBag.CurrentFilter = searchString;

            ViewBag.NumberSortParm = String.IsNullOrEmpty(sortOrder) ? "number_desc" : "";
            ViewBag.FloorSortParm = sortOrder == "floor" ? "floor_desc" : "floor";
            ViewBag.RoomNameSortParm = sortOrder == "roomname" ? "roomname_desc" : "roomname";
            ViewBag.RoomNumberSortParm = sortOrder == "roomnumber" ? "roomnumber_desc" : "roomnumber";
            ViewBag.TileSortParm = sortOrder == "tile" ? "tile_desc" : "tile";
            ViewBag.FunctionSortParm = sortOrder == "function" ? "function_desc" : "function";

            switch (sortOrder)
            {
                case "number_desc":
                    patchpoints = patchpoints.OrderByDescending(pp => pp.Number);
                    break;
                case "floor":
                    patchpoints = patchpoints.OrderBy(pp => pp.Floor);
                    break;
                case "floor_desc":
                    patchpoints = patchpoints.OrderByDescending(pp => pp.Floor);
                    break;
                case "roomname":
                    patchpoints = patchpoints.OrderBy(pp => pp.RoomName);
                    break;
                case "roomname_desc":
                    patchpoints = patchpoints.OrderByDescending(pp => pp.RoomName);
                    break;
                case "roomnumber":
                    patchpoints = patchpoints.OrderBy(pp => pp.RoomNumber);
                    break;
                case "roomnumber_desc":
                    patchpoints = patchpoints.OrderByDescending(pp => pp.RoomNumber);
                    break;
                case "tile":
                    patchpoints = patchpoints.OrderBy(pp => pp.Tile);
                    break;
                case "tile_desc":
                    patchpoints = patchpoints.OrderByDescending(pp => pp.Tile);
                    break;
                case "function":
                    patchpoints = patchpoints.OrderBy(pp => pp.Function);
                    break;
                case "function_desc":
                    patchpoints = patchpoints.OrderByDescending(pp => pp.Function);
                    break;
                default:  // number ascending 
                    patchpoints = patchpoints.OrderBy(pp => pp.Number);
                    break;
            }
            return View(patchpoints);
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
