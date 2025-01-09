namespace Fitness_Tracker.Models
{
    public class Trainer
    {
        public int TrainerId { get; set; }
        public string Name { get; set; }
        public string Expertise { get; set; }
        public string ContactNumber { get; set; }
        public string Email { get; set; }
        public string Specialization { get; set; }


        public ICollection<WorkoutPlan> WorkoutPlans { get; set; }
    }
}
