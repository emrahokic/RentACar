using System;
using System.Collections.Generic;
using System.Text;
using RentACar.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace RentACar.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        public DbSet<DodatneUsluge> DodatneUsluge { get; set; }
        public DbSet<Drzava> Drzava { get; set; }
        public DbSet<Grad> Grad { get; set; }
        public DbSet<KompatibilnostPrikolica> KompatibilnostPrikolica { get; set; }
        public DbSet<OstecenjeInfo> OstecenjeInfo { get; set; }
        public DbSet<Poslovnica> Poslovnica { get; set; }
        public DbSet<Prijevoz> Prijevoz { get; set; }
        public DbSet<PrijevozVozilo> PrijevozVozilo { get; set; }
        public DbSet<Prikolica> Prikolica { get; set; }
        public DbSet<Regija> Regija { get; set; }
        public DbSet<Rezervacija> Rezervacija { get; set; }
        public DbSet<RezervisanaUsluga> RezervisanaUsluga { get; set; }
        public DbSet<TrenutnaPoslovnica> TrenutnaPoslovnica { get; set; }
        public DbSet<UposlenikPrijevoz> UposlenikPrijevoz { get; set; }
        public DbSet<Vozilo> Vozilo { get; set; }
        public DbSet<UgovorZaposlenja> UgovorZaposlenja { get; set; }
        public DbSet<Slika> Slika { get; set; }
        public DbSet<OcjenaPrijevoz> OcjenaPrijevoz { get; set; }
        public DbSet<OcjenaRezervacija> OcjenaRezervacija { get; set; }
        public DbSet<Brend> Brend { get; set; }




        protected override void OnModelCreating(ModelBuilder builder) 
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>(entity =>
            {
                entity.ToTable(name: "User");
                entity.Property(e => e.Id).HasColumnName("UserID");

            });

            builder.Entity<IdentityRole<int>>(entity =>
            {
                entity.ToTable(name: "Role");
                entity.Property(e => e.Id).HasColumnName("RoleID");

            });

            builder.Entity<IdentityUserClaim<int>>(entity =>
            {
                entity.ToTable("UserClaim");
                entity.Property(e => e.UserId).HasColumnName("UserID");
                entity.Property(e => e.Id).HasColumnName("UserClaimID");

            });

            builder.Entity<IdentityUserLogin<int>>(entity =>
            {
                entity.ToTable("UserLogin");
                entity.Property(e => e.UserId).HasColumnName("UserID");

            });

            builder.Entity<IdentityRoleClaim<int>>(entity =>
            {
                entity.ToTable("RoleClaim");
                entity.Property(e => e.Id).HasColumnName("RoleClaimID");
                entity.Property(e => e.RoleId).HasColumnName("RoleID");
            });

            builder.Entity<IdentityUserRole<int>>(entity =>
            {
                entity.ToTable("UserRole");
                entity.Property(e => e.UserId).HasColumnName("UserID");
                entity.Property(e => e.RoleId).HasColumnName("RoleID");

            });


            builder.Entity<IdentityUserToken<int>>(entity =>
            {
                entity.ToTable("UserToken");
                entity.Property(e => e.UserId).HasColumnName("UserID");

            });

        }
    }

}
