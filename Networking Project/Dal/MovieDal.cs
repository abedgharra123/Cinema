using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Networking_Project.Models;
using System.Data.Entity;

namespace Networking_Project.Dal
{
    public class MovieDal : DbContext
    {
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Movie>().ToTable("tblMovie");

        }
        public DbSet<Movie> Movies { get; set; }
    }
}