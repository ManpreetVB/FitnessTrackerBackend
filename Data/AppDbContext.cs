using Fitness_Tracker.Models;
using Microsoft.EntityFrameworkCore;
namespace Fitness_Tracker.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

       
        public DbSet<Member> Members { get; set; }
        public DbSet<Trainer> Trainers { get; set; }
        public DbSet<WorkoutPlan> WorkoutPlans { get; set; }
        public DbSet<User> Users { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure One-to-Many Relationship
            modelBuilder.Entity<WorkoutPlan>()
                .HasOne(wp => wp.Trainer)
                .WithMany(t => t.WorkoutPlans)
                .HasForeignKey(wp => wp.TrainerId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
