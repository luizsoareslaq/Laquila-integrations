using Laquila.Integrations.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Laquila.Integrations.Infrastructure.Contexts
{
    public class LaquilaHubContext : DbContext
    {
        public LaquilaHubContext(DbContextOptions<LaquilaHubContext> options)
            : base(options)
        {
        }

        public DbSet<LaqApiStatus> LaqApiStatus { get; set; }
        public DbSet<LaqApiIntegrations> LaqApiIntegrations { get; set; }
        public DbSet<LaqApiUsers> LaqApiUsers { get; set; }
        public DbSet<LaqApiAuthTokens> LaqApiAuthTokens { get; set; }
        public DbSet<LaqApiLogs> LaqApiLogs { get; set; }
        public DbSet<LaqApiSyncQueue> LaqApiSyncQueues { get; set; }
        public DbSet<LaqApiUrlIntegrations> LaqApiUrlIntegrations { get; set; }
        public DbSet<LaqApiUserRoles> LaqApiUserRoles { get; set; }
        public DbSet<LaqApiRoles> LaqApiRoles { get; set; }
        public DbSet<LaqApiCompany> LaqApiCompanies { get; set; }
        public DbSet<LaqApiUserCompanies> LaqApiUserCompanies { get; set; }
        public DbSet<LaqApiUserIntegrations> LaqApiUserIntegrations { get; set; }
        public DbSet<LaqApiIntegrationCompanies> LaqApiIntegrationCompanies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LaqApiStatus>(entity =>
            {
                entity.ToTable("laq_api_status");
            });

            modelBuilder.Entity<LaqApiRoles>(entity =>
            {
                entity.ToTable("laq_api_roles");
            });

            modelBuilder.Entity<LaqApiUserRoles>(entity =>
            {
                entity.ToTable("laq_api_users_roles");

                entity.HasKey(e => new { e.UserId, e.RoleId });

                entity.HasOne(e => e.User)
                    .WithMany(u => u.UserRoles)
                    .HasForeignKey(e => e.UserId);

                entity.HasOne(e => e.Role)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(e => e.RoleId);
            });

            modelBuilder.Entity<LaqApiIntegrationCompanies>(entity =>
            {
                entity.ToTable("laq_api_company_integrations");

                entity.HasKey(e => new { e.CompanyId, e.ApiIntegrationId });

                entity.HasOne(e => e.Company)
                    .WithMany(u => u.CompaniesIntegration)
                    .HasForeignKey(e => e.CompanyId);

                entity.HasOne(e => e.Integration)
                    .WithMany(r => r.IntegrationCompanies)
                    .HasForeignKey(e => e.ApiIntegrationId);
            });

            modelBuilder.Entity<LaqApiUserIntegrations>(entity =>
            {
                entity.ToTable("laq_api_users_integrations");

                entity.HasKey(e => new { e.UserId, e.IntegrationId });

                entity.HasOne(e => e.User)
                    .WithMany(u => u.UserIntegrations)
                    .HasForeignKey(e => e.UserId);

                entity.HasOne(e => e.Integration)
                    .WithMany(r => r.UserIntegrations)
                    .HasForeignKey(e => e.IntegrationId);
            });

            modelBuilder.Entity<LaqApiUserCompanies>(entity =>
            {
                entity.ToTable("laq_api_users_companies");

                entity.HasKey(e => new { e.UserId, e.CompanyId });

                entity.HasOne(e => e.User)
                    .WithMany(u => u.UserCompanies)
                    .HasForeignKey(e => e.UserId);

                entity.HasOne(e => e.Company)
                    .WithMany(c => c.UserCompanies)
                    .HasForeignKey(e => e.CompanyId);
            });

            modelBuilder.Entity<LaqApiIntegrations>(entity =>
            {
                entity.ToTable("laq_api_integrations");
                entity.HasOne(e => e.Status)
                    .WithOne()
                    .HasForeignKey<LaqApiIntegrations>(i => i.StatusId);
            });

            modelBuilder.Entity<LaqApiUsers>(entity =>
            {
                entity.ToTable("laq_api_users");
                entity.HasOne(e => e.Status)
                    .WithOne()
                    .HasForeignKey<LaqApiUsers>(i => i.StatusId);

            });

            modelBuilder.Entity<LaqApiCompany>(entity =>
            {
                entity.ToTable("laq_api_company");
                entity.HasOne(e => e.Status)
                    .WithOne()
                    .HasForeignKey<LaqApiCompany>(i => i.StatusId);
            });

            modelBuilder.Entity<LaqApiAuthTokens>(entity =>
            {
                entity.ToTable("laq_api_auth_tokens");

                entity.HasOne(x => x.User)
                    .WithMany(u => u.AuthTokens)
                    .HasForeignKey(x => x.ApiUserId)
                    .HasPrincipalKey(u => u.Id)
                    .OnDelete(DeleteBehavior.Cascade);

            });

            modelBuilder.Entity<LaqApiLogs>(entity =>
            {
                entity.ToTable("laq_api_logs");
                entity.HasOne(e => e.User)
                      .WithMany()
                      .HasForeignKey(e => e.ApiUserId);
            });

            modelBuilder.Entity<LaqApiSyncQueue>(entity =>
            {
                entity.ToTable("laq_api_sync_queue");

                entity.HasOne(e => e.Status)
                    .WithOne()
                    .HasForeignKey<LaqApiSyncQueue>(i => i.StatusId);
            });

            modelBuilder.Entity<LaqApiUrlIntegrations>(entity =>
            {
                entity.ToTable("laq_api_url_integrations");

                // entity.Ignore(e => e.Integrations);
                // entity.Ignore(e => e.Status);
            });
        }
    }
}