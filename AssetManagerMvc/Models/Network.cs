using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AssetManagerMvc.Models
{
    public class Network : Asset
    {
        [Display(Name = "Network Name")]
        public string NetworkName { get; set; } = string.Empty;
        [Display(Name = "Type")]
        public string NetworkType { get; set; }         
    }
}