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
        public string ComputerName { get; set; } = string.Empty;
        [Required]
        [Display(Name = "Computer Type")]
        public string ComputerType { get; set; } = string.Empty;
        [Display(Name = "Office Version")]
        public string OfficeVersion { get; set; } = string.Empty;
        [Display(Name = "Operating System")]
        public string OperatingSystem { get; set; } = string.Empty;
        public string Browser { get; set; } = string.Empty;
        public string AntiVirus { get; set; } = string.Empty;
        [Display(Name = "TeamViewer")]
        public bool IsTeamViewerInstalled { get; set; } = false;

        [Display(Name = "Image Version")]
        [DisplayFormat(DataFormatString = "<a href=\"/docs/images.xlsx\">{0}</a>", HtmlEncode = false)]
        public string ImageVersion { get; set; } = string.Empty;

        [Display(Name = "Quality Check")]
        [DisplayFormat(DataFormatString = "<a href=\"/docs/quality{0}.docx\">{0}</a>", HtmlEncode = false)]
        public string QualityCheck { get; set; } = string.Empty;

    }
}