using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
    }
}