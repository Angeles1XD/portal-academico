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

    public DbSet<Curso> Cursos { get; set; }
    public DbSet<Matricula> Matriculas { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Curso>()
            .HasIndex(c => c.Codigo)
            .IsUnique();

        modelBuilder.Entity<Curso>()
            .ToTable(t => t.HasCheckConstraint("CK_Creditos", "Creditos > 0"));

        modelBuilder.Entity<Curso>()
            .ToTable(t => t.HasCheckConstraint("CK_Horario", "HorarioInicio < HorarioFin"));

        modelBuilder.Entity<Matricula>()
            .HasIndex(m => new { m.CursoId, m.UsuarioId })
            .IsUnique();
    }
}