
namespace WorkoutService.Models;

public partial class User
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? FitnessGoal { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string Role { get; set; } = null!;
}
