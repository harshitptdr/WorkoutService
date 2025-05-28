using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkoutService.AzureServices;
using WorkoutService.Logging;
using WorkoutService.Models;
using WorkoutService.Services;

namespace WorkoutService.Controllers
{

    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class WorkoutController : ControllerBase
    {
        private readonly IWorkoutService _workoutService;
        private readonly IBlobService _blobService;
        private readonly Logger _logger;

        public WorkoutController(IWorkoutService workoutService, IBlobService blobService, Logger logger)
        {
            _workoutService = workoutService;
            _blobService = blobService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllWorkouts()
        {
            try
            {
                _logger.Log("Starting to fetch all workouts.");
                var workouts = await _workoutService.GetAllWorkoutsAsync();
                _logger.Log("Successfully fetched all workouts.");
                return Ok(workouts);
            }
            catch (Exception ex)
            {
                _logger.Log($"Error occurred while fetching all workouts. Exception: {ex.Message} | StackTrace: {ex.StackTrace}");
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpGet("by-id/{id}")]
        public async Task<IActionResult> GetWorkoutById(int id)
        {
            try
            {
                _logger.Log($"Fetching workout with Id: {id}");
                var workout = await _workoutService.GetWorkoutByIdAsync(id);
                if (workout == null)
                {
                    _logger.Log($"Workout with Id: {id} not found.");
                    return NotFound();
                }

                _logger.Log($"Successfully fetched workout with Id: {id}");
                return Ok(workout);
            }
            catch (Exception ex)
            {
                _logger.Log($"Error occurred while fetching workout with Id: {id}. Exception: {ex.Message} | StackTrace: {ex.StackTrace}");
                return StatusCode(500, "Internal server error.");
            }
        }
        [HttpGet("by-user/{userId}")]
        public async Task<IActionResult> GetWorkoutByUserId(int userId)
        {
            try
            {
                _logger.Log($"Fetching workout with UserId: {userId}");
                var workout = await _workoutService.GetByUserIdAsync(userId);
                if (workout == null)
                {
                    _logger.Log($"Workout with UserId: {userId} not found.");
                    return NotFound();
                }

                _logger.Log($"Successfully fetched workout with UserId: {userId}");
                return Ok(workout);
            }
            catch (Exception ex)
            {
                _logger.Log($"Error occurred while fetching workout with UserId: {userId}. Exception: {ex.Message} | StackTrace: {ex.StackTrace}");
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddWorkout([FromForm] WorkoutDto workoutDto, IFormFile? image)
        {

            try
            {
                _logger.Log("Attempting to add a new workout with image.");
                var imageUrl = image != null
                    ? await _blobService.UploadProgressImageAsync(image)
                    : null;

                var workout = new Workout
                {
                    UserId = workoutDto.UserId,
                    ExerciseType = workoutDto.ExerciseType,
                    Duration = workoutDto.Duration,
                    CaloriesBurned = workoutDto.CaloriesBurned,
                    ProgressImage = imageUrl
                };

                await _workoutService.AddWorkoutAsync(workout);
                _logger.Log($"Successfully added workout with Id: {workout.Id}");
                return Ok(new { message = "Workout added successfully." });
            }

            catch (Exception ex)
            {
                _logger.Log($"Error occurred while adding a new workout. Exception: {ex.Message} | StackTrace: {ex.StackTrace}");
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateWorkout(int id, [FromBody] Workout workout)
        {
            try
            {
                if (id != workout.Id)
                {
                    _logger.Log($"Workout ID mismatch: URL Id = {id}, Body Id = {workout.Id}");
                    return BadRequest("Workout ID mismatch.");
                }

                _logger.Log($"Attempting to update workout with Id: {id}");
                await _workoutService.UpdateWorkoutAsync(workout);
                _logger.Log($"Successfully updated workout with Id: {id}");
                //return NoContent();
                return Ok(new { message = "Workout updated successfully." });
            }
            catch (Exception ex)
            {
                _logger.Log($"Error occurred while updating workout with Id: {id}. Exception: {ex.Message} | StackTrace: {ex.StackTrace}");
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWorkout(int id)
        {
            try
            {
                _logger.Log($"Attempting to delete workout with Id: {id}");
                var workout = await _workoutService.GetWorkoutByIdAsync(id);
                if (workout == null)
                {
                    _logger.Log($"Workout with Id: {id} not found for deletion.");
                    return NotFound();
                }

                await _workoutService.DeleteWorkoutAsync(workout);
                _logger.Log($"Successfully deleted workout with Id: {id}");
                //return NoContent();
                return Ok(new { message = "Workout deleted successfully." });
            }
            catch (Exception ex)
            {
                _logger.Log($"Error occurred while deleting workout with Id: {id}. Exception: {ex.Message} | StackTrace: {ex.StackTrace}");
                return StatusCode(500, "Internal server error.");
            }
        }
    }

}