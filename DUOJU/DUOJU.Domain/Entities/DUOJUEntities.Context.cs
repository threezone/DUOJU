﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DUOJU.Domain.Entities
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class DUOJUEntities : DbContext
    {
        public DUOJUEntities()
            : base("name=DUOJUEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<DUOJU_CITIES> DUOJU_CITIES { get; set; }
        public DbSet<DUOJU_COUNTRIES> DUOJU_COUNTRIES { get; set; }
        public DbSet<DUOJU_PROVINCES> DUOJU_PROVINCES { get; set; }
        public DbSet<DUOJU_ROLE_PRIVILEGES> DUOJU_ROLE_PRIVILEGES { get; set; }
        public DbSet<DUOJU_USERS> DUOJU_USERS { get; set; }
        public DbSet<DUOJU_PARTIES> DUOJU_PARTIES { get; set; }
        public DbSet<DUOJU_PARTY_COMMENTS> DUOJU_PARTY_COMMENTS { get; set; }
        public DbSet<DUOJU_PARTY_PARTICIPANTS> DUOJU_PARTY_PARTICIPANTS { get; set; }
        public DbSet<DUOJU_IDENTIFIERS> DUOJU_IDENTIFIERS { get; set; }
        public DbSet<DUOJU_USER_FINANCES> DUOJU_USER_FINANCES { get; set; }
    }
}
