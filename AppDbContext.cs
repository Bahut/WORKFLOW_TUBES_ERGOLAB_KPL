using Microsoft.EntityFrameworkCore;
using WORKFLOW_TUBES_KPL_ERGOLAB.Models;

namespace WORKFLOW_TUBES_KPL_ERGOLAB.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Complaint> Complaints { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Complaint>(entity =>
            {
                entity.ToTable("complaints");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Title).HasColumnName("title");
                entity.Property(e => e.Category).HasColumnName("category");
                entity.Property(e => e.Description).HasColumnName("description");
                entity.Property(e => e.Location).HasColumnName("location");
                entity.Property(e => e.Reporter).HasColumnName("reporter");
                entity.Property(e => e.Status).HasColumnName("status");
            });
        }
    }
}