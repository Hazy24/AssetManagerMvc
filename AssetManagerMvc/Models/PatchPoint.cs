using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AssetManagerMvc.Models
{
    public class PatchPoint
    {
        public int PatchPointId { get; set; }

        public int Number { get; set; }
        public int Floor { get; set; }        

        [Display(Name = "Room Name")]
        public string RoomName { get; set; }

        [Display(Name = "Room Number")]
        public int RoomNumber { get; set; }

        public string Tile { get; set; }        
        public string Remark { get; set; }

        public string Function { get; set; }       

    }  
}