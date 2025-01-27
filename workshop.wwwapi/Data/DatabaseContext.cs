﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using workshop.wwwapi.Models;

namespace workshop.wwwapi.Data
{
    public class DatabaseContext : DbContext
    {
        private string _connectionString;
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            _connectionString = configuration.GetValue<string>("ConnectionStrings:DefaultConnectionString")!;
            this.Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //TODO: Appointment Key etc.. Add Here
            modelBuilder.Entity<Appointment>()
                .HasKey(a => new { a.DoctorId, a.PatientId });


            //TODO: Seed Data Here

            modelBuilder.Entity<Patient>().HasData(
               new Patient { Id = 1, FullName = "Erling Haaland" },
               new Patient { Id = 2, FullName = "Martin Ødegaard" }
           );

            modelBuilder.Entity<Doctor>().HasData(
                new Doctor { Id = 1, FullName = "Dr Larsen " },
               new Doctor { Id = 2, FullName = "Dr Hansen" }
           );

            modelBuilder.Entity<Appointment>().HasData(
               new Appointment { DoctorId = 1, PatientId = 1, Booking = new DateTime(2025, 2, 5, 8, 0, 0) },
               new Appointment { DoctorId = 1, PatientId = 2, Booking = new DateTime(2025, 2, 10, 11, 0, 0) },
               new Appointment { DoctorId = 2, PatientId = 1, Booking = new DateTime(2025, 3, 20, 14, 30, 0) },
               new Appointment { DoctorId = 2, PatientId = 2, Booking = new DateTime(2025, 3, 30, 15, 0, 0) }
           );
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseInMemoryDatabase(databaseName: "Database");
            optionsBuilder.UseNpgsql(_connectionString);
            optionsBuilder.LogTo(message => Debug.WriteLine(message)); //see the sql EF using in the console
            
        }


        public DbSet<Patient> Patients { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
    }
}
