using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace TaxiManagementSystem.Models
{
    public partial class TaxiManagementSystemContext : DbContext
    {
        public TaxiManagementSystemContext()
        {
        }

        public TaxiManagementSystemContext(DbContextOptions<TaxiManagementSystemContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Driver> Driver { get; set; }
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<Earnings> Earnings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Driver>(entity =>
            {
                entity.HasKey(e => e.DriverId)
                    .HasName("PK__Driver__F1B1CD04A76BD14F");

                entity.Property(e => e.DriversLicense)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Earnings>(entity =>
            {
                entity.HasKey(e => e.EarningId)
                    .HasName("PK__Earnings__2418A122638DE9B4");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Earnings)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Earnings__Income__398D8EEE");
            });


            modelBuilder.Entity<Users>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("PK__Users__1788CC4C3D84AFB1");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
