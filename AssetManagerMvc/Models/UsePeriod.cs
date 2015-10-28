using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime? StartDate { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime? EndDate { get; set; }
        public string Remark { get; set; }
        public string Function { get; set; }

        public int? UsePeriodStatusId { get; set; }
        public virtual UsePeriodStatus Status { get; set; }

        public int AssetId { get; set; }
        public virtual Asset Asset { get; set; }
    }
    public class UsePeriodStatus
    {
        public int UsePeriodStatusId { get; set; }
        public string Description { get; set; }
        public int ColorCode { get; set; }
        public UsePeriodStatus cbField { get { return this; } }
    }
}