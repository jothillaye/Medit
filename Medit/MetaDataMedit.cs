using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Medit
{
    [MetadataType(typeof(TravEntMetaData))]
    public partial class TravEnt
    {
        public class TravEntMetaData {
            [Display(Name = "Date d'entrée")]
            [Required]
            public System.DateTime DateEntree { get; set; }
        }
    }
}