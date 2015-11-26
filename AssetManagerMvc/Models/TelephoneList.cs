﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AssetManagerMvc.Models
{
    public class Telephonelist
    {
        public string Department { get; set; }
        public string Name { get; set; }
        public string NumberIntern { get; set; }
    }
    public class Group<K, T>
    {
        public K Key;
        public IEnumerable<T> Values;
    }
}