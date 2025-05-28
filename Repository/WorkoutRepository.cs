using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using WorkoutService.Constants;
using WorkoutService.Logging;
using WorkoutService.Models;

namespace WorkoutService.Repository
{
    public class WorkoutRepository : IWorkoutRepository
    {
        private readonly AppDbContext _context;
        private readonly Logger _logger;

        public WorkoutRepository(AppDbContext context, Logger logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<Workout>> GetAllWorkoutsAsync()
        {
            try
            {
                _logger.Log($"Executing stored procedure: {ApplicationConstants.UspGetAllWorkouts}");
                var workouts = await _context.Workouts
                    .FromSqlRaw($"EXEC {ApplicationConstants.UspGetAllWorkouts}")
                    .ToListAsync();
                _logger.Log("Successfully fetched all workouts from database.");
                return workouts;
            }
            catch (Exception ex)
            {
                _logger.Log($"Error occurred while fetching all workouts. Exception: {ex.Message} | StackTrace: {ex.StackTrace}");
                throw;
            }
        }

        public async Task SoftDeleteWorkoutsByUserAsync(int userId, int updatedById, int isActive)
        {
            try
            {
                _logger.Log($"Executing stored procedure: {ApplicationConstants.UspUpdateWorkoutIsActive} for Id: {userId}");
                var parameters = new[]
                {
                    new SqlParameter(ApplicationConstants.ParamUserId, userId),
                    new SqlParameter(ApplicationConstants.ParamUpdatedByUserId, updatedById),
                    new SqlParameter(ApplicationConstants.ParamIsActive, isActive),
                };

                await _context.Database.ExecuteSqlRawAsync(
                    $"EXEC {ApplicationConstants.UspUpdateWorkoutIsActive} {ApplicationConstants.ParamUserId}, {ApplicationConstants.ParamUpdatedByUserId}, {ApplicationConstants.ParamIsActive}",
                    parameters);

                _logger.Log($"Successfully updated workouts IsActive status with Id: {userId}");
            }
            catch (Exception ex)
            {
                _logger.Log($"Error updating workouts IsActive status with Id: {userId}. Exception: {ex.Message} | StackTrace: {ex.StackTrace}");
                throw;
            }
        }

        public async Task<IEnumerable<Workout>> GetWorkoutsByUserIdAsync(int userId)
        {
            try
            {
                _logger.Log($"Executing stored procedure: {ApplicationConstants.UspGetWorkoutsByUserId} for UserId: {userId}");
                var workouts = await _context.Workouts
                    .FromSqlRaw($"EXEC {ApplicationConstants.UspGetWorkoutsByUserId} {ApplicationConstants.ParamUserId}",
                        new SqlParameter(ApplicationConstants.ParamUserId, userId))
                    .ToListAsync();
                _logger.Log($"Successfully fetched workouts for UserId: {userId}");
                return workouts;
            }
            catch (Exception ex)
            {
                _logger.Log($"Error fetching workouts for UserId: {userId}. Exception: {ex.Message} | StackTrace: {ex.StackTrace}");
                throw;
            }
        }

        public async Task<Workout?> GetWorkoutByIdAsync(int id)
        {
            try
            {
                _logger.Log($"Executing stored procedure: {ApplicationConstants.UspGetWorkoutById} for Id: {id}");
                var result = await _context.Workouts
                    .FromSqlRaw($"EXEC {ApplicationConstants.UspGetWorkoutById} {ApplicationConstants.ParamId}",
                        new SqlParameter(ApplicationConstants.ParamId, id))
                    .ToListAsync();
                _logger.Log($"Successfully fetched workout with Id: {id}");
                return result.FirstOrDefault();
            }
            catch (Exception ex)
            {
                _logger.Log($"Error fetching workout with Id: {id}. Exception: {ex.Message} | StackTrace: {ex.StackTrace}");
                throw;
            }
        }

        public async Task AddWorkoutAsync(Workout workout)
        {
            try
            {
                _logger.Log($"Executing stored procedure: {ApplicationConstants.UspAddWorkout} for UserId: {workout.UserId}");
                var parameters = new[]
                {
                    new SqlParameter(ApplicationConstants.ParamUserId, workout.UserId),
                    new SqlParameter(ApplicationConstants.ParamExerciseType, workout.ExerciseType),
                    new SqlParameter(ApplicationConstants.ParamDuration, workout.Duration),
                    new SqlParameter(ApplicationConstants.ParamCaloriesBurned, workout.CaloriesBurned),
                    new SqlParameter(ApplicationConstants.ParamProgressImage, workout.ProgressImage ?? (object)DBNull.Value)
                };

                await _context.Database.ExecuteSqlRawAsync(
                    $"EXEC {ApplicationConstants.UspAddWorkout} {ApplicationConstants.ParamUserId}, {ApplicationConstants.ParamExerciseType}, {ApplicationConstants.ParamDuration}, {ApplicationConstants.ParamCaloriesBurned}, {ApplicationConstants.ParamProgressImage}",
                    parameters);

                _logger.Log($"Workout added successfully for UserId: {workout.UserId}");
            }
            catch (Exception ex)
            {
                _logger.Log($"Error adding workout. UserId: {workout.UserId}. Exception: {ex.Message} | StackTrace: {ex.StackTrace}");
                throw;
            }
        }

        public async Task UpdateWorkoutAsync(Workout workout)
        {
            try
            {
                _logger.Log($"Executing stored procedure: {ApplicationConstants.UspUpdateWorkout} for Id: {workout.Id}");
                var parameters = new[]
                {
                    new SqlParameter(ApplicationConstants.ParamId, workout.Id),
                    new SqlParameter(ApplicationConstants.ParamUserId, workout.UserId),
                    new SqlParameter(ApplicationConstants.ParamExerciseType, workout.ExerciseType),
                    new SqlParameter(ApplicationConstants.ParamDuration, workout.Duration),
                    new SqlParameter(ApplicationConstants.ParamCaloriesBurned, workout.CaloriesBurned),
                    new SqlParameter(ApplicationConstants.ParamProgressImage, workout.ProgressImage ?? (object)DBNull.Value)
                };

                await _context.Database.ExecuteSqlRawAsync(
                    $"EXEC {ApplicationConstants.UspUpdateWorkout} {ApplicationConstants.ParamId}, {ApplicationConstants.ParamUserId}, {ApplicationConstants.ParamExerciseType}, {ApplicationConstants.ParamDuration}, {ApplicationConstants.ParamCaloriesBurned}, {ApplicationConstants.ParamProgressImage}",
                    parameters);

                _logger.Log($"Successfully updated workout with Id: {workout.Id}");
            }
            catch (Exception ex)
            {
                _logger.Log($"Error updating workout with Id: {workout.Id}. Exception: {ex.Message} | StackTrace: {ex.StackTrace}");
                throw;
            }
        }

        public async Task DeleteWorkoutAsync(Workout workout)
        {
            try
            {
                _logger.Log($"Executing stored procedure: {ApplicationConstants.UspDeleteWorkout} for Id: {workout.Id}");
                var param = new SqlParameter(ApplicationConstants.ParamId, workout.Id);
                await _context.Database.ExecuteSqlRawAsync(
                    $"EXEC {ApplicationConstants.UspDeleteWorkout} {ApplicationConstants.ParamId}",
                    param);
                _logger.Log($"Successfully deleted workout with Id: {workout.Id}");
            }
            catch (Exception ex)
            {
                _logger.Log($"Error deleting workout with Id: {workout.Id}. Exception: {ex.Message} | StackTrace: {ex.StackTrace}");
                throw;
            }
        }
    }
}
