using System.ComponentModel.DataAnnotations;

namespace WorkoutService.Models
{
    public class WorkoutDto
    {
        [Required(ErrorMessage = "UserId is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "UserId must be a positive integer.")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "ExerciseType is required.")]
        [StringLength(100, ErrorMessage = "ExerciseType can't exceed 100 characters.")]
        public string ExerciseType { get; set; }

        [Required(ErrorMessage = "Duration is required.")]
        [Range(1, 180, ErrorMessage = "Duration must be between 1 and 1440 minutes.")]
        public int Duration { get; set; }

        [Required(ErrorMessage = "CaloriesBurned is required.")]
        [Range(1, 1000, ErrorMessage = "CaloriesBurned must be between 1 and 10000.")]
        public int CaloriesBurned { get; set; }
    }
}
