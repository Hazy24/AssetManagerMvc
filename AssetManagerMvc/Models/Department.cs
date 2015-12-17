using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AssetManagerMvc.Models
{
    public class Department
    {
        public int DepartmentId { get; set; }

        [Display(Name = "Department Name")]
        public string Name { get; set; }

        [Display(Name = "Department LdapName")]
        public string LdapName { get; set; }

        public virtual ICollection<UserAccount> UserAccounts { get; set; }       
    }
}