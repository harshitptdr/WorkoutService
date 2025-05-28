namespace WorkoutService.Logging
{
    public class Logger
    {
        private readonly string _logFilePath = "logs.txt";

        public void Log(string message)
        {
            try
            {
                var logMessage = $"{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} - {message}";
                File.AppendAllText(_logFilePath, logMessage + Environment.NewLine);
            }
            catch (Exception ex)
            {
                // If logging fails, at least try to log to the console
                Console.WriteLine($"Error logging message: {ex.Message}");
            }
        }
    }
}
