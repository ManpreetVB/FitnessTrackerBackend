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
    }
}
