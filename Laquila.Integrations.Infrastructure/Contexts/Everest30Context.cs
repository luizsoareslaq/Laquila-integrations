using Laquila.Integrations.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Laquila.Integrations.Infrastructure.Contexts
{
    using Laquila.Integrations.Domain.Models.Everest30;
    using Microsoft.EntityFrameworkCore;

    public class Everest30Context : DbContext
    {
        public Everest30Context(DbContextOptions<Everest30Context> options)
            : base(options)
        {
        }

        public DbSet<Orders> Orders { get; set; }
        public DbSet<OrdersLine> OrdersLine { get; set; }
        public DbSet<LoadIn> LoadIn { get; set; }
        public DbSet<LoadOut> LoadOut { get; set; }
        public DbSet<FiscalNotes> FiscalNotes { get; set; }
        public DbSet<FnLine> FnLine { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Orders>().ToTable("ORDERS");
            modelBuilder.Entity<OrdersLine>().ToTable("ORDERS_LINE");
            modelBuilder.Entity<LoadIn>().ToTable("LOAD_IN");
            modelBuilder.Entity<LoadOut>().ToTable("LOAD_OUT");
            modelBuilder.Entity<FiscalNotes>().ToTable("FISCAL_NOTES");
            modelBuilder.Entity<FnLine>().ToTable("FN_LINE");

            modelBuilder.Entity<Orders>().HasKey(x => x.OeId);
            modelBuilder.Entity<OrdersLine>().HasKey(x => x.OelId);
            modelBuilder.Entity<LoadIn>().HasKey(x => x.LiId);
            modelBuilder.Entity<LoadOut>().HasKey(x => x.LoId);
            modelBuilder.Entity<FiscalNotes>().HasKey(x => x.FnId);
            modelBuilder.Entity<FnLine>().HasKey(x => x.FnlId);

            base.OnModelCreating(modelBuilder);
        }
    }
}