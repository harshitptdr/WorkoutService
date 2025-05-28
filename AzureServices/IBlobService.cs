namespace WorkoutService.AzureServices
{
    public interface IBlobService
    {
        Task<string?> UploadProgressImageAsync(IFormFile image);
    }
}
