using Microsoft.EntityFrameworkCore;
using ApiInteligenteDeTareasYAnalisis.Models;

namespace ApiInteligenteDeTareasYAnalisis.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Tarea> Tareas { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configurar la entidad Tarea
            modelBuilder.Entity<Tarea>(entity =>
            {
                entity.ToTable("Tareas");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Titulo)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(2000);

                entity.Property(e => e.Estado)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Prioridad)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.FechaCreacion)
                    .IsRequired();

                entity.Property(e => e.FechaVencimiento)
                    .IsRequired();
            });
        }
    }
}
