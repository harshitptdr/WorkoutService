namespace WorkoutService.Models
{
    public class QueueMessageModel
    {
        public string Action { get; set; }
        public int UserId { get; set; }
        public int InitiatedBy { get; set; }
    }

}
