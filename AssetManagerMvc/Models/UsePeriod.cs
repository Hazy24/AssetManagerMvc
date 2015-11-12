using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AssetManagerMvc.Models
{
    public class UsePeriod
    {
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