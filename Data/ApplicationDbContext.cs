using Microsoft.EntityFrameworkCore;
using product_student_manager.Models;

using System.Collections.Generic;
using System.Reflection.Emit;

namespace ProjectStudentAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Project> Projects { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<ProjectStudent> ProjectStudents { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           
            modelBuilder.Entity<ProjectStudent>()
                .HasKey(ps => new { ps.ProjectId, ps.StudentId });

            modelBuilder.Entity<ProjectStudent>()
                .HasOne(ps => ps.Project)
                .WithMany(p => p.ProjectStudents)
                .HasForeignKey(ps => ps.ProjectId);

            modelBuilder.Entity<ProjectStudent>()
                .HasOne(ps => ps.Student)
                .WithMany(s => s.ProjectStudents)
                .HasForeignKey(ps => ps.StudentId);

            modelBuilder.Entity<Project>()
                .HasIndex(p => p.Title)
                .IsUnique();
        }
    }
}
