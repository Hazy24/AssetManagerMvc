using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AssetManagerMvc.Models
{
    public class Incident
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public Asset Asset { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; }
    }
}