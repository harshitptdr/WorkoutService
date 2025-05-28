using System.Text;
using Azure.Storage.Queues;
using Newtonsoft.Json;
using WorkoutService.Services;
using WorkoutService.Logging;

namespace WorkoutService.AzureServices
{
    public class ReadQueueService : BackgroundService
    {
        private readonly Logger _logger;
        private readonly IConfiguration _config;
        private readonly IServiceProvider _serviceProvider; // Inject IServiceProvider to resolve scoped services

        public ReadQueueService(Logger logger, IConfiguration config, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _config = config;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                var client = new QueueClient(
                    _config["AzureStorage:ConnectionString"],
                    _config["AzureStorage:QueueName"]
                );

                // Ensure the queue exists (for local development)
                await client.CreateIfNotExistsAsync();

                // Polling for messages (you can adjust the interval for real-time processing)
                while (!stoppingToken.IsCancellationRequested)
                {
                    var message = await client.ReceiveMessageAsync();

                    if (message.Value != null)
                    {
                        // Dequeue and process message
                        var base64Message = message.Value.MessageText;
                        var jsonMessage = Encoding.UTF8.GetString(Convert.FromBase64String(base64Message));

                        var messageObj = JsonConvert.DeserializeObject<dynamic>(jsonMessage);

                        // Log message for traceability
                        _logger.Log($"Received message: {jsonMessage}");

                        // Handle the action based on message (SoftDelete or Activate)
                        if (messageObj.Action == "Deactivate User" || messageObj.Action == "Activate User")
                        {
                            int userId = messageObj.UserId;
                            int updatedById = messageObj.InitiatedByUserId;

                            // Resolve the scoped IWorkoutService via the service provider
                            using (var scope = _serviceProvider.CreateScope())
                            {
                                var workoutService = scope.ServiceProvider.GetRequiredService<IWorkoutService>();

                                // Soft delete or update the workout status (set IsActive)
                                if (messageObj.Action == "Deactivate User")
                                {
                                    await workoutService.SoftDeleteWorkoutsByUserAsync(userId, updatedById, 0); // Set IsActive to false
                                }
                                else
                                {
                                    await workoutService.SoftDeleteWorkoutsByUserAsync(userId, updatedById, 1); // Set IsActive to true
                                }
                            }

                            // Delete the message after processing
                            await client.DeleteMessageAsync(message.Value.MessageId, message.Value.PopReceipt);
                        }
                    }

                    // Add a delay for queue polling
                    await Task.Delay(5000, stoppingToken); // Wait for 5 seconds before checking again
                }
            }
            catch (Exception ex)
            {
                _logger.Log($"Error while processing queue: {ex.Message}");
            }
        }
    }
}
