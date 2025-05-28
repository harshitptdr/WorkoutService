namespace WorkoutService.Constants
{
    public static class ApplicationConstants
    {
        // Stored procedure names
        public const string UspGetAllWorkouts = "usp_GetAllWorkouts";
        public const string UspUpdateWorkoutIsActive = "usp_UpdateWorkoutIsActive";
        public const string UspGetWorkoutsByUserId = "usp_GetWorkoutsByUserId";
        public const string UspGetWorkoutById = "usp_GetWorkoutById";
        public const string UspAddWorkout = "usp_AddWorkout";
        public const string UspUpdateWorkout = "usp_UpdateWorkout";
        public const string UspDeleteWorkout = "usp_DeleteWorkout";

        // SQL parameter names
        public const string ParamUserId = "@UserId";
        public const string ParamUpdatedByUserId = "@UpdatedByUserId";
        public const string ParamIsActive = "@IsActive";
        public const string ParamId = "@Id";
        public const string ParamExerciseType = "@ExerciseType";
        public const string ParamDuration = "@Duration";
        public const string ParamCaloriesBurned = "@CaloriesBurned";
        public const string ParamProgressImage = "@ProgressImage";
    }
}
