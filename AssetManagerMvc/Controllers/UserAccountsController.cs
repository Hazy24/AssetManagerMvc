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
    [Authorize(Users = @"OWWOFT\sven, OWWOFT\miquel, OWWOFT\maurice, OWWOFT\kristof, OWWOFT\werner")]
    public class UserAccountsController : Controller
    {
        private AssetManagerContext db = new AssetManagerContext();

        // GET: UserAccounts
        public ActionResult Index(string sortOrder, string searchString)
        {
            var useraccounts = from ua in db.UserAccounts
                               select ua;
            if (!String.IsNullOrEmpty(searchString))
            {
                useraccounts = useraccounts.Where(ua => ua.Name.Contains(searchString)
                || ua.Company.Contains(searchString)
                || ua.Department.Contains(searchString)
                || ua.Mail.Contains(searchString)
                || ua.UserPrincipalName.Contains(searchString)

                    );
            }
            ViewBag.CurrentFilter = searchString;

            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.GivenNameSortParm = sortOrder == "givenname" ? "givenname_desc" : "givenname";
            ViewBag.UserPrincipalNameSortParm = sortOrder == "userprincipalname" ? "userprincipalname_desc" : "userprincipalname";
            ViewBag.SnSortParm = sortOrder == "surname" ? "surname_desc" : "surname";
            ViewBag.MailSortParm = sortOrder == "email" ? "email_desc" : "email";
            ViewBag.CompanySortParm = sortOrder == "company" ? "company_desc" : "company";
            ViewBag.DepartmentSortParm = sortOrder == "department" ? "department_desc" : "department";

            switch (sortOrder)
            {
                case "name_desc":
                    useraccounts = useraccounts.OrderByDescending(ua => ua.Name);
                    break;
                case "givenname":
                    useraccounts = useraccounts.OrderBy(ua => ua.GivenName);
                    break;
                case "givenname_desc":
                    useraccounts = useraccounts.OrderByDescending(ua => ua.GivenName);
                    break;
                case "userprincipalname":
                    useraccounts = useraccounts.OrderBy(ua => ua.UserPrincipalName);
                    break;
                case "userprincipalname_desc":
                    useraccounts = useraccounts.OrderByDescending(ua => ua.UserPrincipalName);
                    break;
                case "surname":
                    useraccounts = useraccounts.OrderBy(ua => ua.Sn);
                    break;
                case "surname_desc":
                    useraccounts = useraccounts.OrderByDescending(ua => ua.Sn);
                    break;
                case "email":
                    useraccounts = useraccounts.OrderBy(ua => ua.Mail);
                    break;
                case "email_desc":
                    useraccounts = useraccounts.OrderByDescending(ua => ua.Mail);
                    break;
                case "company":
                    useraccounts = useraccounts.OrderBy(ua => ua.Company);
                    break;
                case "company_desc":
                    useraccounts = useraccounts.OrderByDescending(ua => ua.Company);
                    break;
                case "department":
                    useraccounts = useraccounts.OrderBy(ua => ua.Department);
                    break;
                case "department_desc":
                    useraccounts = useraccounts.OrderByDescending(ua => ua.Department);
                    break;

                default:  // name ascending 
                    useraccounts = useraccounts.OrderBy(ua => ua.Name);
                    break;
            }

            return View(useraccounts);
        }
        [HttpPost]
        public ActionResult Index(string update)
        {
            try
            {
                int added = UserAccount.UpdateUserAccounts();
                if (added != 1)
                { ViewBag.UpdateResult = "Success! Added " + added + " users."; }
                else
                { ViewBag.UpdateResult = "Success! Added 1 user."; }
            }
            catch (Exception ex)
            {

                ViewBag.UpdateResult = ex.InnerException.Message;
            }

            return View(db.UserAccounts.ToList());
        }
        // GET: UserAccounts/Details/5
        public ActionResult Details(int? id)
        {                 
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserAccount userAccount = db.UserAccounts.Find(id);
            if (userAccount == null)
            {
                return HttpNotFound();
            }
            return View(userAccount);
        }

        // GET: UserAccounts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserAccounts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserAccountId,Name,GivenName,UserPrincipalName,Sn,Mail,Company,Department,IsAdmin")] UserAccount userAccount)
        {
            if (ModelState.IsValid)
            {
                db.UserAccounts.Add(userAccount);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(userAccount);
        }

        // GET: UserAccounts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserAccount userAccount = db.UserAccounts.Find(id);
            if (userAccount == null)
            {
                return HttpNotFound();
            }
            return View(userAccount);
        }

        // POST: UserAccounts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserAccountId,Name,GivenName,UserPrincipalName,Sn,Mail,Company,Department,Remark,Headset,Speakers,Keyboard,Mouse,WirelessMouse,UsbStick,LaptopBag")] UserAccount userAccount)
        {
            if (ModelState.IsValid)
            {
                db.Entry(userAccount).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(userAccount);
        }

        // GET: UserAccounts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserAccount userAccount = db.UserAccounts.Find(id);
            if (userAccount == null)
            {
                return HttpNotFound();
            }
            return View(userAccount);
        }

        // POST: UserAccounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            UserAccount userAccount = db.UserAccounts.Find(id);
            db.UserAccounts.Remove(userAccount);
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
