namespace WorkoutService.Models;

public partial class Workout
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public string ExerciseType { get; set; } = null!;

    public int Duration { get; set; }

    public int CaloriesBurned { get; set; }

    public string? ProgressImage { get; set; }

    public DateTime? WorkoutDate { get; set; }

}
