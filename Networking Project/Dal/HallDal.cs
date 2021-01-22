using Networking_Project.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Networking_Project.Dal
{
    public class HallDal : DbContext
    {
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Hall>().ToTable("Hall");

        }
        public DbSet<Hall> Halls { get; set; }
    }
}