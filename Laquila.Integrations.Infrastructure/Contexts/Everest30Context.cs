using Laquila.Integrations.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Laquila.Integrations.Infrastructure.Contexts
{
    using Laquila.Integrations.Core.Domain.Models;
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

        public DbSet<VMWMS_BuscarPrenotasNaoIntegradas> VMWMS_BuscarPrenotasNaoIntegradas { get; set; }
        public DbSet<VMWMS_BuscarItensNaoIntegrados> VMWMS_BuscarItensNaoIntegrados { get; set; }
        public DbSet<VMWMS_BuscarCadastrosNaoIntegrados> VMWMS_BuscarCadastrosNaoIntegrados { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Orders>(entity =>
            {
                entity.ToTable("ORDERS");
                entity.HasKey(e => e.OeId);
                entity.HasOne(o => o.LoadOut)
                    .WithMany(l => l.Orders)
                    .HasForeignKey(o => o.OeLoId);

                entity.HasMany(o => o.OrdersLines)
                    .WithOne(ol => ol.Orders)
                    .HasForeignKey(ol => ol.OelOeId);
            });

            modelBuilder.Entity<OrdersLine>(entity =>
            {
                entity.ToTable("ORDERS_LINE");
                entity.HasKey(e => e.OelId);

                entity.HasOne(ol => ol.Orders)
                    .WithMany(o => o.OrdersLines)
                    .HasForeignKey(ol => ol.OelOeId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<LoadIn>().ToTable("LOAD_IN");
            modelBuilder.Entity<LoadOut>().ToTable("LOAD_OUT");
            modelBuilder.Entity<FiscalNotes>().ToTable("FISCAL_NOTES");
            modelBuilder.Entity<FnLine>().ToTable("FN_LINE");

            modelBuilder.Entity<LoadIn>().HasKey(x => x.LiId);
            modelBuilder.Entity<LoadOut>().HasKey(x => x.LoId);
            modelBuilder.Entity<FiscalNotes>().HasKey(x => x.FnId);
            modelBuilder.Entity<FnLine>().HasKey(x => x.FnlId);

            modelBuilder.Entity<VMWMS_BuscarPrenotasNaoIntegradas>().HasNoKey().ToView("VMWMS_BuscarPrenotasNaoIntegradas");
            modelBuilder.Entity<VMWMS_BuscarItensNaoIntegrados>().HasNoKey().ToView("VMWMS_BuscarItensNaoIntegrados");
            modelBuilder.Entity<VMWMS_BuscarCadastrosNaoIntegrados>().HasNoKey().ToView("VMWMS_BuscarCadastrosNaoIntegrados");

            base.OnModelCreating(modelBuilder);
        }
    }
}