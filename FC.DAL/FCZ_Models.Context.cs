﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace FC.DAL
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class FCEntities : DbContext
    {
        public FCEntities()
            : base("name=FCEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<FC_LeagueMatches> FC_LeagueMatches { get; set; }
        public virtual DbSet<FC_Season> FC_Season { get; set; }
        public virtual DbSet<FC_Team> FC_Team { get; set; }
    }
}
