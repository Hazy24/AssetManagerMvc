using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AssetManagerMvc.Models
{
    public class LogItem
    {
        public int LogItemId { get; set; }
        public string Text { get; set; } = string.Empty;
        public DateTime DateCreated { get; set; }

        public int AssetId { get; set; }
        public virtual Asset Asset { get; set; }
    }
}