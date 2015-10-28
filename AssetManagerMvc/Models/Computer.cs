using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AssetManagerMvc.Models
{
    public class Computer : Asset
    {
        [Display(Name = "Computer Name")]
        public string ComputerName { get; set; }
        [Required]
        [Display(Name = "Computer Type")]
        public string ComputerType { get; set; }
        [Display(Name = "Office Version")]
        public string OfficeVersion { get; set; }
        [Display(Name = "Operating System")]
        public string OperatingSystem { get; set; }        
        public string Browser { get; set; }
        public string AntiVirus { get; set; }
        [Display(Name = "TeamViewer")]
        public bool IsTeamViewerInstalled { get; set; }
    }
}