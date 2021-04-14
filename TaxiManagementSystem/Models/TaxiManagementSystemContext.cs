using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using TaxiManagementSystem.Models;

namespace TaxiManagementSystem.Model
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
        public virtual DbSet<Earnings> Earnings { get; set; }
        public virtual DbSet<Owner> Owner { get; set; }
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<OwnerDriver> OwnerDriver { get; set; }

        public virtual DbSet<Taxi> Taxi { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=DESKTOP-NPG6LGU;Database=TaxiManagementSystem;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Driver>(entity =>
            {
                entity.HasKey(e => e.DriverId)
                  .HasName("PK__Driver__F1B1CD04C663AF06");

                entity.Property(e => e.DriversLicense)
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

                entity.HasOne(d => d.User)
                   .WithMany(p => p.Driver)
                   .HasForeignKey(d => d.UserId)
                   .OnDelete(DeleteBehavior.ClientSetNull)
                   .HasConstraintName("FK__Driver__UserId__2B3F6F97");
            });

            modelBuilder.Entity<Earnings>(entity =>
            {
                entity.HasKey(e => e.EarningId)
                    .HasName("PK__Earnings__2418A1222A2668CA");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Earnings)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Earnings__UserId__286302EC");
            });

            modelBuilder.Entity<Owner>(entity =>
            {
                entity.HasKey(e => e.OwnerId)
                  .HasName("PK__Owner__819385B85BF30EB0");

                entity.Property(e => e.Email).IsUnicode(false);

                entity.Property(e => e.FirstName).IsUnicode(false);

                entity.Property(e => e.LastName).IsUnicode(false);

                entity.HasOne(d => d.User)
                   .WithMany(p => p.Owner)
                   .HasForeignKey(d => d.UserId)
                   .OnDelete(DeleteBehavior.ClientSetNull)
                   .HasConstraintName("FK__Owner__UserId__2E1BDC42");
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("PK__Users__1788CC4CA4122068");


                object p = entity.Property(e => e.IsOwner).HasDefaultValueSql("((0))");

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

            modelBuilder.Entity<OwnerDriver>(entity =>
            {
                object p = entity.Property(e => e.IsActiveDriver).HasDefaultValueSql("((1))");
                entity.HasKey(e => e.OwnerDriverId)
                    .HasName("PK__OwnerDri__6B77586316BEBE43");

                entity.HasOne(d => d.Driver)
                    .WithMany(p => p.OwnerDriver)
                    .HasForeignKey(d => d.DriverId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__OwnerDriv__Drive__32E0915F");

                entity.HasOne(d => d.Owner)
                    .WithMany(p => p.OwnerDriver)
                    .HasForeignKey(d => d.OwnerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__OwnerDriv__Owner__31EC6D26");
            });

            modelBuilder.Entity<Taxi>(entity =>
            {
                entity.HasKey(e => e.TaxiId)
                    .HasName("PK__Taxi__6696EA2BF3F574EF");
                entity.Property(e => e.Make).IsUnicode(false);

                entity.Property(e => e.Model).IsUnicode(false);

                entity.Property(e => e.Registration).IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
