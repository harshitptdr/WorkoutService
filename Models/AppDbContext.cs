using Microsoft.EntityFrameworkCore;

namespace WorkoutService.Models;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Workout> Workouts { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<Workout>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__workouts__3213E83F4A4786F9");

            entity.ToTable("workouts");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CaloriesBurned).HasColumnName("calories_burned");
            entity.Property(e => e.Duration).HasColumnName("duration");
            entity.Property(e => e.ExerciseType)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("exercise_type");
            entity.Property(e => e.ProgressImage)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("Progress_Image");
            entity.Property(e => e.WorkoutDate)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime")
                .HasColumnName("workout_date");
            entity.Property(e => e.UserId).HasColumnName("user_id");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
