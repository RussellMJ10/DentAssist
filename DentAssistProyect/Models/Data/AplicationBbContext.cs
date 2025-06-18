// Data/ApplicationDbContext.cs
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using DentAssistProyect.Models.Entities;

namespace DentAssistProyect.Models.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Paciente> Pacientes { get; set; }
        public DbSet<Odontologo> Odontologos { get; set; }
        public DbSet<Turno> Turnos { get; set; }
        public DbSet<Tratamiento> Tratamientos { get; set; }
        public DbSet<PlanTratamiento> PlanesTratamiento { get; set; }
        public DbSet<PasoPlan> PasosPlan { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Turno>()
                .HasOne(t => t.Paciente)
                .WithMany(p => p.Turnos)
                .HasForeignKey(t => t.PacienteId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Turno>()
                .HasOne(t => t.Odontologo)
                .WithMany(o => o.Turnos)
                .HasForeignKey(t => t.OdontologoId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<PlanTratamiento>()
                .HasOne(p => p.Paciente)
                .WithMany(p => p.PlanesTratamiento)
                .HasForeignKey(p => p.PacienteId);

            builder.Entity<PlanTratamiento>()
                .HasOne(p => p.Odontologo)
                .WithMany(o => o.PlanesTratamiento)
                .HasForeignKey(p => p.OdontologoId);

            builder.Entity<PasoPlan>()
                .HasOne(p => p.PlanTratamiento)
                .WithMany(p => p.Pasos)
                .HasForeignKey(p => p.PlanTratamientoId);

            builder.Entity<PasoPlan>()
                .HasOne(p => p.Tratamiento)
                .WithMany()
                .HasForeignKey(p => p.TratamientoId);
        }
    }
}