using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PortalAcademico.Models;

namespace PortalAcademico.Data;

public class ApplicationDbContext : IdentityDbContext<IdentityUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Curso> Cursos { get; set; } = null!;
    public DbSet<Matricula> Matriculas { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ========================
        // CURSO
        // ========================
        modelBuilder.Entity<Curso>(entity =>
        {
            entity.HasKey(c => c.Id);

            entity.Property(c => c.Nombre)
                  .IsRequired()
                  .HasMaxLength(100);

            entity.Property(c => c.Codigo)
                  .IsRequired()
                  .HasMaxLength(20);

            entity.Property(c => c.Creditos)
                  .IsRequired();

            // Índice único
            entity.HasIndex(c => c.Codigo)
                  .IsUnique();

            // Constraint (SQLite compatible)
            entity.ToTable("Cursos", t =>
            {
                t.HasCheckConstraint("CK_Curso_Creditos", "Creditos > 0");
            });
        });

        // ========================
        // MATRICULA
        // ========================
        modelBuilder.Entity<Matricula>(entity =>
        {
            entity.HasKey(m => m.Id);

            entity.Property(m => m.UsuarioId)
                  .IsRequired();

            entity.Property(m => m.CursoId)
                  .IsRequired();

            entity.Property(m => m.Estado)
                  .IsRequired()
                  .HasMaxLength(20);

            entity.Property(m => m.FechaRegistro)
                  .HasDefaultValueSql("CURRENT_TIMESTAMP");

            // Relación
            entity.HasOne(m => m.Curso)
                  .WithMany(c => c.Matriculas)
                  .HasForeignKey(m => m.CursoId)
                  .OnDelete(DeleteBehavior.Cascade);

            // Evitar duplicados
            entity.HasIndex(m => new { m.CursoId, m.UsuarioId })
                  .IsUnique();
        });
    }
}