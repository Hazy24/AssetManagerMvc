using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Net;

namespace AssetManagerMvc.Models
{
    public class Printer : Asset
    {
        [Display(Name = "Printer Name")]
        public string PrinterName { get; set; }
        [Display(Name = "Ip Address")]
        public string IpAddress { get; set; }
        [Display(Name = "Toner Model")]
        public string TonerModel { get; set; }
        [Display(Name = "Drum Model")]
        public string DrumModel { get; set; }
        public string Location { get; set; }
    }
}