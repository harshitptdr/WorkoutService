using WorkoutService.Models;

namespace WorkoutService.Services
{
    public interface IWorkoutService
    {
        Task<IEnumerable<Workout>> GetAllWorkoutsAsync();
        Task<Workout?> GetWorkoutByIdAsync(int id);
        Task<IEnumerable<Workout>> GetByUserIdAsync(int userId);
        Task AddWorkoutAsync(Workout workout);
        Task UpdateWorkoutAsync(Workout workout);
        Task DeleteWorkoutAsync(Workout workout);
        Task SoftDeleteWorkoutsByUserAsync(int userId, int updatedById, int isActive);


    }
}
