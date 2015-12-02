﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AssetManagerMvc.Models
{
    public class RepairInfo
    {
        public RepairInfo()
        {
            Date = DateTime.Now;

            // set spaces for strings or ReplaceText will complain if they're empty
            Owner = " ";
            CompoundId = " ";
            Reason = " ";
            Remark = " ";
        }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        public string Owner { get; set; }

        public string CompoundId { get; set; }

        [DataType(DataType.MultilineText)]
        public string Reason { get; set; }

        [DataType(DataType.MultilineText)]
        public string Remark { get; set; }
    }
}