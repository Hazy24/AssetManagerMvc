using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AssetManagerMvc.Models
{
    public class PatchPoint
    {
        public int Number { get; set; }
        public int Floor { get; set; }
        public Office Office { get; set; }
        public string Tile { get; set; }
        public Asset Asset { get; set; }
       
    }
    public class Office
    {
        [Display(Name = "Office Name")]
        public string Name { get; set; }
        [Display(Name = "Office Number")]
        public int Number { get; set; }
    }
}