using Microsoft.AspNetCore.Identity; 
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RegistroDeEstudantes.Models;

namespace RegistroDeEstudantes.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Student> Students { get; set; } = default!;
        public DbSet<Premium> Premiums { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Student>()
                .HasIndex(s => s.Email)
                .IsUnique();

            modelBuilder.Entity<Premium>()
                .HasOne(p => p.Student)
                .WithMany(s => s.Premiums)
                .HasForeignKey(p => p.StudentId)
                .OnDelete(DeleteBehavior.Restrict);
        } 
    } 
}