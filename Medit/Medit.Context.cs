﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class MeditEntities : DbContext
    {
        public MeditEntities()
            : base("name=MeditEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<CodePostal> CodePostals { get; set; }
        public DbSet<Entreprise> Entreprises { get; set; }
        public DbSet<Langue> Langues { get; set; }
        public DbSet<LangueProfession> LangueProfessions { get; set; }
        public DbSet<Profession> Professions { get; set; }
        public DbSet<Travailleur> Travailleurs { get; set; }
        public DbSet<Travailleur_NonSoumis> Travailleur_NonSoumis { get; set; }
        public DbSet<Travailleur_Soumis> Travailleur_Soumis { get; set; }
        public DbSet<TravEnt> TravEnts { get; set; }

        public System.Collections.IEnumerable Travailleur { get; set; }
    }
}
