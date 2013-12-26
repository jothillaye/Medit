using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Medit
{
    [MetadataType(typeof(TravEntMetaData))]
    public partial class TravEnt
    {
        public class TravEntMetaData {
            [Required]
            public string Interlocuteur { get; set; }
            public string InterlocuteurFormated { get { if (Interlocuteur.CompareTo("1") == 0) return "Oui"; else return "Non"; } }

            [Display(Name = "Travailleur")]
            [Required, Range(1, Int32.MaxValue, ErrorMessage = "Le champ travailleur est requis.")]
            public decimal Id_Travailleur { get; set; }

            [Display(Name = "Entreprise")]
            [Required, Range(1, Int32.MaxValue, ErrorMessage = "Le champ entreprise est requis.")]
            public decimal Numero_Entreprise { get; set; }

            [Display(Name = "Date d'entrée")]
            [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
            [Required]
            public System.DateTime DateEntree { get; set; }

            [Display(Name = "Date de Sortie")]
            [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
            public Nullable<System.DateTime> DateSortie { get; set; }

            [Display(Name = "Profession")]
            [Required]
            public decimal Code_Profession { get; set; }

            [Display(Name = "Langue")]
            public decimal Id_Langue { get; set; }

            [Display(Name = "Soumis à des risques ?")]
            [Required(ErrorMessage = "Le champ ci-dessus est requis.")]
            public string isSoumis { get; set; }
        }
    }

    [MetadataType(typeof(TravailleurMetaData))]
    public partial class Travailleur
    {
        public class TravailleurMetaData
        {            
            [Required]
            public string Nom { get; set; }

            [Required]
            [Display(Name = "Prénom")]
            public string Prenom { get; set; }

            [Required]
            [Display(Name = "Numéro de téléphone")]
            public string NumTel { get; set; }

            [Required]
            [Display(Name = "Numéro")]
            public string AdresseNum { get; set; }

            [Required]
            [Display(Name = "Rue")]
            public string AdresseRue { get; set; }

            [Display(Name = "Dossier médical")]
            [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N0}")]
            public Nullable<decimal> NumDossierMedical { get; set; }

            [Required]
            [Display(Name = "Code postal")]
            public decimal Id_CodePostal { get; set; }

            [Display(Name="Prénom & Nom")]
            public string NomPre { get { return Prenom + " " + Nom; } }           
        }
    }

    [MetadataType(typeof(EntrepriseMetaData))]
    public partial class Entreprise
    {
        public class EntrepriseMetaData
        {
            [Display(Name = "Numéro")]
            public string AdresseNum { get; set; }

            [Display(Name = "Rue")]
            public string AdresseRue { get; set; }
                        
            [Display(Name = "Nombre d'Employés Soumis*")]
            [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N0}")]
            public decimal NbEmployeDebAnneeSoumis { get; set; }

            [Display(Name = "Nombre d'Employés Non-Soumis*")]
            [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N0}")]
            public decimal NbEmployeDebAnneeNon_Soumis { get; set; }

            [Display(Name = "Code du Secteur")]
            [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N0}")]
            public decimal Code_Secteur { get; set; }
        }
    }
}