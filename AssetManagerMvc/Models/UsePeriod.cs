using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AssetManagerMvc.Models
{
    public class UsePeriod : IComparable<UsePeriod>
    {
        public UsePeriod() { }
        public UsePeriod(Asset asset, int statusId)
        {
            Asset = asset;
            StartDate = asset.PurchaseDate != null ? asset.PurchaseDate : DateTime.Now;
            UsePeriodStatusId = statusId;
        }
        public UsePeriod Copy()
        {
            UsePeriod copy = new UsePeriod();
            copy.AssetId = this.AssetId;
            copy.UserAccountId = this.UserAccountId;
            copy.UsePeriodStatusId = this.UsePeriodStatusId;
            copy.StartDate = this.StartDate;
            copy.EndDate = this.EndDate;
            copy.Remark = this.Remark;
            copy.Function = this.Function;
            copy.UserIsAdmin = this.UserIsAdmin;

            return copy;
        }
        /* 
            Voor sorteren kijk eerst naar de einddatum, dan naar de startdatum, en
            dan eventueel naar id. datums worden descending gesorteerd (nieuwste bovenaan).
            Niet ingevulde datums worden als in de oneindige toekomst beschouwd.
            Uren en minuten worden genegeerd omdat anders automatisch gegenereerde
            en handmatig ingevulde datums onverwacht vergelijkingsresultaten kunnen geven (bij
            handmatige is het 0:00 uur, bij automatische is het datetime.now).
            Als beide datums identiek zijn worden de hoogste UsperiodId's (= later
            aangemaakt) bovenaan gezet.
        */

        public int CompareTo(UsePeriod otherUseperiod)
        {
            int result = CompareNullableDateTimeDescending(this.EndDate, otherUseperiod.EndDate);
            if (result != 0) return result; //end date differs
            else
            {
                result = CompareNullableDateTimeDescending(this.StartDate, otherUseperiod.StartDate);
                if (result != 0) return result; //start date differs
                else return otherUseperiod.UsePeriodId.CompareTo(this.UsePeriodId); // latest id on top
            }
        }

        
        private int CompareNullableDateTimeDescending(DateTime? x, DateTime? y)
        {
            if (!x.HasValue && !y.HasValue) { return 0; }
            // null dates are treated as in the future, instead of in the past as is default in .Net
            else if (x.HasValue && !y.HasValue) { return 1; }
            else if (!x.HasValue && y.HasValue) { return -1; }

            else { return y.Value.Date.CompareTo(x.Value.Date); }
        }


        public int UsePeriodId { get; set; }
        public int? UserAccountId { get; set; }
        public virtual UserAccount UserAccount { get; set; }
        // TODO check overlap
        // bool overlap = a.start < b.end && b.start < a.end;       
        [Display(Name = "Start date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime? StartDate { get; set; }

        [Display(Name = "End date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime? EndDate { get; set; }

        [DataType(DataType.MultilineText)]
        public string Remark { get; set; }

        public string Function { get; set; }

        [Display(Name = "User is Admin")]
        public bool UserIsAdmin { get; set; }

        public int? UsePeriodStatusId { get; set; }
        public virtual UsePeriodStatus Status { get; set; }

        public int AssetId { get; set; }
        public virtual Asset Asset { get; set; }

        //[NotMapped]
        //public class Filter
        //{           
        //    public string SortOrder { get; set; }
        //    public string SearchString { get; set; }
        //    public bool? Current { get; set; }
        //    public bool? hideUitGebruik { get; set; }
        //    public string Category { get; set; }
        //}
    }
    public class UsePeriodStatus
    {
        public int UsePeriodStatusId { get; set; }
        [Display(Name = "Status")]
        public string Description { get; set; }
        public int ColorCode { get; set; }
        public UsePeriodStatus cbField { get { return this; } }
    }
}