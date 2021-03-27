﻿namespace Doctors.EntityFramework
{
    using Domain.DoctorsAggregate;
    using Microsoft.EntityFrameworkCore;

    public class DoctorContext : DbContext
    {
        public DbSet<Doctor> doctors { get; set; }
        public DbSet<Specialization> specializations { get; set; }

        public DoctorContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Doctor>()
                .Property(e => e.Sex)
                .HasConversion<string>();
            modelBuilder.Entity<Doctor>()
                .HasMany<Specialization>(s => s.Specializations)
                .WithMany(c => c.Doctors);
        }
    }
}