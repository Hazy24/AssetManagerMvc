using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AssetManagerMvc.Models
{
    public class Telephone : Asset
    {
        [Display(Name = "Type")]
        public string TelephoneType { get; set; }

        [RegularExpression(@"0[0-9]\s[0-9][0-9][0-9]\s[0-9][0-9]\s[0-9][0-9]|0[0-9][0-9][0-9]\s[0-9][0-9]\s[0-9][0-9]\s[0-9][0-9]", 
            ErrorMessage = "Use e.g. 09 123 45 67 or 0485 12 34 56")]
        public string Number { get; set; }

        [RegularExpression(@"[0-9][0-9][0-9]", ErrorMessage = "Use e.g. 123")]
        [Display(Name = "Intern")]
        public string NumberIntern { get; set; }

        public string Port { get; set; }
    }
}