using Microsoft.EntityFrameworkCore;
using PharmacyApp.Infrastructure.Models;

namespace PharmacyApp.Infrastructure
{
    public class PharmacyAppContext : DbContext
    {
        public PharmacyAppContext(DbContextOptions<PharmacyAppContext> options)
            : base(options)
        {
        }

        public DbSet<MedicineModel> Medicines { get; set; }
        public DbSet<PrescriptionModel> Prescriptions { get; set; }
        public DbSet<CustomerModel> Customers { get; set; }
        public DbSet<PharmacyModel> Pharmacies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<MedicineModel>().ToTable("Medicines").HasKey(m => m.Id);
            modelBuilder.Entity<PrescriptionModel>().ToTable("Prescriptions");

            // Medicine properties
            modelBuilder.Entity<MedicineModel>()
                .Property(m => m.Name).IsRequired().HasMaxLength(200);
            modelBuilder.Entity<MedicineModel>()
                .Property(m => m.Price).IsRequired();
            modelBuilder.Entity<MedicineModel>()
                .Property(m => m.QuantityInStock).IsRequired();

            // Prescription additional
            modelBuilder.Entity<PrescriptionModel>()
                .Property(p => p.DoctorName).HasMaxLength(200);

            // Customer
            modelBuilder.Entity<CustomerModel>().ToTable("Customers");
            modelBuilder.Entity<CustomerModel>().HasKey(c => c.Id);
            modelBuilder.Entity<CustomerModel>()
                .Property(c => c.Name).IsRequired().HasMaxLength(200);

            // Pharmacy
            modelBuilder.Entity<PharmacyModel>().ToTable("Pharmacies");
            modelBuilder.Entity<PharmacyModel>().HasKey(p => p.Id);
            modelBuilder.Entity<PharmacyModel>()
                .Property(p => p.Name).IsRequired().HasMaxLength(200);

            // Pharmacy 1 - * Medicines
            modelBuilder.Entity<PharmacyModel>()
                .HasMany(p => p.Medicines)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            // Customer 1 - * Prescriptions
            modelBuilder.Entity<CustomerModel>()
                .HasMany(c => c.Prescriptions)
                .WithOne(p => p.Customer)
                .HasForeignKey(p => p.CustomerId)
                .OnDelete(DeleteBehavior.SetNull);

            // Pharmacy 1 - 1 Customer (Contact)
            modelBuilder.Entity<PharmacyModel>()
                .HasOne(p => p.ContactCustomer)
                .WithOne(c => c.PharmacyContactFor)
                .HasForeignKey<PharmacyModel>(p => p.ContactCustomerId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
