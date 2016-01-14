using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AssetManagerMvc.Models
{
    public class Asset
    {       
        public int AssetId { get; set; }

        [Required]
        [Display(Name = "Serial Number")]
        public string SerialNumber { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Model Name")]
        public string ModelName { get; set; } = string.Empty;

        [Display(Name = "Purchase Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime? PurchaseDate { get; set; }

        [Display(Name = "Price")]
        public decimal? PurchasePrice { get; set; }

        [DataType(DataType.MultilineText)]
        public string Remark { get; set; } = string.Empty;

        public string Owner { get; set; } = string.Empty;
        public string Supplier { get; set; } = string.Empty;
        public string Manufacturer { get; set; } = string.Empty;

        [Display(Name = "Ip Address")]        
        [DisplayFormat(DataFormatString = "<a href=\"http://{0}\">{0}</a>", HtmlEncode = false)]
        public string IpAddress { get; set; } = string.Empty;

        public string CompoundId { get; set; }
     
        [NotMapped]
        public string CompoundIdAndSerialNumber
        {
            get
            {
                return CompoundId + " - " + SerialNumber;
            }
        }
        public virtual ICollection<UsePeriod> UsePeriods { get; set; }
        public virtual ICollection<LogItem> LogItems { get; set; }
    }
    public class AssetSelectListItem
    {
        public string CompoundId { get; set; }
        public string Identifier { get; set; }
    }
}