using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AssetManagerMvc.Models
{
    public class Asset
    {
        public int AssetId { get; set; }
        [Display(Name = "Serial Number")]
        public string SerialNumber { get; set; }
        [Display(Name = "Model Name")]
        public string ModelName { get; set; }
        [Display(Name = "Purchase Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime? PurchaseDate { get; set; }
        [Display(Name = "Price")]
        public decimal? PurchasePrice { get; set; }
        public string Owner { get; set; }
        // public List<UsePeriod> UserPeriods { get; set; }
        public string Supplier { get; set; }
        public string Manufacturer { get; set; }

        public string CompoundId
        {
            get
            {
                string compoundid = AssetId.ToString();
                if (this is Computer) compoundid = "C" + compoundid;
                return compoundid;
            }
        }

        private readonly ObservableListSource<UsePeriod> _usePeriods =
                new ObservableListSource<UsePeriod>();

        public virtual ObservableListSource<UsePeriod> UsePeriods { get { return _usePeriods; } }
    }
}