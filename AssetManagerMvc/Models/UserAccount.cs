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
        public string Department { get; set; }

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


        public static int UpdateUserAccounts()
        {
            int added = 0;
            DirectoryEntry entry = new DirectoryEntry("LDAP://OU=Internal Windows Users,OU=Managed Users,DC=owwoft,DC=int");
            added += UpdateUserAccounts(entry);
            entry = new DirectoryEntry("LDAP://OU=Internal ftcc users, OU=Managed Users,DC=owwoft,DC=int");
            added += UpdateUserAccounts(entry);
            entry = new DirectoryEntry("LDAP://OU=Internal MAC Users, OU=Managed Users,DC=owwoft,DC=int");
            added += UpdateUserAccounts(entry);            
            return added;
        }
        private static int UpdateUserAccounts(DirectoryEntry entry)
        {
            using (AssetManagerContext _context = new AssetManagerContext())
            {
                int added = 0;
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
                        ua.Department = PropertyValueIfExists(src[i], "department");
                        ua.Mail = PropertyValueIfExists(src[i], "mail");
                        if (_context.UserAccounts.Any(o => o.Mail == mail))
                        {
                            // todo update
                        }
                        else
                        {
                            _context.UserAccounts.Add(ua);
                            added++;
                        }
                    }
                }
                _context.SaveChanges();
                return added;
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