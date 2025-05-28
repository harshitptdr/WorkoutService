namespace WorkoutService.AzureServices
{
    public interface IReadQueueService
    {
        Task ListenToQueueAndProcessAsync();

    }
}
