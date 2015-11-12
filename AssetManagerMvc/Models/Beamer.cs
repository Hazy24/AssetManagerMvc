using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AssetManagerMvc.Models
{
    public class Beamer : Asset
    {
        [Display(Name = "Beamer Name")]
        public string BeamerName { get; set; }
    }
}