﻿using Microsoft.EntityFrameworkCore;

using P01_HospitalDatabase.Data.Models;

namespace P01_HospitalDatabase.Data
{
    public class HospitalContext : DbContext
    {
        public HospitalContext()
        {
        }

        public HospitalContext(DbContextOptions options)
        : base(options)
        {
        }

        public DbSet<Patient> Patients { get; set; }

        public DbSet<Visitation> Visitations { get; set; }

        public DbSet<Diagnose> Diagnoses { get; set; }

        public DbSet<PatientMedicament> PatientMedicaments { get; set; }

        public DbSet<Medicament> Medicaments { get; set; }
        
        public DbSet<Doctor> Doctor { get; set; } //might be an error in Judge


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Configuration.ConnectionString);
            }

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Patient>(entity =>
            {
                entity
                    .HasKey(p => p.PatientId);

                entity
                    .Property(p => p.FirstName)
                    .IsRequired(true)
                    .IsUnicode(true)
                    .HasMaxLength(50);

                entity
                    .Property(p => p.LastName)
                    .IsRequired(true)
                    .IsUnicode(true)
                    .HasMaxLength(50);

                entity
                    .Property(p => p.Address)
                    .IsUnicode(true)
                    .HasMaxLength(250);

                entity
                    .Property(p => p.Email)
                    .IsUnicode(false)
                    .HasMaxLength(80);
            });

            modelBuilder.Entity<Visitation>(entity =>
            {
                entity
                    .HasKey(v => v.VisitationId);

                entity
                    .Property(v => v.Comments)
                    .IsUnicode(true)
                    .HasMaxLength(250);

                entity
                    .HasOne(v => v.Patient)
                    .WithMany(p => p.Visitations)
                    .HasForeignKey(v => v.PatientId);

                entity
                    .HasOne(v => v.Doctor)
                    .WithMany(d => d.Visitations)
                    .HasForeignKey(v => v.DoctorId);
            });

            modelBuilder.Entity<Diagnose>(entity =>
            {
                entity
                    .HasKey(d => d.DiagnoseId);

                entity
                    .Property(d => d.Name)
                    .IsRequired(true)
                    .IsUnicode(true)
                    .HasMaxLength(50);

                entity
                    .Property(d => d.Comments)
                    .IsUnicode(true)
                    .HasMaxLength(250);

                entity
                    .HasOne(d => d.Patient)
                    .WithMany(p => p.Diagnoses)
                    .HasForeignKey(d => d.PatientId);
            });

            modelBuilder.Entity<Medicament>(entity =>
            {
                entity
                    .HasKey(m => m.MedicamentId);

                entity
                    .Property(m => m.Name)
                    .IsRequired(true)
                    .IsUnicode(true)
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<PatientMedicament>(entity =>
            {
                entity
                    .HasKey(pm => new { pm.PatientId, pm.MedicamentId });

                entity
                    .HasOne(pm => pm.Patient)
                    .WithMany(p => p.Prescriptions)
                    .HasForeignKey(pm => pm.PatientId);

                entity
                    .HasOne(pm => pm.Medicament)
                    .WithMany(m => m.Prescriptions)
                    .HasForeignKey(pm => pm.MedicamentId);
            });

            modelBuilder.Entity<Doctor>(entity =>
            {
                entity
                    .HasKey(d => d.DoctorId);

                entity
                    .Property(d => d.Name)
                    .IsRequired(true)
                    .IsUnicode(true)
                    .HasMaxLength(100);

                entity
                    .Property(d => d.Specialty)
                    .IsRequired(true)
                    .IsUnicode(true)
                    .HasMaxLength(100);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
