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
    public class NetworksController : Controller
    {
        private AssetManagerContext db = new AssetManagerContext();

        // GET: Networks
        public ActionResult Index(string sortOrder, string searchString)
        {
            var networks = from n in db.Networks
                           select n;

            if (!String.IsNullOrEmpty(searchString))
            {
                networks = networks.TextSearch(searchString);
            }
            ViewBag.CurrentFilter = searchString;

            ViewBag.CompoundIdSortParm = String.IsNullOrEmpty(sortOrder) ? "compoundId_desc" : "";            
            ViewBag.SerialNumberSortParm = sortOrder == "serialnumber" ? "serialnumber_desc" : "serialnumber";
            ViewBag.ManufacturerSortParm = sortOrder == "manufacturer" ? "manufacturer_desc" : "manufacturer";
            ViewBag.ModelNameSortParm = sortOrder == "modelname" ? "modelname_desc" : "modelname";
            ViewBag.PurchaseDateSortParm = sortOrder == "purchasedate" ? "purchasedate_desc" : "purchasedate";
            ViewBag.NetworkTypeSortParm = sortOrder == "networktype" ? "networktype_desc" : "networktype";
            ViewBag.IpAddressSortParm = sortOrder == "ipaddress" ? "ipaddress_desc" : "ipaddress";

            switch (sortOrder)
            {
                case "compoundId_desc":
                    networks = networks.OrderByDescending(n => n.AssetId);
                    break;               
                case "serialnumber":
                    networks = networks.OrderBy(n => n.SerialNumber);
                    break;
                case "serialnumber_desc":
                    networks = networks.OrderByDescending(n => n.SerialNumber);
                    break;
                case "manufacturer":
                    networks = networks.OrderBy(n => n.Manufacturer);
                    break;
                case "manufacturer_desc":
                    networks = networks.OrderByDescending(n => n.Manufacturer);
                    break;
                case "modelname":
                    networks = networks.OrderBy(n => n.ModelName);
                    break;
                case "modelname_desc":
                    networks = networks.OrderByDescending(n => n.ModelName);
                    break;
                case "purchasedate":
                    networks = networks.OrderBy(n => n.PurchaseDate);
                    break;
                case "purchasedate_desc":
                    networks = networks.OrderByDescending(n => n.PurchaseDate);
                    break;
                case "networktype":
                    networks = networks.OrderBy(n => n.NetworkType);
                    break;
                case "networktype_desc":
                    networks = networks.OrderByDescending(n => n.NetworkType);
                    break;
                case "ipaddress":
                    networks = networks.OrderBy(n => n.IpAddress);
                    break;
                case "ipaddress_desc":
                    networks = networks.OrderByDescending(n => n.IpAddress);
                    break;

                default:  // compoundId ascending 
                    networks = networks.OrderBy(n => n.AssetId);
                    break;
            }
            return View(networks);
        }
        //Print CompoundId to PDF
        public ActionResult Print(string compoundId)
        {
            return File(Util.CompoundIdtoPDFStream(compoundId), "application/pdf", compoundId + ".pdf");
        }
        // GET: Networks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Network network = db.Networks.Find(id);
            if (network == null)
            {
                return HttpNotFound();
            }
            return View(network);
        }

        // GET: Networks/Create
        public ActionResult Create()
        {
            SetCreateAndEditViewbag(new Network());
            return View();
        }

        // POST: Networks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AssetId,SerialNumber,ModelName,PurchaseDate,PurchasePrice,Remark,Owner,Supplier,Manufacturer,IpAddress,NetworkType")] Network network)
        {
            if (ModelState.IsValid)
            {
                db.Networks.Add(network);
                UsePeriod up = new UsePeriod(network, db.UsePeriodStatuses
                   .Where(x => x.Description == "nieuw toestel").First().UsePeriodStatusId);
                db.UsePeriods.Add(up);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            SetCreateAndEditViewbag(network);
            return View(network);
        }

        // GET: Networks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Network network = db.Networks.Find(id);
            if (network == null)
            {
                return HttpNotFound();
            }
            SetCreateAndEditViewbag(network);
            return View(network);
        }

        // POST: Networks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AssetId,SerialNumber,ModelName,PurchaseDate,PurchasePrice,Remark,Owner,Supplier,Manufacturer,IpAddress,NetworkType")] Network network)
        {
            if (ModelState.IsValid)
            {
                db.Entry(network).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            SetCreateAndEditViewbag(network);
            return View(network);
        }
        private void SetCreateAndEditViewbag(Network network)
        {          
            ViewBag.NetworkType = GenericSelectList(db, typeof(Network), "NetworkType", network.NetworkType);
            ViewBag.Supplier = AssetSelectList(db, "Supplier", network.Supplier);
            ViewBag.Owner = AssetSelectList(db, "Owner", network.Owner);
            ViewBag.Manufacturer = GenericSelectList(db, typeof(Network), "Manufacturer", network.Manufacturer);
            ViewBag.ModelName = GenericSelectList(db, typeof(Network), "ModelName", network.ModelName);
        }
        // GET: Networks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Network network = db.Networks.Find(id);
            if (network == null)
            {
                return HttpNotFound();
            }
            return View(network);
        }

        // POST: Networks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Network network = db.Networks.Find(id);
            db.Assets.Remove(network);
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
