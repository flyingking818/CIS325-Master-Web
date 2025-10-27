// AdvisingContext.cs ----------------------------------------
using Microsoft.EntityFrameworkCore;
using CIS325_Master_Web.Models;

namespace CIS325_Master_Web.Data;

/// <summary>EF Core context that manages database connections and entities.</summary>
// DbContext represents a DB session/bridge (connection, configurations, etc.)
public class AdvisingContext : DbContext
{
    public AdvisingContext(DbContextOptions<AdvisingContext> options) : base(options) { }
    //DbSet represents a table in EF
    public DbSet<Student> Students => Set<Student>();   // Table: Students
    public DbSet<Course> Courses => Set<Course>();    // Table: Courses
    public DbSet<Schedule> Schedules => Set<Schedule>(); // Table: Schedules

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Relationships --------------------------------------
        modelBuilder.Entity<Schedule>()
            .HasOne(s => s.Student) //navigation from from schedule to student
            .WithMany(st => st.Schedules) //navigation from student to many schedules
            .HasForeignKey(s => s.StudentId) //FK column on schedule (studentId)
            .OnDelete(DeleteBehavior.Cascade); //when a student is deleted, it delete related schedules

        modelBuilder.Entity<Schedule>()
            .HasOne(s => s.Course)
            .WithMany(c => c.Schedules)
            .HasForeignKey(s => s.CourseId)
            .OnDelete(DeleteBehavior.Cascade);

        // Seed initial data -----------------------------------
        modelBuilder.Entity<Student>().HasData(
            new Student { Id = 1, FirstName = "Jay", LastName = "Hawkins", Email = "jay@flagler.edu" },
            new Student { Id = 2, FirstName = "Molly", LastName = "Sloan", Email = "molly@flagler.edu" }
        );

        modelBuilder.Entity<Course>().HasData(
            new Course { Id = 1, Code = "CIS325", Title = "Server-Side Web Development", Credits = 3 },
            new Course { Id = 2, Code = "CIS205", Title = "Programming I (C#)", Credits = 3 }
        );
    }
}