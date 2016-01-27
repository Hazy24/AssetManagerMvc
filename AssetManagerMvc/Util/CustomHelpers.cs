using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Linq.Dynamic;
using System.Data;
using System.Data.Entity;
using System.Text.RegularExpressions;

namespace AssetManagerMvc.Models
{
    public static class CustomHelpers
    {
        public static SelectList GenericSelectList(AssetManagerContext db, Type entityType
            , string property, object selectedvalue)
        {             
            var set = db.Set(entityType);
            var query = set.OrderBy(property)
                .Select("new(" + property + ")")                
                .Distinct()
                ;

            SelectList selectlist = new SelectList(query, property, property, selectedvalue);
            string selected = selectedvalue.ToStringOrEmpty();

            if (!string.IsNullOrWhiteSpace(selected) && !selectlist.Contains(selectedvalue))
            {                
                List<SelectListItem> list = selectlist.ToList();
                list.Add(new SelectListItem { Text = selected, Value = selected });
                list.Sort((x, y) => x.Text.CompareTo(y.Text));
                selectlist = new SelectList(list, "Value", "Text", selectedvalue);
            }
            return selectlist;
        }      
        public static string ToStringOrEmpty(this object value)
        {
            return ((object)value ?? String.Empty).ToString();
        }
        public static string GetReferringControllerName(Uri urlReferrer)
        {
            string controller = string.Empty;

            // "/AssetManager/" is in the path when running on server
            Match match = Regex.Match(urlReferrer.AbsolutePath, @"/AssetManager/([A-Za-z]+)");
            if (match.Success && match.Groups.Count > 1) { controller = match.Groups[1].Value; }
            else
            {
                // only"/" is in the path when running locally
                match = Regex.Match(urlReferrer.AbsolutePath, @"/([A-Za-z]+)");
                if (match.Success && match.Groups.Count > 1) { controller = match.Groups[1].Value; }
            }

            return controller;
        }
    }
}