using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AssetManagerMvc.Models
{
    public class Miscellaneous : Asset
    {
        [Display(Name = "Type")]
        public string MiscellaneousType { get; set; }        
    }
}