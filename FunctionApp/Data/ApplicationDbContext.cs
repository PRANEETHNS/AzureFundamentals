using FunctionApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionApp.Data
{
    public class ApplicationDbContext: DbContext
    {
        //This must be created to connect with entity framework
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<SalesRequest> SalesRequests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Defining the primary key
            modelBuilder.Entity<SalesRequest>(entity => 
            {
                entity.HasKey(e => e.Id);
            });
        }

    }
}
