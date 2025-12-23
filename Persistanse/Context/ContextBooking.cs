using Domain.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistanse.Context
{
    public class ContextBooking:DbContext
    {
        public ContextBooking(DbContextOptions<ContextBooking> options) : base(options)
        {
        }

        public DbSet<Doctor> doctors { get; set; } = default!;
        public DbSet<Specialty> specialties { get; set; } = default!;
        public DbSet<Slot> slots { get; set; } = default!;
        public DbSet<Appointment> appointments { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 1. تجاهل ApplicationUser تماماً لمنع إنشاء جدول فاضي
            modelBuilder.Ignore<ApplicationUser>();

            base.OnModelCreating(modelBuilder);

            // 2. تعريف UserId كخاصية عادية (بدون علاقة FK)
            modelBuilder.Entity<Doctor>()
                .Property(d => d.UserId)
                .IsRequired();
            modelBuilder.Entity<PaymentTransaction>()
    .Property(p => p.Amount)
    .HasPrecision(18, 2);

            modelBuilder.Entity<PaymentTransaction>()
                .HasOne(p => p.Appointment)
                .WithMany(a => a.PaymentTransactions)
                .HasForeignKey(p => p.AppointmentId);

            // Slot → Doctor
            modelBuilder.Entity<Slot>()
                .HasOne(s => s.Doctor)
                .WithMany(d => d.Slots)
                .HasForeignKey(s => s.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);

            // Appointment → Doctor
            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Doctor)
                .WithMany(d => d.Appointments)
                .HasForeignKey(a => a.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);

            // Appointment → Slot
            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Slot)
                .WithOne(s => s.Appointment)
                .HasForeignKey<Appointment>(a => a.SlotId)
                .OnDelete(DeleteBehavior.Restrict);

            // Doctor → Specialty
            modelBuilder.Entity<Doctor>()
                .HasOne(d => d.Specialty)
                .WithMany(s => s.Doctors)
                .HasForeignKey(d => d.SpecialtyId)
                .OnDelete(DeleteBehavior.Cascade);

            // Decimal precision
            modelBuilder.Entity<Doctor>()
                .Property(d => d.SessionPrice)
                .HasPrecision(18, 2);
        }

    }
}
