using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AssetManagerMvc.Models;
using System.Linq.Expressions;
using static AssetManagerMvc.Models.CustomHelpers;

namespace AssetManagerMvc.Controllers
{
    [Authorize(Users = @"OWWOFT\sven, OWWOFT\miquel, OWWOFT\maurice, OWWOFT\kristof, OWWOFT\werner")]
    public class ComputersController : Controller
    {
        private AssetManagerContext db = new AssetManagerContext();

        // GET: Computers
        public ActionResult Index(string sortOrder, string searchString)
        {
            // Util.InsertCompounIdsInDb(db);

            var computers = from c in db.Computers
                            select c;

            if (!String.IsNullOrWhiteSpace(searchString))
            {
                computers = computers.TextSearch(searchString);
            }

            ViewBag.CurrentFilter = searchString;

            ViewBag.CompoundIdSortParm = String.IsNullOrEmpty(sortOrder) ? "compoundId_desc" : "";
            ViewBag.ComputerNameSortParm = sortOrder == "computername" ? "computername_desc" : "computername";
            ViewBag.SerialNumberSortParm = sortOrder == "serialnumber" ? "serialnumber_desc" : "serialnumber";
            ViewBag.ManufacturerSortParm = sortOrder == "manufacturer" ? "manufacturer_desc" : "manufacturer";
            ViewBag.ModelNameSortParm = sortOrder == "modelname" ? "modelname_desc" : "modelname";
            ViewBag.PurchaseDateSortParm = sortOrder == "purchasedate" ? "purchasedate_desc" : "purchasedate";
            ViewBag.ComputerTypeSortParm = sortOrder == "computertype" ? "computertype_desc" : "computertype";

            switch (sortOrder)
            {
                case "compoundId_desc":
                    computers = computers.OrderByDescending(c => c.AssetId);
                    break;
                case "computername":
                    computers = computers.OrderBy(c => c.ComputerName);
                    break;
                case "computername_desc":
                    computers = computers.OrderByDescending(c => c.ComputerName);
                    break;
                case "serialnumber":
                    computers = computers.OrderBy(c => c.SerialNumber);
                    break;
                case "serialnumber_desc":
                    computers = computers.OrderByDescending(c => c.SerialNumber);
                    break;
                case "manufacturer":
                    computers = computers.OrderBy(c => c.Manufacturer);
                    break;
                case "manufacturer_desc":
                    computers = computers.OrderByDescending(c => c.Manufacturer);
                    break;
                case "modelname":
                    computers = computers.OrderBy(c => c.ModelName);
                    break;
                case "modelname_desc":
                    computers = computers.OrderByDescending(c => c.ModelName);
                    break;
                case "purchasedate":
                    computers = computers.OrderBy(c => c.PurchaseDate);
                    break;
                case "purchasedate_desc":
                    computers = computers.OrderByDescending(c => c.PurchaseDate);
                    break;
                case "computertype":
                    computers = computers.OrderBy(c => c.ComputerType);
                    break;
                case "computertype_desc":
                    computers = computers.OrderByDescending(c => c.ComputerType);
                    break;

                default:  // compoundId ascending 
                    computers = computers.OrderBy(c => c.AssetId);
                    break;
            }

            return View(computers);
        }
        //Print CompoundId to PDF
        public ActionResult Print(string compoundId)
        {
            return File(Util.CompoundIdtoPDFStream(compoundId), "application/pdf", compoundId + ".pdf");
        }
        // GET: Computers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Computer computer = db.Computers.Find(id);            
            if (computer == null)
            {
                return HttpNotFound();
            }
            return View(computer);
        }

        // GET: Computers/Create
        public ActionResult Create()
        {
            SetCreateAndEditViewbag(new Computer());
            return View();
        }

        // POST: Computers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AssetId,SerialNumber,ModelName,PurchaseDate,PurchasePrice,Owner,Supplier,Manufacturer,ComputerName,ComputerType,OfficeVersion,OperatingSystem,Remark,ImageVersion,QualityCheck")] Computer computer)
        {
            if (ModelState.IsValid)
            {
                
                db.Computers.Add(computer);
                UsePeriod up = new UsePeriod(computer, db.UsePeriodStatuses
                    .Where(x => x.Description == "nieuw toestel").First().UsePeriodStatusId);
                db.UsePeriods.Add(up);
                db.SaveChanges();
                computer.CompoundId = "C" + computer.AssetId;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            SetCreateAndEditViewbag(computer);

            return View(computer);
        }

        // GET: Computers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Computer computer = db.Computers.Find(id);
            if (computer == null)
            {
                return HttpNotFound();
            }
            SetCreateAndEditViewbag(computer);
            return View(computer);
        }

        // POST: Computers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AssetId,SerialNumber,ModelName,PurchaseDate,PurchasePrice,Owner,Supplier,Manufacturer,ComputerName,ComputerType,OfficeVersion,OperatingSystem,Remark,ImageVersion,QualityCheck")] Computer computer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(computer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            SetCreateAndEditViewbag(computer);
            return View(computer);
        }

        private void SetCreateAndEditViewbag(Computer computer)
        {           
            ViewBag.ComputerType = GenericSelectList(db, typeof(Computer), "ComputerType", computer.ComputerType);
            ViewBag.OfficeVersion = GenericSelectList(db, typeof(Computer), "OfficeVersion", computer.OfficeVersion);
            ViewBag.OperatingSystem = GenericSelectList(db, typeof(Computer), "OperatingSystem", computer.OperatingSystem);
            ViewBag.Supplier = AssetSelectList(db, "Supplier", computer.Supplier);
            ViewBag.Owner = AssetSelectList(db, "Owner", computer.Owner);
            ViewBag.Manufacturer = GenericSelectList(db, typeof(Computer), "Manufacturer", computer.Manufacturer);
            ViewBag.ModelName = GenericSelectList(db, typeof(Computer), "ModelName", computer.ModelName);
            ViewBag.ImageVersion = GenericSelectList(db, typeof(Computer), "ImageVersion", computer.ImageVersion);
            ViewBag.QualityCheck = GenericSelectList(db, typeof(Computer), "QualityCheck", computer.QualityCheck);
        }
        // GET: Computers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Computer computer = db.Computers.Find(id);
            if (computer == null)
            {
                return HttpNotFound();
            }
            return View(computer);
        }

        // POST: Computers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Computer computer = db.Computers.Find(id);
            db.Assets.Remove(computer);
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
        public ActionResult InitializeUserControl(Type TModel, string value)
        {
            Expression<Func<string, string>> exp;
            //    string a = testing;
            return View();
        }
    }

}
