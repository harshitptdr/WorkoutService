using WorkoutService.Models;

namespace WorkoutService.Repository
{
    public interface IWorkoutRepository
    {
        Task<IEnumerable<Workout>> GetAllWorkoutsAsync();
        Task<IEnumerable<Workout>> GetWorkoutsByUserIdAsync(int userId);
        Task<Workout?> GetWorkoutByIdAsync(int id);
        Task AddWorkoutAsync(Workout workout);
        Task UpdateWorkoutAsync(Workout workout);
        Task DeleteWorkoutAsync(Workout workout);
        Task SoftDeleteWorkoutsByUserAsync(int userId, int updatedById, int isActive);
    }
}
