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
        [Display(Name = "Size (inch)")]
        [DisplayFormat(DataFormatString = "{0:G}\"")]
        public float Size { get; set; }       

        [RegularExpression(@"[0-9]+x[0-9]+", ErrorMessage = "Use e.g. 1024x768 or 1920x1080")]
        [Display(Name = "Resolution")]        
        public string MaxResolution  { get; set; }
    }
}