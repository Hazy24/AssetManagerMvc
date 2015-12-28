using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AssetManagerMvc.Models
{
    public class Network : Asset
    {
        [Display(Name = "Type")]
        public string NetworkType { get; set; }        
    }
}