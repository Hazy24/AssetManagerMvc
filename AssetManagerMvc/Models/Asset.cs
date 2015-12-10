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
        public string SerialNumber { get; set; }

        [Required]
        [Display(Name = "Model Name")]
        public string ModelName { get; set; }

        [Display(Name = "Purchase Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime? PurchaseDate { get; set; }

        [Display(Name = "Price")]
        public decimal? PurchasePrice { get; set; }

        public string Owner { get; set; }
        public string Supplier { get; set; }
        public string Manufacturer { get; set; }

        [NotMapped]
        public string CompoundId
        {
            get
            {
                string compoundid = AssetId.ToString();
                if (this is Computer) compoundid = "C" + compoundid;
                else if (this is Printer) compoundid = "P" + compoundid;
                else if (this is Beamer) compoundid = "B" + compoundid;
                else if (this is Monitor) compoundid = "M" + compoundid;
                else if (this is Telephone) compoundid = "T" + compoundid;
                return compoundid;
            }
        }
        [NotMapped]
        public string CompoundIdAndSerialNumber
        {
            get
            {
                return CompoundId + " - " + SerialNumber;
            }
        }
        public virtual ICollection<UsePeriod> UsePeriods { get; set; }
    }
    public class AssetSelectListItem
    {
        public string CompoundId { get; set; }
        public string Identifier { get; set; }
    }
}