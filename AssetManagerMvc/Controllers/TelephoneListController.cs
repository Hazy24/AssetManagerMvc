using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AssetManagerMvc.Models;
using System.IO;

namespace AssetManagerMvc.Controllers
{   
    public class TelephoneListController : Controller
    {
        private AssetManagerContext db = new AssetManagerContext();

        // GET: TelephoneList
        public ActionResult Index()
        {
            return View(Util.TelephoneListByDepartment(db));
        }

        public ActionResult Print()
        {
            return File(Util.TelephoneListPDFStream(db), "application/pdf", "test.pdf");
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
