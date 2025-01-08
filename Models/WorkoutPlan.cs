namespace Fitness_Tracker.Models
{
    public class WorkoutPlan
    {
        public int WorkoutPlanId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int DurationInDays { get; set; }
        public int TrainerId { get; set; }
        public Trainer Trainer { get; set; }
    }
}
