namespace Fitness_Tracker.Models
{
    public class Member
    {
        public int MemberId { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string ContactNumber { get; set; }
        public string Email { get; set; }
        public DateTime MembershipStartDate { get; set; }
        public DateTime MembershipEndDate { get; set; }
        public string FitnessGoal { get; set; }
    }
}
