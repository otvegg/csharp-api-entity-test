using Microsoft.EntityFrameworkCore;
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
            _connectionString = configuration.GetValue<string>("ConnectionStrings:DefaultConnection")!;
            this.Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //TODO: Appointment Key etc.. Add Here
            modelBuilder.Entity<Doctor>().HasData(
                new Doctor { Id = 1, FullName = "Dr. Smith" },
                new Doctor { Id = 2, FullName = "Dr. Johnson" }
            );
            modelBuilder.Entity<Appointment>().HasKey(a => new { a.PatientId, a.DoctorId });

            DateTime somedate = new DateTime(2020,12,05, 0, 0, 0, DateTimeKind.Utc);
            modelBuilder.Entity<Appointment>().HasData(
                new Appointment {PatientId = 1, DoctorId = 1, Booktime = somedate.AddDays(1) },
                new Appointment {PatientId = 2, DoctorId = 1, Booktime = somedate.AddDays(2) },
                new Appointment {PatientId = 1, DoctorId = 2, Booktime = somedate.AddDays(3) }
            );


            Seeder seeder = new Seeder();
            modelBuilder.Entity<Patient>().HasData(seeder.Patients);
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
