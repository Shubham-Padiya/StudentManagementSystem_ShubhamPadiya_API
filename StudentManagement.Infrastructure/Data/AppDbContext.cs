using Microsoft.EntityFrameworkCore;
using StudentManagement.Domain.Entities;

namespace StudentManagement.Infrastructure.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options) { }

        public DbSet<Student> Students { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<StudentClass> StudentClasses {  get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Student>(entity =>
            {
                entity.HasIndex(x => x.EmailId).IsUnique();
            });

            modelBuilder.Entity<StudentClass>(entity =>
            {
                entity.HasKey(x => new { x.StudentId, x.ClassId });

                entity.HasOne(x => x.Student)
                      .WithMany(x => x.StudentClasses)
                      .HasForeignKey(x => x.StudentId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(x => x.Class)
                      .WithMany(x => x.StudentClasses)
                      .HasForeignKey(x => x.ClassId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
