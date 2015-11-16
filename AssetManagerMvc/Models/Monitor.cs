using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AssetManagerMvc.Models
{
    public class Monitor : Asset
    {
        public float Size { get; set; }       

        [RegularExpression(@"[0-9]+x[0-9]+")]
        [Display(Name = "Resolution")]        
        public string MaxResolution  { get; set; }
    }
}