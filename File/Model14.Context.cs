﻿//------------------------------------------------------------------------------
// <auto-generated>
//    Этот код был создан из шаблона.
//
//    Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//    Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MaterialWPF.File
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class MaterialEntities : DbContext
    {
        public MaterialEntities()
            : base("name=MaterialEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<Брак> Брак { get; set; }
        public DbSet<Заготовки> Заготовки { get; set; }
        public DbSet<Заказчики> Заказчики { get; set; }
        public DbSet<Заказы> Заказы { get; set; }
        public DbSet<Материалы> Материалы { get; set; }
        public DbSet<МатериалыЗаготовок> МатериалыЗаготовок { get; set; }
        public DbSet<Пользователи> Пользователи { get; set; }
        public DbSet<Производство> Производство { get; set; }
    }
}
