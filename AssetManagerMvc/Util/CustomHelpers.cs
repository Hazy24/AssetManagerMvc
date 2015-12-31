using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Linq.Dynamic;
using System.Data;
using System.Data.Entity;

namespace AssetManagerMvc.Models
{
    public static class CustomHelpers
    {
        public static SelectList GenericSelectList(AssetManagerContext db, Type entityType
            , string property, string selectedvalue)
        {
            var set = db.Set(entityType);
            var query = set.OrderBy(property)
                .Select("new(" + property + ")")
                .Distinct()
                ;
            return new SelectList(query, property, property, selectedvalue);
        }
        public static SelectList AssetSelectList(AssetManagerContext db, string property, string selectedvalue)
        {            
            var query = db.Assets.OrderBy(property)
                .Select("new(" + property + ")")
                .Distinct()
                ;
            return new SelectList(query, property, property, selectedvalue);
        }
        public static string ToStringOrEmpty(this object value)
        {
            return ((object)value ?? String.Empty).ToString();
        }
    }
}