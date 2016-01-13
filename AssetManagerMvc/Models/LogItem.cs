using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AssetManagerMvc.Models
{
    public class LogItem
    {
        public int LogItemId { get; set; }

        [DataType(DataType.MultilineText)]
        public string Text { get; set; } = string.Empty;

        [Display(Name = "Date Created")]
        // [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime DateCreated { get; set; }

        [Display(Name = "Asset Id")]
        public int AssetId { get; set; }

        public virtual Asset Asset { get; set; }
    }
}