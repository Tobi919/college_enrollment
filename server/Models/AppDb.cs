using Microsoft.EntityFrameworkCore;
using WebSqliteApp.Models;

namespace WebSqliteApp.Models;

public class AppDb : DbContext
{
    public AppDb(DbContextOptions<AppDb> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Student> Students => Set<Student>();
    public DbSet<Course> Courses => Set<Course>();
    public DbSet<Enrollment> Enrollments => Set<Enrollment>();


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        modelBuilder.Entity<Student>()
            .HasIndex(u => u.Email)
            .IsUnique();

        modelBuilder.Entity<Enrollment>()
            .HasIndex(u => u.StudentId);

        modelBuilder.Entity<Enrollment>()
            .HasIndex(u => u.CourseId);
    }

public DbSet<WebSqliteApp.Models.Enrollment> Enrollment { get; set; } = default!;
}
