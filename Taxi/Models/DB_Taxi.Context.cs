﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Taxi.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class DB_taxiEntities : DbContext
    {
        public DB_taxiEntities()
            : base("name=DB_taxiEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<tx_trace> tx_trace { get; set; }
        public virtual DbSet<tx_orders> tx_orders { get; set; }
        public virtual DbSet<tx_settings> tx_settings { get; set; }
    }
}
