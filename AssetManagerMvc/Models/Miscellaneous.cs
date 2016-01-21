using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AssetManagerMvc.Models
{
    public class Miscellaneous : Asset
    {
        [Display(Name = "Miscellaneous Name")]
        public string MiscellaneousName { get; set; } = string.Empty;
        [Display(Name = "Type")]
        public string MiscellaneousType { get; set; }        
    }
}