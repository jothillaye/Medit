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

            [Required]
            public string Interlocuteur { get; set; }

            [Display(Name = "Travailleur")]
            [Required]
            public decimal Id_Travailleur { get; set; }
            
            public Nullable<System.DateTime> DateSortie { get; set; }

            [Display(Name = "Profession")]
            [Required]
            public decimal Code_Profession { get; set; }

            [Display(Name = "Entreprise")]
            [Required]
            public decimal Numero_Entreprise { get; set; }
        }
    }

    [MetadataType(typeof(TravailleurMetaData))]
    public partial class Travailleur
    {
        public class TravailleurMetaData
        {


        }
    }
}