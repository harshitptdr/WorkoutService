using WorkoutService.Logging;
using WorkoutService.Models;
using WorkoutService.Repository;

namespace WorkoutService.Services
{
    public class WorkoutsService : IWorkoutService
    {
        private readonly IWorkoutRepository _workoutRepository;
        private readonly Logger _logger;

        public WorkoutsService(IWorkoutRepository workoutRepository, Logger logger)
        {
            _workoutRepository = workoutRepository;
            _logger = logger;
        }
        public async Task<IEnumerable<Workout>> GetAllWorkoutsAsync()
        {
            try
            {
                _logger.Log("Fetching all workouts from repository.");
                var workouts = await _workoutRepository.GetAllWorkoutsAsync();
                _logger.Log("Successfully fetched workouts from repository.");
                return workouts;
            }
            catch (Exception ex)
            {
                _logger.Log($"Error occurred while fetching all workouts from repository. Exception: {ex.Message} | StackTrace: {ex.StackTrace}");
                throw;
            }
        }
        public async Task SoftDeleteWorkoutsByUserAsync(int userId, int updatedById, int isActive)
        {
            await _workoutRepository.SoftDeleteWorkoutsByUserAsync(userId, updatedById, isActive);
        }


        public async Task<Workout?> GetWorkoutByIdAsync(int id)
        {
            try
            {
                _logger.Log($"Fetching workout with Id: {id} from repository.");
                var workout = await _workoutRepository.GetWorkoutByIdAsync(id);
                _logger.Log($"Successfully fetched workout with Id: {id} from repository.");
                return workout;
            }
            catch (Exception ex)
            {
                _logger.Log($"Error occurred while fetching workout with Id: {id} from repository. Exception: {ex.Message} | StackTrace: {ex.StackTrace}");
                throw;
            }
        }
        public async Task<IEnumerable<Workout>> GetByUserIdAsync(int userId)
        {
            try
            {
                _logger.Log($"Fetching workout with UserId: {userId} from repository.");
                var workout = await _workoutRepository.GetWorkoutsByUserIdAsync(userId);
                _logger.Log($"Successfully fetched workout with UserId: {userId} from repository.");
                return workout;
            }
            catch (Exception ex)
            {
                _logger.Log($"Error occurred while fetching workout with UserId: {userId} from repository. Exception: {ex.Message} | StackTrace: {ex.StackTrace}");
                throw;
            }
        }

        public async Task AddWorkoutAsync(Workout workout)
        {
            try
            {
                _logger.Log($"Adding new workout to repository. Id: {workout.Id}");
                await _workoutRepository.AddWorkoutAsync(workout);
                _logger.Log($"Successfully added workout with Id: {workout.Id} to repository.");
            }
            catch (Exception ex)
            {
                _logger.Log($"Error occurred while adding workout to repository. Id: {workout.Id}. Exception: {ex.Message} | StackTrace: {ex.StackTrace}");
                throw;
            }
        }

        public async Task UpdateWorkoutAsync(Workout workout)
        {
            try
            {
                _logger.Log($"Updating workout with Id: {workout.Id} in repository.");
                await _workoutRepository.UpdateWorkoutAsync(workout);
                _logger.Log($"Successfully updated workout with Id: {workout.Id} in repository.");
            }
            catch (Exception ex)
            {
                _logger.Log($"Error occurred while updating workout with Id: {workout.Id} in repository. Exception: {ex.Message} | StackTrace: {ex.StackTrace}");
                throw;
            }
        }

        public async Task DeleteWorkoutAsync(Workout workout)
        {
            try
            {
                _logger.Log($"Deleting workout with Id: {workout.Id} from repository.");
                await _workoutRepository.DeleteWorkoutAsync(workout);
                _logger.Log($"Successfully deleted workout with Id: {workout.Id} from repository.");
            }
            catch (Exception ex)
            {
                _logger.Log($"Error occurred while deleting workout with Id: {workout.Id} from repository. Exception: {ex.Message} | StackTrace: {ex.StackTrace}");
                throw;
            }
        }
    }
}
