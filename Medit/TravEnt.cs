//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Medit
{
    using System;
    using System.Collections.Generic;
    
    public partial class TravEnt {
        public decimal Id_TravEnt { get; set; }
        public string Interlocuteur { get; set; }
        public System.DateTime DateEntree { get; set; }
        public decimal Id_Travailleur { get; set; }
        public Nullable<System.DateTime> DateSortie { get; set; }
        public decimal Code_Profession { get; set; }
        public decimal Numero_Entreprise { get; set; }
        public decimal Id_Langue { get; set; }
        public string DateEntreeFormated
        {
            get
            {
                return String.Format("{0:dd/MM/yyyy}", DateEntree);
            }
        }
        public string DateSortieFormated
        {
            get
            {
                return String.Format("{0:dd/MM/yyyy}", DateSortie);
            }
        }
        public string InterlocuteurFormated 
        {
            get
            { 
                if(Interlocuteur.CompareTo("0") == 0)
                    return "Oui"; 
                else
                    return "Non";
            }
        }
    
        public virtual Entreprise Entreprise { get; set; }
        public virtual Profession Profession { get; set; }
        public virtual Travailleur Travailleur { get; set; }
        public virtual Travailleur_NonSoumis Travailleur_NonSoumis { get; set; }
        public virtual Travailleur_Soumis Travailleur_Soumis { get; set; }
    }
}
