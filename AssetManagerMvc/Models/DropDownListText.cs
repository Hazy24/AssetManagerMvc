using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AssetManagerMvc.Models
{
    public static class HtmlExtensions
    {
        public static MvcHtmlString DropDownListText(this HtmlHelper helper, string model, 
            string property)
        {
            MvcHtmlString dropDownListText = new MvcHtmlString("");
            return dropDownListText;
        }
    }
}