using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProyectoFinal_FernandezCastilloJosePablo.Models;

namespace ProyectoFinal_FernandezCastilloJosePablo.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Carrera> Carreras => Set<Carrera>();
    public DbSet<Curso> Cursos => Set<Curso>();
    public DbSet<Docente> Docentes => Set<Docente>();
    public DbSet<Estudiante> Estudiantes => Set<Estudiante>();
    public DbSet<Matricula> Matriculas => Set<Matricula>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Carrera>().HasIndex(c => c.Codigo).IsUnique();
        builder.Entity<Curso>().HasIndex(c => c.Codigo).IsUnique();
        builder.Entity<Estudiante>().HasIndex(e => e.Cedula).IsUnique();

        builder.Entity<Curso>()
            .HasOne(c => c.Carrera).WithMany(c => c.Cursos)
            .HasForeignKey(c => c.CarreraId).OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Curso>()
            .HasOne(c => c.Docente).WithMany(d => d.Cursos)
            .HasForeignKey(c => c.DocenteId).OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Estudiante>()
            .HasOne(e => e.Carrera).WithMany(c => c.Estudiantes)
            .HasForeignKey(e => e.CarreraId).OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Matricula>()
            .HasOne(m => m.Estudiante).WithMany(e => e.Matriculas)
            .HasForeignKey(m => m.EstudianteId).OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Matricula>()
            .HasOne(m => m.Curso).WithMany(c => c.Matriculas)
            .HasForeignKey(m => m.CursoId).OnDelete(DeleteBehavior.Restrict);
    }
}
