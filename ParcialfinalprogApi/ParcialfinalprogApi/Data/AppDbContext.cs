using Microsoft.EntityFrameworkCore;
using ParcialfinalprogApi.Models;
using System.Collections.Generic;

namespace ParcialfinalprogApi.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Moneda> Monedas { get; set; }
        public DbSet<Transaccion> Transacciones { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=cryptoBDD.db");
        }
    }
}