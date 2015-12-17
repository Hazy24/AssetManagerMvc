using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.DirectoryServices;
using System.Linq;
using System.Web;

namespace AssetManagerMvc.Models
{
    public class UserAccount
    {
        public int UserAccountId { get; set; }
        [Display(Name = "Full Name")]
        public string Name { get; set; }
        [Display(Name = "Given Name")]
        public string GivenName { get; set; }
        public string UserPrincipalName { get; set; }
        [Display(Name = "Surname")]
        public string Sn { get; set; }
        [Display(Name = "Email")]
        public string Mail { get; set; }
        public string Company { get; set; }
        public string DepartmentString { get; set; }

        public int? DepartmentId { get; set; }
        public virtual Department Department { get; set; }

        [DataType(DataType.MultilineText)]
        public string Remark { get; set; }
        public int Headset { get; set; }
        public int Speakers { get; set; }
        public int Keyboard { get; set; }
        public int Mouse { get; set; }
        [Display(Name = "Wireless Mouse")]
        public int WirelessMouse { get; set; }
        [Display(Name = "Usb Stick")]
        public int UsbStick { get; set; }
        [Display(Name = "Laptop Bag")]
        public int LaptopBag { get; set; }
        [Display(Name = "Laptop Stand")]
        public int LaptopStand { get; set; }

        public virtual ICollection<UsePeriod> UsePeriods { get; set; }

        public static void AddDepartments(AssetManagerContext db)
        {
            List<Department> Departments = db.Departments.ToList();
            foreach (UserAccount useraccount in db.UserAccounts)
            {
                useraccount.Department = Departments.Find(d => d.LdapName == useraccount.DepartmentString);
            }
            db.SaveChanges();
        }
        public static void UpdateUserAccounts(out int addedTotal, out int updatedTotal)
        {
            int added, updated;
            addedTotal = 0;
            updatedTotal = 0;
            DirectoryEntry entry = new DirectoryEntry("LDAP://OU=Internal Windows Users,OU=Managed Users,DC=owwoft,DC=int");
            UpdateUserAccounts(entry, out added, out updated);
            addedTotal += added;
            updatedTotal += updated;
            entry = new DirectoryEntry("LDAP://OU=Internal ftcc users, OU=Managed Users,DC=owwoft,DC=int");
            UpdateUserAccounts(entry, out added, out updated);
            addedTotal += added;
            updatedTotal += updated;
            entry = new DirectoryEntry("LDAP://OU=Internal MAC Users, OU=Managed Users,DC=owwoft,DC=int");
            UpdateUserAccounts(entry, out added, out updated);
            addedTotal += added;
            updatedTotal += updated;
        }
        private static void UpdateUserAccounts(DirectoryEntry entry, out int added, out int updated)
        {
            added = 0;
            updated = 0;
            using (AssetManagerContext _context = new AssetManagerContext())
            {
                List<Department> Departments = _context.Departments.ToList();
                DirectorySearcher mySearcher = new DirectorySearcher(entry);
                SearchResultCollection src = mySearcher.FindAll();
                UserAccount ua;
                for (int i = 0; i < src.Count; i++)
                {
                    if (src[i].Properties.Contains("mail"))
                    {
                        string mail = src[i].Properties["mail"][0].ToString();

                        ua = new UserAccount();
                        ua.GivenName = PropertyValueIfExists(src[i], "givenname");
                        ua.Sn = PropertyValueIfExists(src[i], "sn");
                        ua.Name = PropertyValueIfExists(src[i], "name");
                        ua.UserPrincipalName = PropertyValueIfExists(src[i], "userprincipalname");
                        ua.Company = PropertyValueIfExists(src[i], "company");
                        ua.DepartmentString = PropertyValueIfExists(src[i], "department");
                        ua.Department = Departments.Find(d => d.LdapName == ua.DepartmentString);
                        ua.Mail = PropertyValueIfExists(src[i], "mail");
                        if (_context.UserAccounts.Any(o => o.Mail == mail))
                        {
                            // todo update
                            UserAccount existingAccount = _context.UserAccounts.First(o => o.Mail == mail);
                            existingAccount.GivenName = ua.GivenName;
                            existingAccount.Sn = ua.Sn;
                            existingAccount.Name = ua.Name;
                            existingAccount.UserPrincipalName = ua.UserPrincipalName;
                            existingAccount.Company = ua.Company;
                            existingAccount.DepartmentString = ua.DepartmentString;
                            existingAccount.Department = ua.Department;
                            updated++;
                        }
                        else
                        {
                            _context.UserAccounts.Add(ua);
                            added++;
                        }
                    }
                }
                _context.SaveChanges();
            }
        }
        private static string PropertyValueIfExists(SearchResult searchResult, string propertyName)
        {
            string propertyValue = string.Empty;
            if (searchResult.Properties.Contains(propertyName))
                propertyValue = searchResult.Properties[propertyName][0].ToString();
            return propertyValue;
        }
    }
}